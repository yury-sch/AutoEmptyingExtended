using ColossalFramework;
using ColossalFramework.UI;
using UnityEngine;

namespace AutoEmptyingExtended.UI
{
    public class UIBuildingInfoPanel : UIPanel
    {
        private int _selectedBuilding;

        private UICheckboxContainer _checkbox1;
        private UICheckboxContainer _checkbox2;

        public override void Start()
        {
            base.Start();

            // configure panel
            autoLayout = true;
            autoLayoutDirection = LayoutDirection.Vertical;
            autoLayoutPadding = new RectOffset(0, 0, 1, 1);
            autoLayoutStart = LayoutStart.TopLeft;

            position = new Vector3(160, 120);
            width = 400;
            height = 200;

            // add sub-components
            _checkbox1 = AddUIComponent<UICheckboxContainer>();
            _checkbox1.Text = "CityServicePanel.AutoEmptying.Disabled".Translate();

            _checkbox2 = AddUIComponent<UICheckboxContainer>();
            _checkbox2.Text = "CityServicePanel.AutoEmptying.TurnoffWhenDone".Translate();
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

                    var buildingAi = Singleton<BuildingManager>.instance.m_buildings.m_buffer[buildingId].Info.m_buildingAI;
                    if (buildingAi.CanBeEmptied() && (buildingAi is LandfillSiteAI || buildingAi is CemeteryAI))
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
