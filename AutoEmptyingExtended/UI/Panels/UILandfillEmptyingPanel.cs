using AutoEmptyingExtended.Data;

namespace AutoEmptyingExtended.UI.Panels
{
    public class UILandfillEmptyingPanel : UIConfigurationPanel
    {
        public override void Awake()
        {
            Data = ConfigurationDataManager.Data.Landfill;

            base.Awake();
        }
    }
}
