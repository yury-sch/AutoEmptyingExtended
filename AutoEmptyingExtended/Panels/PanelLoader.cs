using ColossalFramework.UI;
using ICities;
using UnityEngine;

namespace AutoEmptyingExtended.Panels
{
    public class ModLoading : LoadingExtensionBase
    {
        private LoadMode _mode;
        private GameObject _bwGameObject;
        
        private void InitWindows()
        {
            var serviceBuildingInfo = UIView.Find<UIPanel>("(Library) CityServiceWorldInfoPanel");
            var emptyingPanel = _bwGameObject.AddComponent<EmptyingPanel>();
            emptyingPanel.transform.parent = serviceBuildingInfo.transform;
            //serviceBuildingInfo.eventVisibilityChanged += (component, show) => emptyingPanel.OnVisibilityChanged(component, show);
        }

        public override void OnLevelLoaded(LoadMode mode)
        {
            if (mode != LoadMode.LoadGame && mode != LoadMode.NewGame)
                return;
            _mode = mode;

            _bwGameObject = new GameObject("AUEBuildingWindowObject");
            InitWindows();
        }

        public override void OnLevelUnloading()
        {
            if (_mode != LoadMode.LoadGame && _mode != LoadMode.NewGame)
                return;
            
            if (_bwGameObject != null)
                Object.Destroy(_bwGameObject);
        }
    }
}
