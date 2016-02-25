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
        
        private void HandleLandfill(ushort buildingId, ref BuildingAI buildingAi)
        {
            var lsAi = buildingAi as LandfillSiteAI;

            if (lsAi.m_electricityProduction > 0)
                return;

            var garbageAmount = _buildingManager.m_buildings.m_buffer[buildingId].m_customBuffer1 * 1000 + _buildingManager.m_buildings.m_buffer[buildingId].m_garbageBuffer;
            var percentage = garbageAmount / lsAi.m_garbageCapacity;

            if (percentage > 0.9f && (_buildingManager.m_buildings.m_buffer[buildingId].m_flags & Building.Flags.Downgrading) == Building.Flags.None)
            {
                lsAi.SetEmptying(buildingId, ref _buildingManager.m_buildings.m_buffer[buildingId], true);
            }
            else if (garbageAmount == 0)
            {
                lsAi.SetEmptying(buildingId, ref _buildingManager.m_buildings.m_buffer[buildingId], false);
            }
        }

        private void HandleCemetery(ushort buildingId, ref BuildingAI buildingAi)
        {
            var cemeteryAi = buildingAi as CemeteryAI;

            if (cemeteryAi.m_graveCount == 0)
                return;
            
            var percentage = _buildingManager.m_buildings.m_buffer[buildingId].m_customBuffer1 / cemeteryAi.m_graveCount;
            if (percentage > 0.9f && (_buildingManager.m_buildings.m_buffer[buildingId].m_flags & Building.Flags.Downgrading) == Building.Flags.None)
            {
                cemeteryAi.SetEmptying(buildingId, ref _buildingManager.m_buildings.m_buffer[buildingId], true);
            }
            else if (_buildingManager.m_buildings.m_buffer[buildingId].m_customBuffer1 == 0)
            {
                cemeteryAi.SetEmptying(buildingId, ref _buildingManager.m_buildings.m_buffer[buildingId], false);
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
                if (_buildingManager.m_buildings.m_buffer[i].m_flags == Building.Flags.None)
                    continue;
                
                BuildingAI buildingAi = _buildingManager.m_buildings.m_buffer[i].Info.GetComponent<PlayerBuildingAI>();

                if (buildingAi is LandfillSiteAI)
                {
                    HandleLandfill(i, ref buildingAi);
                }
                else if (buildingAi is CemeteryAI)
                {
                    HandleCemetery(i, ref buildingAi);
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
