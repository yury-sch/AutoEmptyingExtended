using ColossalFramework.UI;
using ICities;
using UnityEngine;

namespace AutoEmptyingExtended.Panels
{
    public class ModLoading : LoadingExtensionBase
    {
        private LoadMode _mode;
        private GameObject _bwGameObject;
        private EmptyingPanel _emptyingPanel;

        public override void OnLevelLoaded(LoadMode mode)
        {
            if (mode != LoadMode.LoadGame && mode != LoadMode.NewGame)
                return;
            _mode = mode;

            _bwGameObject = new GameObject("buildingWindowObject");
            var serviceBuildingInfo = UIView.Find<UIPanel>("(Library) CityServiceWorldInfoPanel");

            _emptyingPanel = _bwGameObject.AddComponent<EmptyingPanel>();
            _emptyingPanel.transform.parent = serviceBuildingInfo.transform;
            _emptyingPanel.ServicePanel = serviceBuildingInfo.gameObject.transform.GetComponentInChildren<CityServiceWorldInfoPanel>();
            serviceBuildingInfo.eventVisibilityChanged += (component, show) => _emptyingPanel.OnVisibilityChanged(component, show);
        }


        public override void OnLevelUnloading()
        {
            if (_mode != LoadMode.LoadGame && _mode != LoadMode.NewGame)
                return;
            
            if (_bwGameObject != null)
            {
                UnityEngine.Object.Destroy(_bwGameObject);
            }
        }
    }
}
