using AutoEmptyingExtended.Data;
using ColossalFramework;
using ColossalFramework.UI;
using UnityEngine;

namespace AutoEmptyingExtended.UI.Panels
{
    public class UIServiceInfoPanel : UIPanel
    {
        private ushort _selectedBuilding;

        private UICheckboxContainer _autoEmptyingCheckbox;
        //private UICheckboxContainer _autoFillingCheckbox;

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
            _autoEmptyingCheckbox = AddUIComponent<UICheckboxContainer>();
            _autoEmptyingCheckbox.eventCheckChanged += (component, value) =>
            {
                if (_selectedBuilding == 0)
                    return;
                    BuildingDataManager.Data[_selectedBuilding].AutoEmptyingDisabled = value;
            };

            //_autoFillingCheckbox = AddUIComponent<UICheckboxContainer>();
            //_autoFillingCheckbox.Text = "CityServicePanel.AutoEmptying.TurnoffWhenDone".Translate();
            //_autoFillingCheckbox.eventCheckChanged += (component, value) =>
            //{
            //    if (_selectedBuilding == 0)
            //        return;
            //    BuildingDataManager.Data[_selectedBuilding].AutoFillingDisabled = value;
            //};
        }

        public override void Update()
        {
            base.Update();

            _autoEmptyingCheckbox.Text = "CityServicePanel.AutoEmptying.Disabled".Translate();

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

                        var serviceData = BuildingDataManager.Data[buildingId];
                        _autoEmptyingCheckbox.Checked = serviceData.AutoEmptyingDisabled;
                        //_autoFillingCheckbox.Checked = serviceData.AutoFillingDisabled;
                    }
                    else
                    {
                        this.isVisible = false;
                    }
                }
            }
        }
    }
}
