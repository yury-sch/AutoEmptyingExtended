using ColossalFramework.UI;
using UnityEngine;

namespace AutoEmptyingExtended.UI
{
    public class UIEmptyingInfoViewPanel : UIPanel
    {
        public override void Start()
        {
            base.Start();

            var resourceManager = ResourceManager.Instance;

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
            var enabledCheckbox = this.AddUIComponent<UICheckboxContainer>();

            var percentSlider = this.AddUIComponent<UISliderContainer>();
            percentSlider.IconAtlas = resourceManager.Atlas;
            percentSlider.IconSprite = "DimensionIcon";
            percentSlider.ValueFormat = "###'%'";
            percentSlider.MinValue = 1f;
            percentSlider.MaxValue = 100f;
            percentSlider.StepSize = 1f;
            percentSlider.width = this.width - this.padding.horizontal;

            var timeRange = this.AddUIComponent<UIRangePicker>();
            timeRange.IconAtlas = resourceManager.Atlas;
            timeRange.IconSprite = "ClockIcon";
            timeRange.ValueFormat = "0#.00";
            timeRange.MinValue = 0;
            timeRange.MaxValue = 24f;
            timeRange.StepSize = 1f;
            timeRange.width = this.width - this.padding.horizontal;

            //calculate height
            this.height = this.padding.vertical
                + enabledCheckbox.height + this.autoLayoutPadding.vertical
                + percentSlider.height + this.autoLayoutPadding.vertical
                + timeRange.height + this.autoLayoutPadding.vertical;
        }
    }
}
