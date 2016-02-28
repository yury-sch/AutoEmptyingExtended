using AutoEmptyingExtended.UI;
using ColossalFramework.UI;
using UnityEngine;

namespace AutoEmptyingExtended.Panels
{
    public class ConfigurationPanel : UIPanel
    {
        public override void Start()
        {
            base.Start();

            this.isVisible = false;

            this.backgroundSprite = "MenuPanel";
            this.size = new Vector2(350, 300);
            this.absolutePosition = new Vector3(GetUIView().fixedWidth - 70 - this.width, GetUIView().fixedHeight - 130 - this.height);

            // icon
            var icon = this.AddUIComponent<UISprite>();
            icon.spriteName = "EmptyIcon";
            icon.size = new Vector2(65, 65);
            icon.position = new Vector3(0, icon.size.y - 41);

            //label
            var label = this.AddUIComponent<UILabel>();
            label.text = "AutoEmptying: E";
            label.height = 17;
            label.position = new Vector3(this.width / 2 - label.width /2, -(float)41 / 2  + label.height / 2 );

            //close cross
            var closeButton = this.AddUIComponent<UIButton>();
            closeButton.size = new Vector2(33, 33);
            closeButton.position = new Vector2(this.width - 4 - closeButton.width, -4);
            closeButton.normalBgSprite = "buttonclose";
            closeButton.hoveredFgSprite = "buttonclosehover";
            closeButton.pressedBgSprite = "buttonclosepressed";
            closeButton.eventButtonStateChanged += (component, state) => 
            {
                if (state == UIButton.ButtonState.Pressed)
                    this.isVisible = false;
            };

            var uiTimeRangeLandfillSite = this.AddUIComponent<UITimeRange>();
            uiTimeRangeLandfillSite.position = new Vector3(15, -60);
            uiTimeRangeLandfillSite.size = new Vector2(this.width - uiTimeRangeLandfillSite.position.x * 2, 40);

            var uiTimeRangeCemetery = this.AddUIComponent<UITimeRange>();
            uiTimeRangeCemetery.position = new Vector3(15, uiTimeRangeLandfillSite.position.y - uiTimeRangeLandfillSite.size.y - 20);
            uiTimeRangeCemetery.size = new Vector2(this.width - uiTimeRangeCemetery.position.x * 2, 40);


        }
    }
}
