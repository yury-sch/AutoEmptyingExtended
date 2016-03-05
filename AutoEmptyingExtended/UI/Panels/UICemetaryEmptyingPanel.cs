using AutoEmptyingExtended.Data;

namespace AutoEmptyingExtended.UI.Panels
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
