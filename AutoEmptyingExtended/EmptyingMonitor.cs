using System;
using System.Text;
using AutoEmptyingExtended.Data;
using ColossalFramework;
using ICities;

namespace AutoEmptyingExtended
{
    public class EmptyingMonitor : ThreadingExtensionBase
    {
        #region Fields

        private readonly BuildingManager _buildingManager;
        private readonly ConfigurationDataManager _confidurationManager;
        private readonly BuildingDataManager _buildingDataManager;

        #endregion

        #region Ctor

        public EmptyingMonitor()
        {
            _buildingManager = Singleton<BuildingManager>.instance;
            _confidurationManager = ConfigurationDataManager.Data;
            _buildingDataManager = BuildingDataManager.Data;
        }

        #endregion
        
        #region Utilities
        
        private int GetAmount(ref Building building)
        {
            var buildingAI = building.Info.m_buildingAI;

            var amount = 0;
            if (buildingAI is LandfillSiteAI)
                amount = building.m_customBuffer1 * 1000 + building.m_garbageBuffer;
            else if (buildingAI is CemeteryAI)
                amount = building.m_customBuffer1;
            //var snowAmount = _buildingManager.m_buildings.m_buffer[buildingId].m_customBuffer1 * 1000 + _buildingManager.m_buildings.m_buffer[buildingId].m_garbageBuffer; //snowDump
            return amount;
        }

        private int GetCapacity(ref Building building)
        {
            var buildingAI = building.Info.m_buildingAI;

            var capacity = 0;
            if (buildingAI is LandfillSiteAI)
                capacity = ((LandfillSiteAI)buildingAI).m_garbageCapacity;
            else if (buildingAI is CemeteryAI)
                capacity = ((CemeteryAI)buildingAI).m_graveCount;
            //snowDumpAi.m_snowCapacity //snowDump
            return capacity;
        }

        private void HandleEmptyingService(ushort buildingId, ref Building building, ConfigurationDataContainer configuration)
        {
            var buildingAi = building.Info.m_buildingAI;

            var serviceData = _buildingDataManager[buildingId];
            var currentTime = DayNightProperties.instance.m_TimeOfDay;
            
            var amount = GetAmount(ref building);
            var capacity = GetCapacity(ref building);
            var percentage = ((float)amount / capacity) * 100;
            var canBeE = CanBeEmptied(buildingId, ref building);

            if (configuration.AutoEmptyingEnabled
                && (building.m_flags & Building.Flags.Downgrading) == Building.Flags.None
                && !serviceData.StartedAutomatically //verify that the user is not stopped manually
                && !serviceData.AutoEmptyingDisabled 
                && percentage >= configuration.EmptyingPercentStart
                && currentTime >= configuration.EmptyingTimeStart
                && currentTime < configuration.EmptyingTimeEnd
                && canBeE)
            {
                buildingAi.SetEmptying(buildingId, ref building, true);
                serviceData.StartedAutomatically = true;
                Logger.LogDebug(() =>
                {
                    var log = new StringBuilder();
                    log.AppendLine("Emptying started automatically.");
                    log.AppendLine($"[buildingId: {buildingId}]");
                    log.AppendLine($"[AI: {(buildingAi is LandfillSiteAI ? "LandfillSite" : buildingAi is CemeteryAI ? "Cemetery" : "")}]");
                    log.AppendLine($"[percentage: {percentage}]");
                    return log.ToString();
                });
            }
            else if (!configuration.AutoEmptyingEnabled
                || serviceData.AutoEmptyingDisabled
                || amount == 0
                || currentTime >= configuration.EmptyingTimeEnd - 0.01
                || !canBeE)
            {
                if (serviceData.StartedAutomatically
                    && (building.m_flags & Building.Flags.Downgrading) == Building.Flags.Downgrading)
                {
                    buildingAi.SetEmptying(buildingId, ref building, false);
                }
                serviceData.StartedAutomatically = false;
            }
        }

        public bool CanBeEmptied(ushort buildingId, ref Building data)
        {
            var ai = data.Info.m_buildingAI;

            var serviceBuildings = _buildingManager.GetServiceBuildings(ai.m_info.m_class.m_service);
            
            if (serviceBuildings.m_buffer == null || serviceBuildings.m_size > serviceBuildings.m_buffer.Length)
                return false;

            for (var index = 0; index < serviceBuildings.m_size; ++index)
            {
                var serviceBuildingId = serviceBuildings.m_buffer[index];
                if (serviceBuildingId != 0 && serviceBuildingId != buildingId)
                {
                    var sInfo = _buildingManager.m_buildings.m_buffer[serviceBuildingId].Info;
                    if (sInfo.m_class.m_service == ai.m_info.m_class.m_service &&
                        sInfo.m_class.m_level == ai.m_info.m_class.m_level &&
                        ((_buildingManager.m_buildings.m_buffer[serviceBuildingId].m_flags & Building.Flags.Active) != Building.Flags.None &&
                         _buildingManager.m_buildings.m_buffer[serviceBuildingId].m_productionRate != 0) &&
                        !sInfo.m_buildingAI.IsFull(serviceBuildingId,
                            ref _buildingManager.m_buildings.m_buffer[serviceBuildingId]))
                    {
                        var siteAI = sInfo.m_buildingAI as LandfillSiteAI;
                        if (siteAI != null && siteAI.m_electricityProduction > 0)
                            return true;
                        var cemeteryAI = sInfo.m_buildingAI as CemeteryAI;
                        if (cemeteryAI != null && cemeteryAI.m_graveCount == 0)
                            return true;
                    }

                }
            }
            return false;
        }

        #endregion

        #region Methods

        public override void OnAfterSimulationTick()
        {
            var buffer = _buildingManager.m_buildings.m_buffer;
            var temp = 0;
            for (ushort i = 0; i < buffer.Length; i++)
            {
                if (buffer[i].m_flags == Building.Flags.None)
                    continue;

                var buildingAi = buffer[i].Info.m_buildingAI;
                if (buildingAi is LandfillSiteAI)
                {
                    Logger.LogDebug(() =>
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine($"id = {i}");
                        sb.AppendLine($".m_garbageConsumption > 0 ?: {(buildingAi as LandfillSiteAI).m_garbageConsumption > 0}");
                        return sb.ToString();
                    });
                    
                    temp++;
                    //LandfillSiteAI asfa = buildingAi as LandfillSiteAI;
                }
            }
            

            for (ushort i = 0; i < buffer.Length; i++)
            {
                if (buffer[i].m_flags == Building.Flags.None)
                    continue;

                var buildingAi = buffer[i].Info.m_buildingAI;
                if (!buildingAi.CanBeEmptied())
                    continue;

                if (buildingAi is LandfillSiteAI)
                {
                    HandleEmptyingService(i, ref buffer[i], _confidurationManager.Landfill);
                }
                else if (buildingAi is CemeteryAI)
                {
                    HandleEmptyingService(i, ref buffer[i], _confidurationManager.Cemetary);
                }
            }

            base.OnAfterSimulationTick();
        }

        #endregion
    }
}
