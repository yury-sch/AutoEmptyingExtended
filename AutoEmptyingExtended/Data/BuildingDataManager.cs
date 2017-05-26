using System.Collections.Generic;
using ColossalFramework;

namespace AutoEmptyingExtended.Data
{
    public class BuildingDataManager
    {
        private static BuildingDataManager _instance;
        public static BuildingDataManager Data => _instance ?? (_instance = new BuildingDataManager());


        private BuildingDataManager()
        {
            _buildingData = new Dictionary<ushort, BuildingDataContainer>();
        }

        private readonly Dictionary<ushort, BuildingDataContainer> _buildingData;


        public BuildingDataContainer this[ushort id]
        {
            get
            {
                if (_buildingData.ContainsKey(id))
                    return _buildingData[id];

                var buildingManager = Singleton<BuildingManager>.instance;
                var buildingAi = buildingManager.m_buildings.m_buffer[id].Info.m_buildingAI;
                if (!(buildingAi is LandfillSiteAI || buildingAi is CemeteryAI)) return null;

                var buildingData = new BuildingDataContainer();
                _buildingData.Add(id, buildingData);

                return buildingData;
            }
            set
            {
                BuildingDataContainer buildingData = null;
                if (_buildingData.ContainsKey(id))
                    buildingData = _buildingData[id];
                else
                {
                    var buildingManager = Singleton<BuildingManager>.instance;
                    var buildingAi = buildingManager.m_buildings.m_buffer[id].Info.m_buildingAI;
                    if (buildingAi is LandfillSiteAI || buildingAi is CemeteryAI)
                    {
                        buildingData = new BuildingDataContainer();
                        _buildingData.Add(id, buildingData);
                    }
                }

                if (buildingData != null)
                    _buildingData[id] = value;
            }
        }

        public Dictionary<ushort, BuildingDataContainer>.ValueCollection Values => _buildingData.Values;
    }
}
