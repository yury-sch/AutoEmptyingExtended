using System;
using ColossalFramework.UI;
using UnityEngine;

namespace AutoEmptyingExtended.UI
{
    public class UITimeRange : UIPanel
    {
        #region Const

        private static byte _padding = 10;
        private static byte _labelWidth = 50;
        private static float _minValue = 0;
        private static float _maxValue = 24f;
        private static float _stepSize = 1f;

        #endregion

        #region Fields

        private float _iconWidth;
        private Vector2 _sliderSize;

        private UISlider _sliderStart;
        private UISlider _sliderEnd;

        #endregion

        #region Properties

        public float StartValue
        {
            get { return _sliderStart?.value ?? _minValue; }
            set
            {
                if (_sliderStart != null && _sliderEnd != null && value > _minValue)
                {
                    _sliderStart.value = Math.Min(value, _sliderEnd.value - 1);
                }
            }
        }

        public float EndValue
        {
            get { return _sliderEnd?.value ?? _maxValue; }
            set
            {
                if (_sliderStart != null && _sliderEnd != null && value < _maxValue)
                {
                    _sliderEnd.value = Math.Max(value, _sliderStart.value + 1);
                }
            }
        }

        #endregion

        #region Utilities

        private UISlider CreateSlider(bool invert)
        {
            // Create the slider
            var slider = this.AddUIComponent<UISlider>();
            slider.fillMode = UIFillMode.Fill;
            slider.orientation = UIOrientation.Horizontal;
            slider.minValue = _minValue;
            slider.maxValue = _maxValue;
            slider.stepSize = _stepSize;
            slider.size = _sliderSize;
            slider.zOrder = 15;

            // Create the indicator
            var indicatorObject = new GameObject();
            indicatorObject.transform.parent = slider.transform;
            var indicator = indicatorObject.AddComponent<UISprite>();
            indicator.spriteName = "SliderBudget";
            if (invert)
                indicator.flip = UISpriteFlip.FlipVertical;
            slider.thumbObject = indicator;

            return slider;
        }

        private UILabel CreateLabel()
        {
            var label = this.AddUIComponent<UILabel>();
            label.textColor = new Color32(206, 248, 0, 255);
            label.size = new Vector2(_labelWidth, 15);

            return label;
        }

        #endregion

        #region Methods

        public override void Start()
        {
            //init
            this.backgroundSprite = "SubcategoriesPanel";
            _iconWidth = this.height;
            _sliderSize = new Vector2(this.width - _labelWidth - _padding * 3 - _iconWidth, 10);

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
            
            //up value
            var labelStart = CreateLabel();
            labelStart.position = new Vector3(_padding * 3 + _iconWidth + _sliderSize.x, -(this.height / 2) + labelStart.size.y + 2);

            //down value
            var labelEnd = CreateLabel();
            labelEnd.position = new Vector3(_padding * 3 + _iconWidth + _sliderSize.x, -(this.height / 2) - 2);

            //up slider
            _sliderStart = CreateSlider(false);
            _sliderStart.position = new Vector3(_padding * 2 + _iconWidth, -(this.height / 2) + _sliderStart.size.y);
            _sliderStart.eventValueChanged += (component, value) =>
            {
                if (value >= _sliderEnd.value)
                    _sliderStart.value = _sliderEnd.value - 1;
                else
                    labelStart.text = value.ToString("0#.00");
            };

            //down slider
            _sliderEnd = CreateSlider(true);
            _sliderEnd.position = new Vector3(_padding * 2 + _iconWidth, -(this.height / 2));
            _sliderEnd.eventValueChanged += (component, value) => 
            {
                if (value <= _sliderStart.value)
                    _sliderEnd.value = _sliderStart.value + 1;
                else
                    labelEnd.text = value.ToString("0#.00");
            };

            //values
            _sliderStart.value = _minValue;
            _sliderEnd.value = _maxValue;

            base.Start();
        }

        #endregion
    }
}
