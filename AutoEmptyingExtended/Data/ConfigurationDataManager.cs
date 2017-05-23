namespace AutoEmptyingExtended.Data
{
    public class ConfigurationDataManager
    {
        private static ConfigurationDataManager _instance;

        private ConfigurationDataManager()
        {
            Landfill = new ConfigurationDataContainer();
            Cemetary = new ConfigurationDataContainer();
        }

        public static ConfigurationDataManager Data => _instance ?? (_instance = new ConfigurationDataManager());

        public ConfigurationDataContainer Landfill { get; set; }

        public ConfigurationDataContainer Cemetary { get; set; }
    }
}
