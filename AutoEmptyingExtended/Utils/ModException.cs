using System;

namespace AutoEmptyingExtended.Utils
{
    public class DetailedException : Exception
    {
        public DetailedException(string message) : base($"{message}" +
                                                        "\n -----------------------------------------------" +
                                                        "\n Please, notify the mod authors on SteamWorkshop" +
                                                        "\n -----------------------------------------------") { }
    }
}
