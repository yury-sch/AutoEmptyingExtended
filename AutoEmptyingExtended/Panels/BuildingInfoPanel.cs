using ColossalFramework;
using ColossalFramework.UI;
using UnityEngine;

namespace AutoEmptyingExtended.Panels
{
    public class BuildingInfoPanel : UIPanel
    {
        #region Components 

        private CheckboxSubPanel _checkbox1;
        private CheckboxSubPanel _checkbox2;

        #endregion

        private int _selectedBuilding;

        public override void Start()
        {
            base.Start();

            // configure panel
            autoLayout = true;
            autoLayoutDirection = LayoutDirection.Vertical;
            autoLayoutPadding = new RectOffset(0, 0, 1, 1);
            autoLayoutStart = LayoutStart.TopLeft;

            position = new Vector3(160, 110);
            width = 400;
            height = 200;

            // add sub-components
            _checkbox1 = AddUIComponent<CheckboxSubPanel>();
            //_checkbox1.Text = "Disable auto emptying for this site";

            _checkbox2 = AddUIComponent<CheckboxSubPanel>();
            //_checkbox2.Text = "Disable the site after emptying";
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

                    Debug.Log($"MyPanel {buildingId}");
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
    }
}
