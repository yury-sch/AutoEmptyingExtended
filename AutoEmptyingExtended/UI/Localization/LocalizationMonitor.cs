using ICities;

namespace AutoEmptyingExtended.UI.Localization
{
    public class LocalizationMonitor : ThreadingExtensionBase
    {
        public override void OnBeforeSimulationTick()
        {
            LocalizationManager.Instance.CheckAndUpdateLocales();

            base.OnBeforeSimulationTick();
        }
    }
}