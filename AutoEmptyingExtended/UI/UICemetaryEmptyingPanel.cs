using AutoEmptyingExtended.Data;

namespace AutoEmptyingExtended.UI
{
    public class UICemetaryEmptyingPanel : UIEmptyingPanel
    {
        public override void Start()
        {
            this.Data = ConfigurationDataManager.Data.Cemetary;

            base.Start();
        }
    }
}
