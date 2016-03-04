using AutoEmptyingExtended.Data;

namespace AutoEmptyingExtended.UI
{
    public class UILandfillEmptyingPanel : UIEmptyingPanel
    {
        public override void Start()
        {
            this.Data = ConfigurationDataManager.Data.Landfill;

            base.Start();
        }
    }
}
