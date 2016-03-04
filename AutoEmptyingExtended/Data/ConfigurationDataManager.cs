namespace AutoEmptyingExtended.Data
{
    public class ConfigurationDataManager
    {
        private static ConfigurationDataManager _instance;
        private ConfigurationDataContainer _landfill;
        private ConfigurationDataContainer _cemetary;

        private ConfigurationDataManager()
        {
            _landfill = new ConfigurationDataContainer();
            _cemetary = new ConfigurationDataContainer();
        }

        public static ConfigurationDataManager Data => _instance ?? (_instance = new ConfigurationDataManager());

        public ConfigurationDataContainer Landfill { get { return _landfill; } set { _landfill = value; } }

        public ConfigurationDataContainer Cemetary { get { return _cemetary; } set { _cemetary = value; } }
    }
}
