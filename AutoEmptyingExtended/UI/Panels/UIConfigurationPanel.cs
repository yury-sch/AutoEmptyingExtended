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
        private UISliderContainerPanel _percentSlider;
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
            _percentSlider = AddUIComponent<UISliderContainerPanel>();
            _percentSlider.IconAtlas = resourceManager.Atlas;
            _percentSlider.IconSprite = "DimensionIcon";
            _percentSlider.ValueFormat = FilledFormat;
            _percentSlider.MinValue = 1f;
            _percentSlider.MaxValue = 100f;
            _percentSlider.StepSize = 1f;

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

            _percentSlider.width = width - padding.horizontal;
            _timeRange.width = width - padding.horizontal;
            _summaryLabel.width = width - padding.horizontal;
            _summaryLabel.autoHeight = true;
            _summaryLabel.wordWrap = true;

            // update displayed values
            _enabledCheckbox.Checked = !Data.AutoEmptyingDisabled;
            _timeRange.EndValue = Data.EmptyingTimeEnd;     // IMPORTANT: init EndValue before StartValue
            _timeRange.StartValue = Data.EmptyingTimeStart;
            _percentSlider.Value = Data.EmptyingPercentStart;

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
            _percentSlider.EventValueChanged += (component, value) =>
            {
                Data.EmptyingPercentStart = value;
                _summaryLabel.text = FormatSummaryValue();
            };

            // show summary
            _summaryLabel.text = FormatSummaryValue();
        }
    }
}
