using ICities;

namespace AutoEmptyingExtended.UI
{
    public class ThreadingExtension : ThreadingExtensionBase
    {
        public override void OnUpdate(float realTimeDelta, float simulationTimeDelta)
        {
            base.OnUpdate(realTimeDelta, simulationTimeDelta);

            if (ToolsModifierControl.toolController == null)
                return;

            if (ToolsModifierControl.toolController.CurrentTool.GetType() !=typeof(EmptyingTool))
            {
                //UI.Close();
            }

        }
    }
}
