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
            if (bytes == null)
                return new ConfigurationDataContainer
                {
                    AutoEmptyingEnabled = true,
                    EmptyingPercentStart = 90,
                    EmptyingTimeStart = 0,
                    EmptyingTimeEnd = 24
                };

            using (var stream = new MemoryStream(bytes))
                return DataSerializer.Deserialize<ConfigurationDataContainer>(stream, DataSerializer.Mode.Memory);
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
        }

        //public override void OnCreated(ISerializableData serializableData)
        //{
        //    base.OnCreated(serializableData);

        //    //LandfillData = new ConfigurationData();
        //    //CemetaryData = new ConfigurationData();
        //}
        
        public override void OnLoadData()
        {
            base.OnLoadData();

            ConfigurationDataManager.Data.Landfill = LoadData(LandfillDataId);
            ConfigurationDataManager.Data.Cemetary = LoadData(CemetaryDataId);
        }

        public override void OnSaveData()
        {
            base.OnSaveData();

            SaveData(LandfillDataId, ConfigurationDataManager.Data.Landfill);
            SaveData(CemetaryDataId, ConfigurationDataManager.Data.Cemetary);
        }
    }
}
