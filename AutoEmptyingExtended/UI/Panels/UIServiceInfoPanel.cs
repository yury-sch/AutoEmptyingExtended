using AutoEmptyingExtended.Data;
using AutoEmptyingExtended.UI.Localization;
using ColossalFramework;
using ColossalFramework.UI;
using UnityEngine;

namespace AutoEmptyingExtended.UI.Panels
{
    /// <summary>
    /// Panel appearing while a particular cemetry/landfill is selected
    /// </summary>
    public class UIServiceInfoPanel : UIPanel
    {
        private ushort _selectedBuilding;

        private UICheckboxContainerPanel _autoEmptyingCheckbox;
        //private UICheckboxContainerPanel _autoFillingCheckbox;

        private void SetLocales()
        {
            _autoEmptyingCheckbox.Text = "CityServicePanel.AutoEmptying.Disabled".Translate();
        }

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
            _autoEmptyingCheckbox = AddUIComponent<UICheckboxContainerPanel>();
            _autoEmptyingCheckbox.EventCheckChanged += (component, value) =>
            {
                if (_selectedBuilding == 0) return;
                BuildingDataManager.Data[_selectedBuilding].AutoEmptyingDisabled = value;
            };

            //_autoFillingCheckbox = AddUIComponent<UICheckboxContainerPanel>();
            //_autoFillingCheckbox.Text = "CityServicePanel.AutoEmptying.TurnoffWhenDone".Translate();
            //_autoFillingCheckbox.eventCheckChanged += (component, value) =>
            //{
            //    if (_selectedBuilding == 0)
            //        return;
            //    BuildingDataManager.Data[_selectedBuilding].AutoFillingDisabled = value;
            //};

            SetLocales();
            LocalizationManager.Instance.EventLocaleChanged += language => SetLocales();
        }

        public override void Update()
        {
            base.Update();
            
            var instanceId = WorldInfoPanel.GetCurrentInstanceID();
            if (instanceId.Type != InstanceType.Building || instanceId.Building == 0) return;

            var buildingId = instanceId.Building;
            if (_selectedBuilding == buildingId) return;

            _selectedBuilding = buildingId;

            var buildingAi = Singleton<BuildingManager>.instance.m_buildings.m_buffer[buildingId].Info.m_buildingAI;
            if (buildingAi.CanBeEmptied() && (buildingAi is LandfillSiteAI || buildingAi is CemeteryAI))
            {
                isVisible = true;

                var serviceData = BuildingDataManager.Data[buildingId];
                _autoEmptyingCheckbox.Checked = serviceData.AutoEmptyingDisabled;
                //_autoFillingCheckbox.Checked = serviceData.AutoFillingDisabled;
            }
            else
            {
                isVisible = false;
            }
        }
    }
}
