using System;
using System.Text;
using AutoEmptyingExtended.Data;
using ColossalFramework;
using ICities;
using UnityEngine;

namespace AutoEmptyingExtended
{
    public class EmptyingMonitor : ThreadingExtensionBase
    {
        #region Fields

        private readonly BuildingManager _buildingManager;
        private readonly ConfigurationDataManager _configurationManager;
        private readonly BuildingDataManager _buildingDataManager;

        #endregion

        #region Ctor

        public EmptyingMonitor()
        {
            _buildingManager = Singleton<BuildingManager>.instance;
            _configurationManager = ConfigurationDataManager.Data;
            _buildingDataManager = BuildingDataManager.Data;
        }

        #endregion

        #region Utilities

        private int GetAmount(ref Building building)
        {
            BuildingAI buildingAI = building.Info.m_buildingAI;

            int amount = 0;
            if (buildingAI is LandfillSiteAI)
                amount = building.m_customBuffer1 * 1000 + building.m_garbageBuffer;
            else if (buildingAI is CemeteryAI)
                amount = building.m_customBuffer1;
            return amount;
        }

        private int GetCapacity(ref Building building)
        {
            BuildingAI buildingAI = building.Info.m_buildingAI;

            int capacity = 0;
            if (buildingAI is LandfillSiteAI)
                capacity = (buildingAI as LandfillSiteAI).m_garbageCapacity;
            else if (buildingAI is CemeteryAI)
                capacity = (buildingAI as CemeteryAI).m_graveCount;
            return capacity;
        }

        private void HandleEmptyingService(ushort buildingId, ref Building building, ConfigurationDataContainer configuration)
        {
            //MOD ENABLED
            if (configuration.AutoEmptyingDisabled == false)
            {
                BuildingAI buildingAi = building.Info.m_buildingAI;
                BuildingDataContainer serviceData = _buildingDataManager[buildingId];
                float currentTime = DayNightProperties.instance.m_TimeOfDay;

                float amount = GetAmount(ref building);
                float capacity = GetCapacity(ref building);
                int percentage = Convert.ToInt32( (amount / capacity) * 100 );
                //reduce processor usage
                //only process stuff if it is necessary
                if(percentage != serviceData.LastPercentage || configuration.HasJustChanged)
                    serviceData.LastPercentage = percentage;
                else
                    return;

                if (serviceData.AutoEmptyingDisabled == false)
                {
                    //if it has started automatically, it will keep
                    //emptying only in the determined period
                    //until it gets the target percentage
                    if (serviceData.StartedAutomatically == true)
                    {
                        if(currentTime >= configuration.EmptyingTimeStart //0
                           && currentTime <= configuration.EmptyingTimeEnd)
                        {
                            buildingAi.SetEmptying(buildingId, ref building, true);
                        }
                        else if (currentTime <= configuration.EmptyingTimeStart
                                 || currentTime >= configuration.EmptyingTimeEnd - 0.01)
                        {
                            buildingAi.SetEmptying(buildingId, ref building, false);
                        }

                        if (percentage <= configuration.EmptyingPercentStop)
                        {
                            buildingAi.SetEmptying(buildingId, ref building, false);
                            serviceData.StartedAutomatically = false;
                        }
                    }
                    else
                    {
                        //START CONDITIONS
                        if (percentage >= configuration.EmptyingPercentStart //90
                            && currentTime >= configuration.EmptyingTimeStart //0
                            && currentTime <= configuration.EmptyingTimeEnd //24
                            )
                        {
                            buildingAi.SetEmptying(buildingId, ref building, true);
                            serviceData.StartedAutomatically = true;
                        }
                        //In Case of changing values
                        //if it is out of percentage interval
                        if (percentage <= configuration.EmptyingPercentStart
                            && percentage >= configuration.EmptyingPercentStop)
                        {
                            buildingAi.SetEmptying(buildingId, ref building, false);
                        }
                        //if it is out of time interval
                        if (currentTime <= configuration.EmptyingTimeStart
                            || currentTime >= configuration.EmptyingTimeEnd - 0.01)
                        {
                            buildingAi.SetEmptying(buildingId, ref building, false);
                        }
                    }

                }
                //disabled for this building
                else
                {
                    serviceData.StartedAutomatically = false;
                }
            }
            //MOD DISABLED
            else
            {
                BuildingDataContainer serviceData = _buildingDataManager[buildingId];
                serviceData.StartedAutomatically = false;
            }
        }

        #endregion

        #region Methods

        public override void OnAfterSimulationTick()
        {
            if(_configurationManager.Landfill.AutoEmptyingDisabled == false)
            {
                if (threadingManager.simulationTick % 512 == 0 && !threadingManager.simulationPaused)
                {
                    Building[] buffer = _buildingManager.m_buildings.m_buffer;
                    for (ushort i = 0; i < buffer.Length; i++)
                    {
                        if (buffer[i].Info.m_buildingAI.CanBeEmptied() == false)
                            continue;

                        if (buffer[i].Info.m_buildingAI is LandfillSiteAI)
                            HandleEmptyingService(i, ref buffer[i], _configurationManager.Landfill);

                        else if (buffer[i].Info.m_buildingAI is CemeteryAI)
                            HandleEmptyingService(i, ref buffer[i], _configurationManager.Cemetary);
                    }
                    if (_configurationManager.Landfill.HasJustChanged)
                        _configurationManager.Landfill.HasJustChanged = false;
                    if (_configurationManager.Cemetary.HasJustChanged)
                        _configurationManager.Cemetary.HasJustChanged = false;
                }
            }
            base.OnAfterSimulationTick();
        }

        #endregion
    }
}
