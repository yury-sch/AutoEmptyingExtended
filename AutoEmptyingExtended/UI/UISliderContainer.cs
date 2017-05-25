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
        private float _value;
        private float _stepSize;
        private string _valueFormat;

        private Vector2 _sliderSize;
        private UITextureAtlas _iconAtlas;
        private string _iconSprite;

        private UISprite _icon;
        private UISlider _slider;
        private UILabel _labelValue;
        private UIPanel _sliderLine;
        #endregion

        #region Ctor

        public UISliderContainer()
        {
            //init
            height = 40;
            backgroundSprite = "SubcategoriesPanel";

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
            get { return _value; }
            set
            {
                if (_slider != null && value >= _minValue && value <= _maxValue)
                {
                    _slider.value = value;
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

        #region Events

        public event PropertyChangedEventHandler<float> eventValueChanged;

        #endregion

        #region Utilities

        private UISlider AddSlider(bool invert)
        {
            // Create the slider
            var slider = AddUIComponent<UISlider>();
            slider.backgroundSprite = "BudgetSlider";
            slider.fillMode = UIFillMode.Fill;
            slider.orientation = UIOrientation.Horizontal;
            slider.minValue = _minValue;
            slider.maxValue = _maxValue;
            slider.stepSize = _stepSize;
            slider.zOrder = 15;

            // Create the indicator
            var indicatorObject = new GameObject();
            indicatorObject.transform.parent = slider.transform;

            var indicator = indicatorObject.AddComponent<UISprite>();
            indicator.spriteName = "SliderBudget";
            if (invert) indicator.flip = UISpriteFlip.FlipVertical;
            slider.thumbObject = indicator;

            return slider;
        }

        private UILabel AddLabel()
        {
            var label = AddUIComponent<UILabel>();
            label.textColor = new Color32(206, 248, 0, 255);

            return label;
        }

        #endregion

        #region Methods

        public override void Awake()
        {
            base.Awake();

            // add sub-components
            // --- label
            _labelValue = AddLabel();

            // --- slider middle background
            _sliderLine = AddUIComponent<UIPanel>();
            _sliderLine.backgroundSprite = "BudgetSlider";
            _sliderLine.zOrder = 2;

            // --- slider
            _slider = AddSlider(false);
            _slider.value = _maxValue;
            _slider.eventValueChanged += (component, value) =>
            {
                if (_value != value)
                {
                    _value = value;
                    _slider.value = value;
                    _labelValue.text = value.ToString(_valueFormat);

                    eventValueChanged?.Invoke(this, value);
                }
            };
        }

        public override void Start()
        {
            base.Start();

            //add icon
            _icon = AddUIComponent<UISprite>();
            if (_iconAtlas != null) _icon.atlas = _iconAtlas;
            if (_iconSprite != null) _icon.spriteName = _iconSprite;
            _icon.size = new Vector2(_iconWidth, _iconWidth);
            _icon.position = new Vector3(_padding, 0);


            // adjust UI elements size
            _sliderSize = new Vector2(width - _labelWidth - _padding * 3 - _iconWidth, 10);

            _sliderLine.size = new Vector2(_sliderSize.x, 10);
            _sliderLine.position = new Vector3(_padding * 2 + _iconWidth, -(this.height / 2) + 5);

            _labelValue.size = new Vector2(_labelWidth, 15);
            _labelValue.position = new Vector3(_padding * 3 + _iconWidth + _sliderSize.x, -(this.height / 2) + (_labelValue.size.y / 2));
            
            _slider.size = _sliderSize;
            _slider.position = new Vector3(_padding * 2 + _iconWidth, -(this.height / 2) + (_slider.size.y / 2));
        }

        #endregion
    }
}
