using ColossalFramework.UI;
using UnityEngine;

namespace AutoEmptyingExtended.UI
{
    public class UISliderContainer : UIPanel
    {
        #region Const

        private static byte _padding = 10;
        private static float _labelWidth = 50;

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
        private UISlider _slider;
        private UILabel _labelValue;

        #endregion

        #region Ctor

        public UISliderContainer()
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
                if (_slider != null)
                {
                    _slider.minValue = value;
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
                if (_slider != null)
                {
                    _slider.maxValue = value;
                }
            }
        }

        public float StepSize
        {
            get { return _stepSize; }
            set
            {
                _stepSize = value;
                if (_slider != null)
                {
                    _slider.stepSize = _stepSize;
                }
            }
        }

        public float Value
        {
            get { return _slider?.value ?? (_maxValue + _minValue) / 2; }
            set
            {
                if (_slider == null || value < _minValue || value > _maxValue)
                    return;
                _slider.value = value;
                _labelValue.text = value.ToString(_valueFormat);
            }
        }

        public string ValueFormat
        {
            get { return _valueFormat; }
            set
            {
                _valueFormat = value;

                //update labels
                if (_slider != null)
                {
                    Value = _slider.value;
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
            slider.backgroundSprite = "BudgetSlider";
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

            //value
            _labelValue = CreateLabel();
            _labelValue.position = new Vector3(_padding * 3 + _iconWidth + _sliderSize.x, -(this.height / 2) + (_labelValue.size.y / 2));
            
            //slider
            _slider = CreateSlider(false);
            _slider.position = new Vector3(_padding * 2 + _iconWidth, -(this.height / 2) + (_slider.size.y / 2));
            _slider.eventValueChanged += (component, value) => { Value = value; };

            //values
            _slider.value = _maxValue;

            base.Start();
        }

        #endregion
    }
}
