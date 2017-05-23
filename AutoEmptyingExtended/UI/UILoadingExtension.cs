using System.Linq;
using AutoEmptyingExtended.UI.Panels;
using AutoEmptyingExtended.Utils;
using ColossalFramework.UI;
using ICities;

namespace AutoEmptyingExtended.UI
{
    public class UILoadingExtension : LoadingExtensionBase
    {
        private LoadMode _mode;
        private UIServiceInfoPanel _serviceInfoPanel;
        private UILandfillEmptyingPanel _landfillEmptyingPanel;
        private UICemetaryEmptyingPanel _cemetaryEmptyingPanel;

        #region Auxilary methods

        private static UIPanel FindUIPanel(string name)
        {
            var panelInfo = UIView.library.m_DynamicPanels.FirstOrDefault(e => e.name == name && e.instance is UIPanel);
            return panelInfo?.instance as UIPanel;
        }

        #endregion

        private void InitWindows()
        {
            // Add UIServiceInfoPanel to landfill site and cemetry panels
            var cityServiceWorldInfoPanel = FindUIPanel("CityServiceWorldInfoPanel");
            if (cityServiceWorldInfoPanel == null) throw new DetailedException($"Failed to find {nameof(cityServiceWorldInfoPanel)}");

            _serviceInfoPanel = cityServiceWorldInfoPanel.AddUIComponent<UIServiceInfoPanel>();

            // Add UILandfillEmptyingPanel to garbage configuration panel
            var garbageInfoViewPanel = FindUIPanel("GarbageInfoViewPanel");
            if (garbageInfoViewPanel == null) throw new DetailedException($"Failed to find {nameof(garbageInfoViewPanel)}");

            _landfillEmptyingPanel = garbageInfoViewPanel.AddUIComponent<UILandfillEmptyingPanel>();

            // Add UILandfillEmptyingPanel to health configuration panel
            var healthInfoViewPanel = FindUIPanel("HealthInfoViewPanel");
            if (healthInfoViewPanel == null) throw new DetailedException($"Failed to find {nameof(healthInfoViewPanel)}");

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
                UnityEngine.Object.Destroy(_serviceInfoPanel);
            if (_landfillEmptyingPanel != null)
                UnityEngine.Object.Destroy(_landfillEmptyingPanel);
            if (_cemetaryEmptyingPanel != null)
                UnityEngine.Object.Destroy(_cemetaryEmptyingPanel);
        }
    }
}
