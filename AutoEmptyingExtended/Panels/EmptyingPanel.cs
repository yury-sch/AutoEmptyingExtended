using System.Reflection;
using AutoEmptyingExtended.UI;
using ColossalFramework;
using ColossalFramework.UI;
using UnityEngine;

namespace AutoEmptyingExtended.Panels
{
    public class EmptyingPanel : UIPanel
    {
        private int _selectedBuilding;

        [Range(0.0f, 24f)]
        public float m_TimeOfDay;

        public override void Start()
        {
            base.Start();

            this.backgroundSprite = "MenuPanelInfo";
            this.position = new Vector3(0, 12);
            this.width = this.parent.width;
            //temp
            this.height = 100;

            var uiTimeRange = this.AddUIComponent<UITimeRange>();
            uiTimeRange.position = new Vector3(15, -15);
            uiTimeRange.size = new Vector2(this.width - uiTimeRange.position.x * 2, 40);
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
