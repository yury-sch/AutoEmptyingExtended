using AutoEmptyingExtended.Utils;
using ColossalFramework.UI;
using UnityEngine;

namespace AutoEmptyingExtended.UI.Panels
{
    public class UIRangePickerPanel : UIPanel
    {
        #region Const

        private const byte PanelPadding = 10;
        private const byte LabelWidth = 50;

        #endregion

        #region Fields

        private readonly float _iconWidth;

        private float _minValue, _maxValue;
        private float _startValue, _endValue;
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
        private UIPanel _sliderLine;

        #endregion

        #region Ctor

        public UIRangePickerPanel()
        {
            //init
            height = 40;
            backgroundSprite = "SubcategoriesPanel";

            _iconWidth = height;
            _minValue = 0;
            _maxValue = 100f;
            _stepSize = 1f;
            _valueFormat = "F";
        }

        #endregion

        #region Events

        public event PropertyChangedEventHandler<float> EventStartValueChanged;

        public event PropertyChangedEventHandler<float> EventEndValueChanged;

        #endregion

        #region Properties

        public float MinValue
        {
            get => _minValue;
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
            get => _maxValue;
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
            get => _stepSize;
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
            get => _startValue;
            set
            {
                if (value < _minValue || value > _maxValue)
                    throw new DetailedException($"{nameof(UIRangePickerPanel)}: {nameof(StartValue)} is out of the range");

                _startValue = value;
                UpdateUIStartValue();
            }
        }
        public float EndValue
        {
            get => _endValue;
            set
            {
                if (value < _minValue || value > _maxValue)
                    throw new DetailedException($"{nameof(UIRangePickerPanel)}: {nameof(StartValue)} < {nameof(MinValue)}");

                _endValue = value;
                UpdateUIEndValue();
            }
        }

        public string ValueFormat
        {
            get => _valueFormat;
            set
            {
                _valueFormat = value;

                UpdateUIStartValue();
                UpdateUIEndValue();
            }
        }


        public UITextureAtlas IconAtlas
        {
            set => _iconAtlas = value;
        }
        public string IconSprite
        {
            set => _iconSprite = value;
        }

        #endregion

        #region Utilities

        private void UpdateUIStartValue()
        {
            if (_sliderStart == null || _labelStart == null) return;

            _sliderStart.value = _startValue;
            _labelStart.text = _startValue.ToString(_valueFormat);
        }
        private void UpdateUIEndValue()
        {
            if (_sliderEnd == null || _labelEnd == null) return;

            _sliderEnd.value = _endValue;
            _labelEnd.text = _endValue.ToString(_valueFormat);
        }

        private UISlider AddSlider(bool invert)
        {
            // Create the slider
            var slider = AddUIComponent<UISlider>();
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

            // add slider middle background
            _sliderLine = AddUIComponent<UIPanel>();
            _sliderLine.backgroundSprite = "BudgetSlider";
            _sliderLine.zOrder = 2;

            // add sub-panels
            _labelStart = AddLabel();           // up value
            _sliderStart = AddSlider(false);    // up slider
            _labelEnd = AddLabel();             // down value
            _sliderEnd = AddSlider(true);       // down slider

            // init events
            _sliderStart.eventValueChanged += (component, value) =>
            {
                // nothing has changed -> return
                if (_startValue == value) return;

                // deny setting the value higher than _sliderEnd value
                if (value > _sliderEnd.value - 1)
                {
                    _sliderStart.value = _sliderEnd.value - 1;
                    return;
                }

                // update & save
                _startValue = value;
                UpdateUIStartValue();

                EventStartValueChanged?.Invoke(this, value);
            };
            _sliderEnd.eventValueChanged += (component, value) =>
            {
                // nothing has changed -> return
                if (_endValue == value) return;

                // deny setting the value lower than _sliderStart value
                if (value < _sliderStart.value + 1)
                {
                    _sliderEnd.value = _sliderStart.value + 1;
                    return;
                }
                
                // update & save
                _endValue = value;
                UpdateUIEndValue();

                EventEndValueChanged?.Invoke(this, value);
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
            _icon.position = new Vector3(PanelPadding, 0);


            // adjust UI elements size
            _sliderSize = new Vector2(width - LabelWidth - PanelPadding * 3 - _iconWidth, 10);

            _sliderLine.size = new Vector2(_sliderSize.x, 10);
            _sliderLine.position = new Vector3(PanelPadding * 2 + _iconWidth, -(height / 2) + 5);

            _labelStart.size = new Vector2(LabelWidth, 15);
            _labelStart.position = new Vector3(PanelPadding * 3 + _iconWidth + _sliderSize.x, -(height / 2) + _labelStart.size.y + 2);

            _labelEnd.size = new Vector2(LabelWidth, 15);
            _labelEnd.position = new Vector3(PanelPadding * 3 + _iconWidth + _sliderSize.x, -(height / 2) - 2);

            _sliderStart.size = _sliderSize;
            _sliderStart.position = new Vector3(PanelPadding * 2 + _iconWidth, -(height / 2) + _sliderStart.size.y);

            _sliderEnd.size = _sliderSize;
            _sliderEnd.position = new Vector3(PanelPadding * 2 + _iconWidth, -(height / 2));
        }

        #endregion
    }
}
