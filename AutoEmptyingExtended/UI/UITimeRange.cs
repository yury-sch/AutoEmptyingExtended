using ColossalFramework.UI;
using UnityEngine;

namespace AutoEmptyingExtended.UI
{
    public class UITimeRange : UIPanel
    {
        private static byte _padding = 10;

        Vector2 _sliderSize;
        private float _iconWidth;

        private UISlider CreateSlider(string statName, bool invert)
        {
            // Create the slider
            var slider = this.AddUIComponent<UISlider>();
            slider.fillMode = UIFillMode.Fill;
            //slider.backgroundSprite = "BudgetSlider";
            slider.color = new Color32(50, 0, 0, 255);
            slider.canFocus = true;
            slider.orientation = UIOrientation.Horizontal;
            slider.minValue = 0;
            slider.maxValue = 24f;
            slider.stepSize = 1f;
            slider.size = _sliderSize;
            slider.zOrder = 15;

            // Create the indicator
            var indicatorObject = new GameObject(statName + "Indicator");
            indicatorObject.transform.parent = slider.transform;
            var indicator = indicatorObject.AddComponent<UISprite>();
            indicator.spriteName = "SliderBudget";
            if (invert)
                indicator.flip = UISpriteFlip.FlipVertical;
            slider.thumbObject = indicator;
            //uiSlider.thumbOffset = new Vector2(0, 2);
            //slider.thumbObject.zOrder = 20;

            return slider;
        }
        
        public override void Start()
        {
            //init
            this.backgroundSprite = "SubcategoriesPanel";
            _iconWidth = this.height;
            _sliderSize = new Vector2(this.width - 50 - _padding * 3 - _iconWidth, 10);

            //slider icon
            var icon = this.AddUIComponent<UISprite>();
            icon.spriteName = "EmptyIcon";
            icon.size = new Vector2(_iconWidth, _iconWidth);
            icon.position = new Vector3(_padding, 0);

            //slider middle background
            var sliderLine = this.AddUIComponent<UIPanel>();
            sliderLine.backgroundSprite = "BudgetSlider";
            sliderLine.position = new Vector3(_padding * 2 + _iconWidth, -(this.height / 2) + 5);
            sliderLine.size = new Vector2(_sliderSize.x, 10);
            sliderLine.zOrder = 2;

            //up slider
            var uiSlider = CreateSlider("EmptyingTimeRaneUp", false);
            uiSlider.position = new Vector3(_padding * 2 + _iconWidth, -(this.height / 2) + uiSlider.size.y);

            //down slider
            var uiSlider2 = CreateSlider("EmptyingTimeRaneDown", true);
            uiSlider2.position = new Vector3(_padding * 2 + _iconWidth, -(this.height / 2));
            
            base.Start();
        }
    }
}
