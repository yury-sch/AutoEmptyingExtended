using AutoEmptyingExtended.Data;
using AutoEmptyingExtended.UI.Panels;

namespace AutoEmptyingExtended.UI
{
    public class UICemetaryEmptyingPanel : UIConfigurationPanel
    {
        public override void Start()
        {
            this.Data = ConfigurationDataManager.Data.Cemetary;

            base.Start();
        }
    }
}
