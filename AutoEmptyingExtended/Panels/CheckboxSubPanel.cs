﻿using ColossalFramework.UI;
using UnityEngine;

namespace AutoEmptyingExtended.Panels
{
    public class CheckboxSubPanel : UIPanel
    {
        #region Components 

        private UIButton _checkbox;
        private UILabel _description;

        #endregion

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
            }
        }

        public string Text
        {
            set { _description.text = value; }
        }

        public override void Awake()
        {
            base.Awake();

            // add sub-components
            //_checkbox = AddUIComponent<UIButton>();
            //_description = AddUIComponent<UILabel>();
            
            //_checkbox.normalFgSprite = "check-unchecked";
        }

        public override void Start()
        {
            base.Start();

            // configure panel
            height = 20;
            width = 400;
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
            _description.text = "<default_text>";
            _description.autoHeight = true;
            _description.autoSize = true;
            _description.relativePosition = new Vector3(_checkbox.relativePosition.x + _checkbox.width + inset, 0);

        }
    }
}
