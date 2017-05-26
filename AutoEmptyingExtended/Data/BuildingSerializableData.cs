using System.Collections.Generic;
using System.IO;
using System.Linq;
using ColossalFramework;
using ColossalFramework.IO;
using ICities;

namespace AutoEmptyingExtended.Data
{
    public class BuildingSerializableData : SerializableDataExtensionBase
    {
        private const string DataId = "AEE_BuildingData";
        private const uint DataVersion = 0;


        private BuildingDataContainer[] FilterData(BuildingDataContainer[] data)
        {
            var list = new List<ushort>();
            var buffer = Singleton<BuildingManager>.instance.m_buildings.m_buffer;
            for (ushort i = 0; i < buffer.Length; i++)
            {
                if (buffer[i].m_flags == Building.Flags.None)
                    continue;

                var buildingAi = buffer[i].Info.m_buildingAI;
                if (!buildingAi.CanBeEmptied())
                    continue;

                if (buildingAi is LandfillSiteAI || buildingAi is CemeteryAI)
                {
                    list.Add(i);
                }
            }
            return data
                .Where(d => list.Contains(d.BuildingId))
                .ToArray();
        }
        

        public override void OnLoadData()
        {
            base.OnLoadData();

            // get bytes from savegame
            var bytes = serializableDataManager.LoadData(DataId);
            if (bytes == null || bytes.Length == 0) return;

            BuildingDataContainer[] data;
            try
            {
                // deserialize data from byte[]
                using (var stream = new MemoryStream(bytes))
                    data = DataSerializer.DeserializeArray<BuildingDataContainer>(stream, DataSerializer.Mode.Memory);
            }
            catch
            {
                // sometimes deserialization fials -> use default configuration
                Logger.LogError($"Failed to load the configuration (ID {DataId}) from savegame. Defaults were used.");
                serializableDataManager.EraseData(DataId);
                return;
            }

            foreach (var buildingData in data)
                BuildingDataManager.Data[buildingData.BuildingId] = buildingData;

            Logger.Log($"Configuration (ID {DataId}) was loaded ({bytes.Length} bytes)");
        }
        public override void OnSaveData()
        {
            base.OnSaveData();

            // serialize data to bytes[]
            var data = BuildingDataManager.Data.Values.ToArray();
            data = FilterData(data);

            byte[] bytes;
            using (var stream = new MemoryStream())
            {
                DataSerializer.SerializeArray(stream, DataSerializer.Mode.Memory, DataVersion, data);
                bytes = stream.ToArray();
            }

            // save bytes in savegame
            serializableDataManager.SaveData(DataId, bytes);

            Logger.Log($"Configuration (ID {DataId}) was saved ({bytes.Length} bytes)");
        }
    }
}
