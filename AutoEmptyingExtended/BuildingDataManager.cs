using System.Collections.Generic;
using System.IO;
using System.Linq;
using ColossalFramework.IO;
using ICities;

namespace AutoEmptyingExtended
{
    public class BuildingDataManager : SerializableDataExtensionBase
    {
        private const string DataId = "AEE_BuildingData";
        private const uint DataVersion = 0;

        public static Dictionary<ushort, BuildingData> BuildingData;


        public override void OnCreated(ISerializableData serializableData)
        {
            base.OnCreated(serializableData);

            // create new empty data dictionary
            BuildingData = new Dictionary<ushort, BuildingData>();
        }
        
        public override void OnLoadData()
        {
            base.OnLoadData();

            // get bytes from savegame
            var bytes = serializableDataManager.LoadData(DataId);
            if (bytes == null) return;

            BuildingData[] data;

            // deserialize data from byte[]
            using (var stream = new MemoryStream(bytes))
                data = DataSerializer.DeserializeArray<BuildingData>(stream, DataSerializer.Mode.Memory);

            foreach (var e in data)
                BuildingData.Add(e.BuildingId, e);

            Logger.Log("data loaded ({0} bytes)", bytes.Length);
        }

        public override void OnSaveData()
        {
            base.OnSaveData();

            // serialize data to bytes[]
            var data = BuildingData.Values.ToArray();

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

    public class BuildingData : IDataContainer
    {
        public ushort BuildingId;

        public bool AutoEmptyingDisabled;
        public bool AutoFillingDisabled;

        public void Serialize(DataSerializer s)
        {
            s.WriteUInt16(BuildingId);
            s.WriteBool(AutoEmptyingDisabled);
            s.WriteBool(AutoFillingDisabled);
        }

        public void Deserialize(DataSerializer s)
        {
            BuildingId = (ushort)s.ReadUInt16();
            AutoEmptyingDisabled = s.ReadBool();
            AutoFillingDisabled = s.ReadBool();
        }

        public void AfterDeserialize(DataSerializer s) { }
    }
}
