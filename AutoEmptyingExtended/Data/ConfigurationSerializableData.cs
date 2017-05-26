using System.IO;
using ColossalFramework.IO;
using ICities;

namespace AutoEmptyingExtended.Data
{
    public class ConfigurationSerializableData : SerializableDataExtensionBase
    {
        private const string LandfillDataId = "AEE_LandfillConfigurationData";
        private const string CemetaryDataId = "AEE_CemetaryConfigurationData";
        private const uint DataVersion = 0;
        

        private ConfigurationDataContainer LoadData(string dataId)
        {
            // get bytes from savegame
            var bytes = serializableDataManager.LoadData(dataId);
            if (bytes == null || bytes.Length == 0)
                return new ConfigurationDataContainer();

            try
            {
                using (var stream = new MemoryStream(bytes))
                {
                    var data = DataSerializer.Deserialize<ConfigurationDataContainer>(stream, DataSerializer.Mode.Memory);
                    Logger.Log($"Configuration (ID {dataId}) was loaded ({bytes.Length} bytes)");

                    return data;
                }
            }
            catch
            {
                // sometimes deserialization fials -> use default configuration
                Logger.LogError($"Failed to load the configuration (ID {dataId}) from savegame. Defaults were used.");
                serializableDataManager.EraseData(dataId);

                return new ConfigurationDataContainer();
            }
        }
        private void SaveData(string dataId, ConfigurationDataContainer data)
        {
            byte[] bytes;
            using (var stream = new MemoryStream())
            {
                DataSerializer.Serialize(stream, DataSerializer.Mode.Memory, DataVersion, data);
                bytes = stream.ToArray();
            }

            // save bytes in savegame
            serializableDataManager.SaveData(dataId, bytes);

            Logger.Log($"Configuration (ID {dataId}) was saved ({bytes.Length} bytes)");
        }


        public override void OnLoadData()
        {
            base.OnLoadData();

            ConfigurationDataManager.Data.Landfill = LoadData(LandfillDataId);
            ConfigurationDataManager.Data.Cemetary = LoadData(CemetaryDataId);

            Logger.Log(ConfigurationDataManager.Data.Landfill.AutoEmptyingDisabled.ToString());
            Logger.Log(ConfigurationDataManager.Data.Landfill.EmptyingTimeStart.ToString());
        }
        public override void OnSaveData()
        {
            base.OnSaveData();

            SaveData(LandfillDataId, ConfigurationDataManager.Data.Landfill);
            SaveData(CemetaryDataId, ConfigurationDataManager.Data.Cemetary);
        }
    }
}
