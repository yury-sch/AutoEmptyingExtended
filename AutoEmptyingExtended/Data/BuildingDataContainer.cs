using ColossalFramework.IO;

namespace AutoEmptyingExtended.Data
{
    public class BuildingDataContainer : IDataContainer
    {
        public ushort BuildingId { get; set; }

        public bool AutoEmptyingDisabled { get; set; }
        public bool AutoFillingDisabled { get; set; }

        public bool StartedAutomatically { get; set; }

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