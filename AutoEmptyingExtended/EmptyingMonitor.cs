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

            if (configuration.AutoEmptyingEnabled
                && (building.m_flags & Building.Flags.Downgrading) == Building.Flags.None
                && !serviceData.StartedAutomatically //verify that the user is not stopped manually
                && !serviceData.AutoEmptyingDisabled 
                && percentage >= configuration.EmptyingPercentStart
                && currentTime >= configuration.EmptyingTimeStart
                && currentTime < configuration.EmptyingTimeEnd)
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
                || currentTime >= configuration.EmptyingTimeEnd - 0.01)
            {
                if (serviceData.StartedAutomatically
                    && (building.m_flags & Building.Flags.Downgrading) == Building.Flags.Downgrading)
                {
                    buildingAi.SetEmptying(buildingId, ref building, false);
                }
                serviceData.StartedAutomatically = false;
            }
        }
       
        #endregion

        #region Methods
            
        public override void OnAfterSimulationTick()
        {
            var buffer = _buildingManager.m_buildings.m_buffer;
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
