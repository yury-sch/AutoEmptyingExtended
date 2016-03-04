using System;
using AutoEmptyingExtended.Data;
using ColossalFramework.UI;
using UnityEngine;

namespace AutoEmptyingExtended.UI
{
    public abstract class UIEmptyingPanel : UIPanel
    {
        private bool _initialized;

        private UICheckboxContainer _enabledCheckbox;
        private UISliderContainer _percentSlider;
        private UIRangePicker _timeRange;

        protected ConfigurationDataContainer Data;
        
        public override void Start()
        {
            base.Start();

            var resourceManager = TextureManager.Instance;

            // configure panel
            this.padding = new RectOffset(10, 10, 5, 10);
            this.width = this.parent.width;
            this.position = new Vector3(0, -this.parent.height + 1);
            this.backgroundSprite = "MenuPanel2";
            this.autoLayout = true;
            this.autoLayoutDirection = LayoutDirection.Vertical;
            this.autoLayoutPadding = new RectOffset(0, 0, 10, 5);
            this.autoLayoutStart = LayoutStart.TopLeft;

            // add sub-components
            _enabledCheckbox = this.AddUIComponent<UICheckboxContainer>();
            _enabledCheckbox.Text = "ConfigurationPanel.AutoEmptying.Enabled".Translate();
            _enabledCheckbox.eventCheckChanged += (component, value) =>
            {
                if (value != Data.AutoEmptyingEnabled)
                {
                    Data.AutoEmptyingEnabled = value;
                }
            };

            _percentSlider = this.AddUIComponent<UISliderContainer>();
            _percentSlider.IconAtlas = resourceManager.Atlas;
            _percentSlider.IconSprite = "DimensionIcon";
            _percentSlider.ValueFormat = "###'%'";
            _percentSlider.MinValue = 1f;
            _percentSlider.MaxValue = 100f;
            _percentSlider.StepSize = 1f;
            _percentSlider.width = this.width - this.padding.horizontal;
            _percentSlider.eventValueChanged += (component, value) => 
            {
                if (value != Data.EmptyingPercentStart)
                {
                    Data.EmptyingPercentStart = value;
                }
            };

            _timeRange = this.AddUIComponent<UIRangePicker>();
            _timeRange.IconAtlas = resourceManager.Atlas;
            _timeRange.IconSprite = "ClockIcon";
            _timeRange.ValueFormat = "0#.00";
            _timeRange.MinValue = 0;
            _timeRange.MaxValue = 24f;
            _timeRange.StepSize = 1f;
            _timeRange.width = this.width - this.padding.horizontal;
            _timeRange.eventStartValueChanged += (component, value) =>
            {
                if (value != Data.EmptyingTimeStart)
                {
                    Data.EmptyingTimeStart = value;
                }
            };
            _timeRange.eventEndValueChanged += (component, value) =>
            {
                if (value != Data.EmptyingTimeEnd)
                {
                    Data.EmptyingTimeEnd = value;
                }
            };

            //calculate height
            this.height = this.padding.vertical
                + _enabledCheckbox.height + this.autoLayoutPadding.vertical
                + _percentSlider.height + this.autoLayoutPadding.vertical
                + _timeRange.height + this.autoLayoutPadding.vertical;
        }

        public override void Update()
        {
            base.Update();

            if (_initialized)
                return;

            _enabledCheckbox.Checked = Data.AutoEmptyingEnabled;
            _percentSlider.Value = Data.EmptyingPercentStart;
            _timeRange.StartValue = Data.EmptyingTimeStart;
            _timeRange.EndValue = Data.EmptyingTimeEnd;

            _initialized = true;
        }
    }
}
