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
                return "Manage your city cemeteries and landfills in an automatic way"
#if DEBUG
                    + " v." + Assembly.GetExecutingAssembly().GetName().Version
#endif
                ;
            }
        }
    }
}
