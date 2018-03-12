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

        //private bool CanBeEmptied(ushort buildingId, ref Building data)
        //{
        //    var ai = data.Info.m_buildingAI;

        //    var serviceBuildings = _buildingManager.GetServiceBuildings(ai.m_info.m_class.m_service);

        //    if (serviceBuildings.m_buffer == null || serviceBuildings.m_size > serviceBuildings.m_buffer.Length)
        //        return false;

        //    for (var index = 0; index < serviceBuildings.m_size; ++index)
        //    {
        //        var serviceBuildingId = serviceBuildings.m_buffer[index];
        //        if (serviceBuildingId != 0 && serviceBuildingId != buildingId)
        //        {
        //            var sInfo = _buildingManager.m_buildings.m_buffer[serviceBuildingId].Info;

        //            //Logger.LogDebug(() =>
        //            //{
        //            //    return $"{sInfo.m_class.m_level}";
        //            //});

        //            if (sInfo.m_class.m_service == ai.m_info.m_class.m_service &&
        //                sInfo.m_class.m_level == ai.m_info.m_class.m_level &&
        //                ((_buildingManager.m_buildings.m_buffer[serviceBuildingId].m_flags & Building.Flags.Active) != Building.Flags.None &&
        //                 _buildingManager.m_buildings.m_buffer[serviceBuildingId].m_productionRate != 0) &&
        //                !sInfo.m_buildingAI.IsFull(serviceBuildingId,
        //                    ref _buildingManager.m_buildings.m_buffer[serviceBuildingId]))
        //            {
        //                var siteAI = sInfo.m_buildingAI as LandfillSiteAI;
        //                if (siteAI != null && siteAI.m_electricityProduction > 0)
        //                    return true;
        //                var cemeteryAI = sInfo.m_buildingAI as CemeteryAI;
        //                if (cemeteryAI != null && cemeteryAI.m_graveCount == 0)
        //                    return true;
        //            }

        //        }
        //    }
        //    return false;
        //}

        private bool CanBeEmptied(ItemClass.Service serviceType)
        {
            var serviceBuildings = _buildingManager.GetServiceBuildings(serviceType);

            if (serviceBuildings.m_buffer == null || serviceBuildings.m_size > serviceBuildings.m_buffer.Length)
                return false;

            for (var index = 0; index < serviceBuildings.m_size; ++index)
            {
                var serviceBuildingId = serviceBuildings.m_buffer[index];
                if (serviceBuildingId == 0) continue;

                var sInfo = _buildingManager.m_buildings.m_buffer[serviceBuildingId].Info;

                if ((_buildingManager.m_buildings.m_buffer[serviceBuildingId].m_flags & Building.Flags.Active) ==
                    Building.Flags.None ||
                    _buildingManager.m_buildings.m_buffer[serviceBuildingId].m_productionRate == 0 ||
                    sInfo.m_buildingAI.IsFull(serviceBuildingId,
                        ref _buildingManager.m_buildings.m_buffer[serviceBuildingId])) continue;

                switch (serviceType)
                {
                    case ItemClass.Service.Garbage:
                        var siteAI = sInfo.m_buildingAI as LandfillSiteAI;
                        if (siteAI != null && siteAI.m_electricityProduction > 0)
                            return true;
                        break;
                    case ItemClass.Service.HealthCare:
                        var cemeteryAI = sInfo.m_buildingAI as CemeteryAI;
                        if (cemeteryAI != null && cemeteryAI.m_graveCount == 0)
                            return true;
                        break;
                }
            }
            return false;
        }

        private void HandleEmptyingService(ushort buildingId, ref Building building, ConfigurationDataContainer configuration, bool canBeEmptied)
        {
            var buildingAi = building.Info.m_buildingAI;

            var serviceData = _buildingDataManager[buildingId];
            var currentTime = DayNightProperties.instance.m_TimeOfDay;

            var amount = GetAmount(ref building);
            var capacity = GetCapacity(ref building);
            var percentage = (float)amount / capacity * 100;

            if (!configuration.AutoEmptyingDisabled
                && (building.m_flags & Building.Flags.Downgrading) == Building.Flags.None
                && !serviceData.StartedAutomatically //verify that the user is not stopped manually
                && !serviceData.AutoEmptyingDisabled
                && percentage >= configuration.EmptyingPercentStart
                && currentTime >= configuration.EmptyingTimeStart
                && currentTime < configuration.EmptyingTimeEnd
                && canBeEmptied)
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
            //seems to not work, need revision
            else if (!configuration.AutoEmptyingDisabled
                     && !serviceData.StartedAutomatically //verify that the user is not stopped manually
                     && !serviceData.AutoEmptyingDisabled
                     && configuration.HasJustChanged 
                     && percentage < configuration.EmptyingPercentStart)
            {
                 configuration.HasJustChanged = false;
                 buildingAi.SetEmptying(buildingId, ref building, false);
                 serviceData.StartedAutomatically = false;
            }

            else if (configuration.AutoEmptyingDisabled
                     || serviceData.AutoEmptyingDisabled
                     || percentage < configuration.EmptyingPercentStop
                     || currentTime >= configuration.EmptyingTimeEnd - 0.01
                     || !canBeEmptied)
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
            var lCanBeEmptied = CanBeEmptied(ItemClass.Service.Garbage);
            var cCanBeEmptied = CanBeEmptied(ItemClass.Service.HealthCare);

            var buffer = _buildingManager.m_buildings.m_buffer;
            for (ushort i = 0; i < buffer.Length; i++)
            {
                if (buffer[i].m_flags == Building.Flags.None)
                    continue;

                if (!buffer[i].Info.m_buildingAI.CanBeEmptied())
                    continue;

                if (buffer[i].Info.m_buildingAI is LandfillSiteAI)
                {
                    HandleEmptyingService(i, ref buffer[i], _confidurationManager.Landfill, lCanBeEmptied);
                }
                else if (buffer[i].Info.m_buildingAI is CemeteryAI)
                {
                    HandleEmptyingService(i, ref buffer[i], _confidurationManager.Cemetary, cCanBeEmptied);
                }
            }

            base.OnAfterSimulationTick();
        }

        #endregion
    }
}
