using System;
using ColossalFramework.UI;
using UnityEngine;

namespace AutoEmptyingExtended.UI
{
    public class UIEmptyingControl : UICustomControl
    {
        private readonly UIView _uiView;
        private ConfigurationPanel _configurationPanel;
        private EmptyingTool _emptyingTool;
        private bool _uiShown;

        public UIEmptyingControl()
        {
            _uiView = UIView.GetAView();
            
            var lastTab = _uiView.FindUIComponent<UITabstrip>("MainToolstrip").tabs.Last();
            var btnToggle = (UIButton)_uiView.AddUIComponent(typeof(UIButton));
            btnToggle.absolutePosition = new Vector3(lastTab.absolutePosition.x + lastTab.width + 10, lastTab.absolutePosition.y);
            btnToggle.name = "AutoEmptyingButton";
            btnToggle.normalBgSprite = "EmptyIcon";
            btnToggle.focusedFgSprite = "EmptyIcon";
            btnToggle.hoveredFgSprite = "ToolbarIconGroup6Hovered";
            btnToggle.focusedBgSprite = "ToolbarIconGroup6Focused";
            btnToggle.size = new Vector2(43, 49);
            btnToggle.eventClick += (component, state) =>
            {
                if (!_uiShown)
                {
                    Show();
                }
                else {
                    Close();
                }
            };
        }

        public void Show()
        {
            try
            {
                ToolsModifierControl.mainToolbar.CloseEverything();
            }
            catch (Exception e)
            {
                Debug.Log("Error on Show(): " + e);
            }

            if (_configurationPanel == null)
            {
                _configurationPanel = (ConfigurationPanel)_uiView.AddUIComponent(typeof(ConfigurationPanel));
                _configurationPanel.eventCloseClick += uiComponent => Close();
            }

            if (_emptyingTool == null)
            {
                _emptyingTool = ToolsModifierControl.toolController.gameObject.GetComponent<EmptyingTool>() ??
                                    ToolsModifierControl.toolController.gameObject.AddComponent<EmptyingTool>();
            }

            _uiView.AddUIComponent(typeof (GarbageInfoViewPanel));

            ToolsModifierControl.toolController.CurrentTool = _emptyingTool;
            ToolsModifierControl.SetTool<EmptyingTool>();

            _uiShown = true;
        }

        public void Close()
        {
            if (_configurationPanel != null)
            {
                Destroy(_configurationPanel);
            }

            if (_emptyingTool != null)
            {
                ToolsModifierControl.toolController.CurrentTool = ToolsModifierControl.GetTool<DefaultTool>();
                ToolsModifierControl.SetTool<DefaultTool>();

                Destroy(_emptyingTool);
            }

            _uiShown = false;
        }

    }
}
