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

        protected ConfigurationDataContainer Data;

        #region Auxilary methods

        private void SetLocaledText()
        {
            _enabledCheckbox.Text = "ConfigurationPanel.AutoEmptying.Enabled".Translate();
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
            _enabledCheckbox.EventCheckChanged += (component, value) => { Data.AutoEmptyingEnabled = value; };

            // --- "filled %" slider
            _percentSlider = AddUIComponent<UISliderContainerPanel>();
            _percentSlider.IconAtlas = resourceManager.Atlas;
            _percentSlider.IconSprite = "DimensionIcon";
            _percentSlider.ValueFormat = "###'%'";
            _percentSlider.MinValue = 1f;
            _percentSlider.MaxValue = 100f;
            _percentSlider.StepSize = 1f;
            _percentSlider.EventValueChanged += (component, value) => { Data.EmptyingPercentStart = value; };

            // --- "emptying timespan" slider
            _timeRange = AddUIComponent<UIRangePickerPanel>();
            _timeRange.IconAtlas = resourceManager.Atlas;
            _timeRange.IconSprite = "ClockIcon";
            _timeRange.ValueFormat = "0#.00";
            _timeRange.MinValue = 0;
            _timeRange.MaxValue = 24f;
            _timeRange.StepSize = 1f;
            _timeRange.EventStartValueChanged += (component, value) => { Data.EmptyingTimeStart = value; };
            _timeRange.EventEndValueChanged += (component, value) => { Data.EmptyingTimeEnd = value; };

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
            height = 160;

            _percentSlider.width = width - padding.horizontal;
            _timeRange.width = width - padding.horizontal;
            
            // update displayed values
            _timeRange.StartValue = Data.EmptyingTimeStart;
            _timeRange.EndValue = Data.EmptyingTimeEnd;
            _percentSlider.Value = Data.EmptyingPercentStart;
        }
    }
}
