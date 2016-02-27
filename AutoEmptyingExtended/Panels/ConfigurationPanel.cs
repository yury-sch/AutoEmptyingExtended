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

            this.backgroundSprite = "MenuPanel2";
            this.size = new Vector2(350, 300);
            this.absolutePosition = new Vector3(GetUIView().fixedWidth - 70 - this.width, GetUIView().fixedHeight - 130 - this.height);

            var uiTimeRange = this.AddUIComponent<UITimeRange>();
            uiTimeRange.position = new Vector3(15, -60);
            uiTimeRange.size = new Vector2(this.width - uiTimeRange.position.x * 2, 40);

            var uiTimeRange2 = this.AddUIComponent<UITimeRange>();
            uiTimeRange2.position = new Vector3(15, uiTimeRange.position.y - uiTimeRange.size.y - 20);
            uiTimeRange2.size = new Vector2(this.width - uiTimeRange2.position.x * 2, 40);
        }
    }
}
