using System.Reflection;
using ColossalFramework;
using ColossalFramework.UI;
using UnityEngine;

namespace AutoEmptyingExtended.Panels
{
    public class EmptyingPanel : UIPanel
    {
        private int _selectedBuilding;

        public override void Start()
        {
            base.Start();

            this.backgroundSprite = "MenuPanel2";
            this.position = new Vector3(0, 12);
            this.width = this.parent.width;

            //temp
            this.height = 100;
        }

        public override void Update()
        {
            var instanceId = WorldInfoPanel.GetCurrentInstanceID();
            if (instanceId.Type == InstanceType.Building && instanceId.Building != 0)
            {
                var buildingId = instanceId.Building;
                if (_selectedBuilding != buildingId)
                {
                    _selectedBuilding = buildingId;

                    Debug.Log($"EmptyingPanel {buildingId}");
                    var buildingAi = Singleton<BuildingManager>.instance.m_buildings.m_buffer[buildingId].Info.m_buildingAI;
                    if (buildingAi is LandfillSiteAI || buildingAi is CemeteryAI)
                    {
                        this.isVisible = true;
                        //Do something (update emptying display data)
                    }
                    else
                    {
                        this.isVisible = false;
                    }

                }
            }

            base.Update();
        }

        //internal void OnVisibilityChanged(UIComponent component, bool show)
        //{
        //    if (show)
        //        _needUpdate = true;
        //}
        
    }
}
