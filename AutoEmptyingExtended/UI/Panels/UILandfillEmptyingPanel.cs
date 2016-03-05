using AutoEmptyingExtended.Data;

namespace AutoEmptyingExtended.UI.Panels
{
    public class UILandfillEmptyingPanel : UIConfigurationPanel
    {
        public override void Start()
        {
            this.Data = ConfigurationDataManager.Data.Landfill;

            base.Start();
        }
    }
}
