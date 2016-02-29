using ColossalFramework.UI;
using ICities;
using UnityEngine;

namespace AutoEmptyingExtended.UI
{
    public class UILoadingExtension : LoadingExtensionBase
    {
        private LoadMode _mode;
        private GameObject _bwGameObject;
        
        private void InitWindows()
        {
            //TODO We must to destroy all our own view components after level unloading. I used _bwGameObject container for this.
            var serviceBuildingInfo = UIView.Find<UIPanel>("(Library) CityServiceWorldInfoPanel");
            serviceBuildingInfo.AddUIComponent<BuildingInfoPanel>();
            
            ToolsModifierControl.toolController.gameObject.AddComponent<UIEmptyingControl>();
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
