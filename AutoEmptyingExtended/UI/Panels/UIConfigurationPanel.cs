using AutoEmptyingExtended.Data;
using AutoEmptyingExtended.UI.Localization;
using ColossalFramework.UI;
using UnityEngine;

namespace AutoEmptyingExtended.UI.Panels
{
    #region Class wrappers

    public class UICemetaryEmptyingPanel : UIConfigurationPanel
    {
        public override void Awake()
        {
            Data = ConfigurationDataManager.Data.Cemetary;

            base.Awake();
        }
    }

    public class UILandfillEmptyingPanel : UIConfigurationPanel
    {
        public override void Awake()
        {
            Data = ConfigurationDataManager.Data.Landfill;

            base.Awake();
        }
    }

    #endregion

    /// <summary>
    /// Panel appearing while a cemetry/landfill info view is selected
    /// </summary>
    public abstract class UIConfigurationPanel : UIPanel
    {
        private UICheckboxContainerPanel _enabledCheckbox;
        private UIRangePickerPanel _percentRange;
        private UIRangePickerPanel _timeRange;
        private UILabel _summaryLabel;

        private string _summaryFormat;
        private const string TimeFormat = "0#.00";
        private const string FilledFormat = "###'%'";

        protected ConfigurationDataContainer Data;

        #region Auxilary methods

        private void SetLocaledText()
        {
            _enabledCheckbox.Text = "ConfigurationPanel.AutoEmptying.Enabled".Translate();
            _summaryFormat = "ConfigurationPanel.AutoEmptying.Summary".Translate();
        }

        private string FormatSummaryValue()
        {
            return string.Format(_summaryFormat,
                    Data.EmptyingTimeStart.ToString(TimeFormat),
                    Data.EmptyingTimeEnd.ToString(TimeFormat),
                    Data.EmptyingPercentStart.ToString(FilledFormat));
        }

        #endregion

        public override void Awake()
        {
            base.Awake();
            
            var resourceManager = TextureManager.Instance;

            // configure the panel itself
            padding = new RectOffset(10, 10, 5, 5);
            backgroundSprite = "MenuPanel2";

            autoLayout = true;
            autoLayoutDirection = LayoutDirection.Vertical;
            autoLayoutPadding = new RectOffset(0, 0, 10, 10);
            autoLayoutStart = LayoutStart.TopLeft;

            // add sub-components
            // --- "schedule cleaning" checkbox
            _enabledCheckbox = AddUIComponent<UICheckboxContainerPanel>();

            // --- "filled %" slider
            _percentRange = AddUIComponent<UIRangePickerPanel>();
            _percentRange.IconAtlas = resourceManager.Atlas;
            _percentRange.IconSprite = "DimensionIcon";
            _percentRange.ValueFormat = FilledFormat;
            _percentRange.MinValue = 1f;
            _percentRange.MaxValue = 100f;
            _percentRange.StepSize = 1f;

            // --- "emptying timespan" slider
            _timeRange = AddUIComponent<UIRangePickerPanel>();
            _timeRange.IconAtlas = resourceManager.Atlas;
            _timeRange.IconSprite = "ClockIcon";
            _timeRange.ValueFormat = TimeFormat;
            _timeRange.MinValue = 0;
            _timeRange.MaxValue = 24f;
            _timeRange.StepSize = 1f;

            // --- "summary" label
            _summaryLabel = AddUIComponent<UILabel>();
            _summaryLabel.textColor = new Color32(206, 248, 0, 255);

            // manage mod localization
            SetLocaledText();
            LocalizationManager.Instance.EventLocaleChanged += language => SetLocaledText();
        }

        public override void Start()
        {
            base.Start();

            // adjust UI elements size
            position = new Vector3(0, -parent.height + 1);
            width = parent.width;
            height = 214;

            _percentRange.width = width - padding.horizontal;
            _timeRange.width = width - padding.horizontal;
            _summaryLabel.width = width - padding.horizontal;
            _summaryLabel.autoHeight = true;
            _summaryLabel.wordWrap = true;

            // update displayed values
            _enabledCheckbox.Checked = !Data.AutoEmptyingDisabled;
            _timeRange.EndValue = Data.EmptyingTimeEnd;     // IMPORTANT: init EndValue before StartValue
            _timeRange.StartValue = Data.EmptyingTimeStart;
            _percentRange.EndValue = Data.EmptyingPercentStart;
            _percentRange.StartValue = Data.EmptyingPercentStop;

            // add events
            _enabledCheckbox.EventCheckChanged += (component, value) => { Data.AutoEmptyingDisabled = !value; };
            _timeRange.EventStartValueChanged += (component, value) =>
            {
                Data.EmptyingTimeStart = value;
                _summaryLabel.text = FormatSummaryValue();
            };
            _timeRange.EventEndValueChanged += (component, value) => {
                Data.EmptyingTimeEnd = value;
                _summaryLabel.text = FormatSummaryValue();
            };
            //guarantee that only valid values are assigned and make some sense
            //start value == percentage to stop
            _percentRange.EventStartValueChanged += (component, value) =>
            {
                if (value <= (_percentRange.EndValue - 5f))
                {
                    Data.EmptyingPercentStop = value;
                }
                else if (value > _percentRange.EndValue - 5f)
                {
                    value = _percentRange.EndValue - 5f;
                    _percentRange.StartValue = value;
                    Data.EmptyingPercentStop = value;
                }

                _summaryLabel.text = FormatSummaryValue();
            };
            //end value == percentage to start
            _percentRange.EventEndValueChanged += (component, value) =>
            {
                if (value >= (_percentRange.StartValue + 5f))
                {
                    Data.EmptyingPercentStart = value;
                }
                else if(value < _percentRange.StartValue + 5f)
                {
                    value = _percentRange.StartValue + 5f;
                    Data.EmptyingPercentStop = value;
                    _percentRange.EndValue = value;
                }
                _summaryLabel.text = FormatSummaryValue();
                Data.HasJustChanged = true;

            };

            // show summary
            _summaryLabel.text = FormatSummaryValue();
        }
    }
}
