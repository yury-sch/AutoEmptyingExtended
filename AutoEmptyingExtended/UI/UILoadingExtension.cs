using AutoEmptyingExtended.UI.Panels;
using ColossalFramework.UI;
using ICities;
using UnityEngine;

namespace AutoEmptyingExtended.UI
{
    public class UILoadingExtension : LoadingExtensionBase
    {
        private LoadMode _mode;
        private UIServiceInfoPanel _serviceInfoPanel;
        private UILandfillEmptyingPanel _landfillEmptyingPanel;
        private UICemetaryEmptyingPanel _cemetaryEmptyingPanel;

        private void InitWindows()
        {
            var serviceBuildingInfo = UIView.Find<UIPanel>("(Library) CityServiceWorldInfoPanel");
            _serviceInfoPanel = serviceBuildingInfo.AddUIComponent<UIServiceInfoPanel>();

            var garbageInfoViewPanel = UIView.Find<UIPanel>("(Library) GarbageInfoViewPanel");
            _landfillEmptyingPanel = garbageInfoViewPanel.AddUIComponent<UILandfillEmptyingPanel>();

            var healthInfoViewPanel = UIView.Find<UIPanel>("(Library) HealthInfoViewPanel");
            _cemetaryEmptyingPanel = healthInfoViewPanel.AddUIComponent<UICemetaryEmptyingPanel>();
        }

        public override void OnLevelLoaded(LoadMode mode)
        {
            if (mode != LoadMode.LoadGame && mode != LoadMode.NewGame)
                return;
            _mode = mode;

            InitWindows();
        }

        public override void OnLevelUnloading()
        {
            if (_mode != LoadMode.LoadGame && _mode != LoadMode.NewGame)
                return;
            
            if (_serviceInfoPanel != null)
                Object.Destroy(_serviceInfoPanel);
            if (_landfillEmptyingPanel != null)
                Object.Destroy(_landfillEmptyingPanel);
            if (_cemetaryEmptyingPanel != null)
                Object.Destroy(_cemetaryEmptyingPanel);
        }
    }
}
