using System.Reflection;
using ICities;

namespace AutoEmptyingExtended
{
    public class ModIdentity : IUserMod
    {
        public string Name => "Automatic Emptying: Extended";

        public string Description
        {
            get
            {
                return "Automatically clicks the \"Empty building to another facility\" buttons of your cemeteries and garbages to start emptying when they are almost filled up and stop when empty."
#if DEBUG
                    + " v." + Assembly.GetExecutingAssembly().GetName().Version
#endif
                ;
            }
        }
    }
}
