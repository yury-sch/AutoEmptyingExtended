using AutoEmptyingExtended.Data;
using AutoEmptyingExtended.UI.Localization;
using ColossalFramework.UI;
using UnityEngine;

namespace AutoEmptyingExtended.UI.Panels
{
    public abstract class UIConfigurationPanel : UIPanel
    {
        private UICheckboxContainer _enabledCheckbox;
        private UISliderContainer _percentSlider;
        private UIRangePicker _timeRange;

        protected ConfigurationDataContainer Data;


        #region Auxilary methods

        private void UpdateUILabels()
        {
            Logger.LogInGame("UpdateUILabels()");
            _enabledCheckbox.Checked = Data.AutoEmptyingEnabled;
            _percentSlider.Value = Data.EmptyingPercentStart;
            _timeRange.StartValue = Data.EmptyingTimeStart;
            _timeRange.EndValue = Data.EmptyingTimeEnd;
        }

        private void SetLocaledText()
        {
            _enabledCheckbox.Text = "ConfigurationPanel.AutoEmptying.Enabled".Translate();
        }

        #endregion


        public override void Awake()
        {
            base.Awake();
            Logger.LogInGame("UIConfigurationPanel: Awake()");
            
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
            _enabledCheckbox = AddUIComponent<UICheckboxContainer>();
            _enabledCheckbox.eventCheckChanged += (component, value) => { Data.AutoEmptyingEnabled = value; };

            // --- "filled %" slider
            _percentSlider = AddUIComponent<UISliderContainer>();
            _percentSlider.IconAtlas = resourceManager.Atlas;
            _percentSlider.IconSprite = "DimensionIcon";
            _percentSlider.ValueFormat = "###'%'";
            _percentSlider.MinValue = 1f;
            _percentSlider.MaxValue = 100f;
            _percentSlider.StepSize = 1f;
            _percentSlider.eventValueChanged += (component, value) => { Data.EmptyingPercentStart = value; };

            // --- "emptying timespan" slider
            _timeRange = AddUIComponent<UIRangePicker>();
            _timeRange.IconAtlas = resourceManager.Atlas;
            _timeRange.IconSprite = "ClockIcon";
            _timeRange.ValueFormat = "0#.00";
            _timeRange.MinValue = 0;
            _timeRange.MaxValue = 24f;
            _timeRange.StepSize = 1f;
            _timeRange.eventStartValueChanged += (component, value) => { Data.EmptyingTimeStart = value; };
            _timeRange.eventEndValueChanged += (component, value) => { Data.EmptyingTimeEnd = value; };

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
            
            // update values
            _timeRange.StartValue = Data.EmptyingTimeStart;
            _timeRange.EndValue = Data.EmptyingTimeEnd;
            _percentSlider.Value = Data.EmptyingPercentStart;
        }
    }
}
