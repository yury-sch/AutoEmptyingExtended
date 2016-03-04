using System.IO;
using System.Linq;
using ColossalFramework.IO;
using ICities;

namespace AutoEmptyingExtended.Data
{
    public class BuildingSerializableData : SerializableDataExtensionBase
    {
        private const string DataId = "AEE_BuildingData";
        private const uint DataVersion = 0;

        //public static Dictionary<ushort, BuildingData> Data { private get; set; }
        
        //public override void OnCreated(ISerializableData serializableData)
        //{
        //    base.OnCreated(serializableData);

        //    // create new empty data dictionary
        //    //Data = new Dictionary<ushort, BuildingData>();
        //}
        
        public override void OnLoadData()
        {
            base.OnLoadData();

            // get bytes from savegame
            var bytes = serializableDataManager.LoadData(DataId);
            if (bytes == null) return;

            BuildingDataContainer[] data;

            // deserialize data from byte[]
            using (var stream = new MemoryStream(bytes))
                data = DataSerializer.DeserializeArray<BuildingDataContainer>(stream, DataSerializer.Mode.Memory);

            foreach (var buildingData in data)
                BuildingDataManager.Data[buildingData.BuildingId] = buildingData;

            Logger.Log("data loaded ({0} bytes)", bytes.Length);
        }

        public override void OnSaveData()
        {
            base.OnSaveData();

            // serialize data to bytes[]
            var data = BuildingDataManager.Data.Values.ToArray();
            
            byte[] bytes;
            using (var stream = new MemoryStream())
            {
                DataSerializer.SerializeArray(stream, DataSerializer.Mode.Memory, DataVersion, data);
                bytes = stream.ToArray();
            }

            // save bytes in savegame
            serializableDataManager.SaveData(DataId, bytes);

            Logger.Log("data saved ({0} bytes)", bytes.Length);
        }
    }
}
