using AutoEmptyingExtended.Data;

namespace AutoEmptyingExtended.UI.Panels
{
    public class UICemetaryEmptyingPanel : UIConfigurationPanel
    {
        public override void Awake()
        {
            this.Data = ConfigurationDataManager.Data.Cemetary;

            base.Awake();
        }
    }
}
