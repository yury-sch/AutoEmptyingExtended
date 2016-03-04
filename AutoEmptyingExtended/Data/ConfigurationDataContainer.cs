using ColossalFramework.IO;

namespace AutoEmptyingExtended.Data
{
    public class ConfigurationDataContainer : IDataContainer
    {
        public bool AutoEmptyingEnabled { get; set; }
        public float EmptyingPercentStart { get; set; }
        public float EmptyingTimeStart { get; set; }
        public float EmptyingTimeEnd { get; set; }

        public void Serialize(DataSerializer s)
        {
            s.WriteBool(AutoEmptyingEnabled);
            s.WriteFloat(EmptyingPercentStart);
            s.WriteFloat(EmptyingTimeStart);
            s.WriteFloat(EmptyingTimeEnd);
        }

        public void Deserialize(DataSerializer s)
        {
            AutoEmptyingEnabled = s.ReadBool();
            EmptyingPercentStart = s.ReadFloat();
            EmptyingTimeStart = s.ReadFloat();
            EmptyingTimeEnd = s.ReadFloat();
        }

        public void AfterDeserialize(DataSerializer s) { }
    }
}