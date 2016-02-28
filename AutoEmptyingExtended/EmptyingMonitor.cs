using System;
using ColossalFramework;
using ICities;

namespace AutoEmptyingExtended
{
    public class EmptyingMonitor : ThreadingExtensionBase
    {
        #region Fields

        private readonly BuildingManager _buildingManager;

        #endregion

        #region Ctor

        public EmptyingMonitor()
        {
            _buildingManager = Singleton<BuildingManager>.instance;
        }

        #endregion

        #region Utilities
        
        private void HandleLandfill(ushort buildingId, ref Building building)
        {
            var lsAi = building.Info.m_buildingAI as LandfillSiteAI;
            if (lsAi == null)
                throw new Exception("not landfill");

            var garbageAmount = building.m_customBuffer1 * 1000 + building.m_garbageBuffer;
            var percentage = garbageAmount / lsAi.m_garbageCapacity;

            if (percentage > 0.9f && (building.m_flags & Building.Flags.Downgrading) == Building.Flags.None)
            {
                lsAi.SetEmptying(buildingId, ref building, true);
            }
            else if (garbageAmount == 0)
            {
                lsAi.SetEmptying(buildingId, ref building, false);
            }
        }

        private void HandleCemetery(ushort buildingId, ref Building building)
        {
            var cemeteryAi = building.Info.m_buildingAI as CemeteryAI;
            if (cemeteryAi == null)
                throw new Exception("not cemetery");

            var corpseCount = building.m_customBuffer1;
            var percentage = corpseCount / cemeteryAi.m_graveCount;

            if (percentage > 0.9f && (building.m_flags & Building.Flags.Downgrading) == Building.Flags.None)
            {
                cemeteryAi.SetEmptying(buildingId, ref building, true);
            }
            else if (corpseCount == 0)
            {
                cemeteryAi.SetEmptying(buildingId, ref building, false);
            }
        }

        private void HandleSnowDump(ushort buildingId, ref BuildingAI buildingAi)
        {
            var snowDumpAi = buildingAi as SnowDumpAI;
            
            var snowAmount = _buildingManager.m_buildings.m_buffer[buildingId].m_customBuffer1 * 1000 + _buildingManager.m_buildings.m_buffer[buildingId].m_garbageBuffer;
            var percentage = snowAmount / snowDumpAi.m_snowCapacity;

            if (percentage > 0.9f && (_buildingManager.m_buildings.m_buffer[buildingId].m_flags & Building.Flags.Downgrading) == Building.Flags.None)
            {
                snowDumpAi.SetEmptying(buildingId, ref _buildingManager.m_buildings.m_buffer[buildingId], true);
            }
            else if (snowAmount == 0)
            {
                snowDumpAi.SetEmptying(buildingId, ref _buildingManager.m_buildings.m_buffer[buildingId], false);
            }
        }
        
        #endregion

        #region Methods

        public override void OnAfterSimulationTick()
        {
            for (ushort i = 0; i < _buildingManager.m_buildings.m_buffer.Length; i++)
            {
                var building = _buildingManager.m_buildings.m_buffer[i];
                if (building.m_flags == Building.Flags.None)
                    continue;

                //also mau be used = .Info.GetComponent<PlayerBuildingAI>();
                var buildingAi = building.Info.m_buildingAI;
                if (!buildingAi.CanBeEmptied())
                    continue;

                if (buildingAi is LandfillSiteAI)
                {
                    HandleLandfill(i, ref building);
                }
                else if (buildingAi is CemeteryAI)
                {
                    HandleCemetery(i, ref building);
                }
                //else if (buildingAi is SnowDumpAI)
                //{
                //    HandleSnowDump(i, ref buildingAi);
                //}
            }

            base.OnAfterSimulationTick();
        }

        #endregion
    }
}
