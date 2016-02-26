using ColossalFramework;
using ColossalFramework.UI;
using UnityEngine;

namespace AutoEmptyingExtended.Panels
{
    public class EmptyingPanel : UIPanel
    {
        internal CityServiceWorldInfoPanel ServicePanel { private get; set; }
        
        public override void Start()
        {
            base.Start();

            this.backgroundSprite = "MenuPanel2";
            this.position = new Vector3(0, 12);
            this.width = this.parent.width;

            //temp
            this.height = 100;
        }

        internal void OnVisibilityChanged(UIComponent component, bool show)
        {
            //this.isVisible = false;
            if (show)
            {
                var buildingId = WorldInfoPanel.GetCurrentInstanceID().Building;

                //At first click buildingId == 0. Why?
                Debug.Log($"EmptyingPanel {buildingId}");
                var buildingAi = Singleton<BuildingManager>.instance.m_buildings.m_buffer[buildingId].Info.m_buildingAI;
                if (buildingAi is LandfillSiteAI || buildingAi is CemeteryAI)
                {
                    //this.isVisible = true;
                }
            }
        }
    }
}
