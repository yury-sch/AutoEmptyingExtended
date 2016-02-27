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

            this.backgroundSprite = "MenuPanel2";
            this.position = new Vector3(0, 12);
            this.width = 500;
            this.height = 200;
            this.isVisible = false;

            var uiTimeRange = this.AddUIComponent<UITimeRange>();
            uiTimeRange.position = new Vector3(15, -60);
            uiTimeRange.size = new Vector2(this.width - uiTimeRange.position.x * 2, 40);
        }
    }
}
