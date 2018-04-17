using ColossalFramework.IO;

namespace AutoEmptyingExtended.Data
{
    public class BuildingDataContainer : IDataContainer
    {
        public ushort BuildingId { get; set; }

        public bool AutoEmptyingDisabled { get; set; }
        public bool StartedAutomatically { get; set; }

        public int LastPercentage { get; set; }

        public void Serialize(DataSerializer s)
        {
            s.WriteUInt16(BuildingId);
            s.WriteBool(AutoEmptyingDisabled);
            s.WriteBool(StartedAutomatically);
            s.WriteInt32(LastPercentage);
        }

        public void Deserialize(DataSerializer s)
        {
            BuildingId = (ushort)s.ReadUInt16();
            AutoEmptyingDisabled = s.ReadBool();
            StartedAutomatically = s.ReadBool();
            LastPercentage = s.ReadInt32();
        }

        public void AfterDeserialize(DataSerializer s) { }
    }
}