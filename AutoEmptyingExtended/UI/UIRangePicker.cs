using System;
using ColossalFramework.UI;
using UnityEngine;

namespace AutoEmptyingExtended.UI
{
    public class UIRangePicker : UIPanel
    {
        #region Const

        private static byte _padding = 10;
        private static byte _labelWidth = 50;

        #endregion

        #region Fields
        
        private readonly float _iconWidth;

        private float _minValue;
        private float _maxValue;
        private float _stepSize;
        private string _valueFormat;

        private Vector2 _sliderSize;
        private UITextureAtlas _iconAtlas;
        private string _iconSprite;

        private UISprite _icon;
        private UISlider _sliderStart;
        private UISlider _sliderEnd;
        private UILabel _labelStart;
        private UILabel _labelEnd;

        #endregion

        #region Ctor

        public UIRangePicker()
        {
            //init
            this.height = 40;
            this.backgroundSprite = "SubcategoriesPanel";
            _iconWidth = this.height;
            _minValue = 0;
            _maxValue = 100f;
            _stepSize = 1f;
            _valueFormat = "F";
        }

        #endregion

        #region Properties

        public float MinValue
        {
            get
            {
                return _minValue;
            }
            set
            {
                _minValue = value;
                if (_sliderStart != null && _sliderEnd != null)
                {
                    _sliderStart.minValue = value;
                    _sliderEnd.minValue = value;
                }
            }
        }

        public float MaxValue
        {
            get
            {
                return _maxValue;
            }
            set
            {
                _maxValue = value;
                if (_sliderStart != null && _sliderEnd != null)
                {
                    _sliderStart.maxValue = value;
                    _sliderEnd.maxValue = value;
                }
            }
        }

        public float StepSize
        {
            get { return _stepSize; }
            set
            {
                _stepSize = value;
                if (_sliderStart != null && _sliderEnd != null)
                {
                    _sliderStart.stepSize = _stepSize;
                    _sliderEnd.stepSize = _stepSize;
                }
            }
        }

        public float StartValue
        {
            get { return _sliderStart?.value ?? _minValue; }
            set
            {
                if (_sliderStart != null && _sliderEnd != null && value >= _minValue)
                {
                    var calcValue = Math.Min(value, _sliderEnd.value - 1);
                    _sliderStart.value = calcValue;
                    _labelStart.text = calcValue.ToString(_valueFormat);
                }
            }
        }

        public float EndValue
        {
            get { return _sliderEnd?.value ?? _maxValue; }
            set
            {
                if (_sliderStart != null && _sliderEnd != null && value <= _maxValue)
                {
                    var calcValue = Math.Max(value, _sliderStart.value + 1);
                    _sliderEnd.value = calcValue;
                    _labelEnd.text = calcValue.ToString(_valueFormat);
                }
            }
        }

        public string ValueFormat
        {
            get { return _valueFormat; }
            set
            {
                _valueFormat = value;

                //update labels
                if (_sliderStart != null && _sliderEnd != null)
                {
                    StartValue = _sliderStart.value;
                    EndValue = _sliderEnd.value;
                }
            }
        }

        public UITextureAtlas IconAtlas
        {
            set { _iconAtlas = value; }
        }

        public string IconSprite
        {
            set { _iconSprite = value; }
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
            _sliderSize = new Vector2(this.width - _labelWidth - _padding * 3 - _iconWidth, 10);

            //slider icon
            _icon = this.AddUIComponent<UISprite>();
            if (_iconAtlas != null)
                _icon.atlas = _iconAtlas;
            if (_iconSprite != null)
                _icon.spriteName = _iconSprite;
            _icon.size = new Vector2(_iconWidth, _iconWidth);
            _icon.position = new Vector3(_padding, 0);

            //slider middle background
            var sliderLine = this.AddUIComponent<UIPanel>();
            sliderLine.backgroundSprite = "BudgetSlider";
            sliderLine.position = new Vector3(_padding * 2 + _iconWidth, -(this.height / 2) + 5);
            sliderLine.size = new Vector2(_sliderSize.x, 10);
            sliderLine.zOrder = 2;
            
            //up value
            _labelStart = CreateLabel();
            _labelStart.position = new Vector3(_padding * 3 + _iconWidth + _sliderSize.x, -(this.height / 2) + _labelStart.size.y + 2);

            //down value
            _labelEnd = CreateLabel();
            _labelEnd.position = new Vector3(_padding * 3 + _iconWidth + _sliderSize.x, -(this.height / 2) - 2);

            //up slider
            _sliderStart = CreateSlider(false);
            _sliderStart.position = new Vector3(_padding * 2 + _iconWidth, -(this.height / 2) + _sliderStart.size.y);
            _sliderStart.eventValueChanged += (component, value) => { StartValue = value; };

            //down slider
            _sliderEnd = CreateSlider(true);
            _sliderEnd.position = new Vector3(_padding * 2 + _iconWidth, -(this.height / 2));
            _sliderEnd.eventValueChanged += (component, value) => { EndValue = value; };

            //values
            _sliderStart.value = _minValue;
            _sliderEnd.value = _maxValue;

            base.Start();
        }

        #endregion
    }
}
