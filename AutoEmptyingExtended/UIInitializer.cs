using AutoEmptyingExtended.Panels;
using ColossalFramework.UI;
using ICities;
using UnityEngine;

namespace AutoEmptyingExtended
{
    public class UIInitializer : LoadingExtensionBase
    {
        private LoadMode _mode;
        private GameObject _bwGameObject;
        
        private void InitWindows()
        {
            //TODO We must to destroy all our own view components after level unloading. I used _bwGameObject container for this.
            var serviceBuildingInfo = UIView.Find<UIPanel>("(Library) CityServiceWorldInfoPanel");
            serviceBuildingInfo.AddUIComponent<BuildingInfoPanel>();


            //temp
            var emptyingConfigurationPanel = UIView.GetAView().AddUIComponent(typeof(ConfigurationPanel));
            var bulldozeButton = UIView.Find<UIMultiStateButton>("BulldozerButton");
            //var mainButton = _bwGameObject.AddComponent<UIButton>();
            var mainButton = bulldozeButton.parent.AddUIComponent<UIButton>();
            mainButton.size = bulldozeButton.size;
            mainButton.relativePosition = new Vector2
            (
                bulldozeButton.relativePosition.x + bulldozeButton.width / 2.0f - mainButton.width - bulldozeButton.width,
                bulldozeButton.relativePosition.y
            );
            mainButton.normalBgSprite = "EmptyIcon";
            mainButton.focusedFgSprite = "ToolbarIconGroup6Focused";
            mainButton.hoveredFgSprite = "ToolbarIconGroup6Hovered";
            mainButton.eventButtonStateChanged += (component, state) =>
            {
                if (state == UIButton.ButtonState.Pressed)
                    emptyingConfigurationPanel.isVisible = !emptyingConfigurationPanel.isVisible;
            };
            //mainButton.transform.parent = bulldozeButton.transform.parent;
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
