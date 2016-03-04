using AutoEmptyingExtended.Data;
using AutoEmptyingExtended.UI.Panels;

namespace AutoEmptyingExtended.UI
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
