using ColossalFramework.UI;
using UnityEngine;

namespace AutoEmptyingExtended.UI.Panels
{
    public class UICheckboxContainerPanel : UIPanel
    {
        private string _text = "<default_text>";
        private UIButton _checkbox;
        private UILabel _description;

        public event PropertyChangedEventHandler<bool> EventCheckChanged;

        public UICheckboxContainerPanel()
        {
            width = 400;
            height = 16;
        }

        public bool Checked
        {
            get
            {
                if (_checkbox != null)
                    return _checkbox.normalFgSprite == "check-checked";

                return false;
            }

            set
            {
                if (_checkbox == null)
                    return;

                if (value == Checked)
                    return;

                _checkbox.normalFgSprite = value ? "check-checked" : "check-unchecked";

                EventCheckChanged?.Invoke(this, value);
            }
        }

        public string Text
        {
            set
            {
                _text = value;

                if (_description != null)
                    _description.text = value;
            }
        }

        public override void Awake()
        {
            base.Awake();

            // configure panel
            isVisible = true;
            isEnabled = true;
            canFocus = true;
            isInteractive = true;
            relativePosition = Vector3.zero;

            // add sub-components
            const int inset = 5;

            _checkbox = AddUIComponent<UIButton>();
            _checkbox.normalFgSprite = "check-unchecked";
            _checkbox.relativePosition = new Vector3(inset, 0);
            _checkbox.eventClick += (component, param) => { Checked = !Checked; };

            _description = AddUIComponent<UILabel>();
            _description.text = _text;
            _description.autoHeight = true;
            _description.autoSize = true;
            _description.relativePosition = new Vector3(_checkbox.relativePosition.x + _checkbox.width + inset, 0);
        }
    }
}
