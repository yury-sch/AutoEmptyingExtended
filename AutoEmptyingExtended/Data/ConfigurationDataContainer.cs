using ColossalFramework.IO;

namespace AutoEmptyingExtended.Data
{
    public class ConfigurationDataContainer : IDataContainer
    {
        public bool AutoEmptyingDisabled { get; set; } = false;
        public bool HasJustChanged { get; set; } = false;
        public float EmptyingPercentStart { get; set; } = 90;
        public float EmptyingPercentStop { get; set; } = 10;
        public float EmptyingTimeStart { get; set; } = 0;
        public float EmptyingTimeEnd { get; set; } = 24;

        public void Serialize(DataSerializer s)
        {
            s.WriteBool(AutoEmptyingDisabled);
            s.WriteBool(HasJustChanged);
            s.WriteFloat(EmptyingPercentStart);
            s.WriteFloat(EmptyingPercentStop);
            s.WriteFloat(EmptyingTimeStart);
            s.WriteFloat(EmptyingTimeEnd);
        }

        public void Deserialize(DataSerializer s)
        {
            AutoEmptyingDisabled = s.ReadBool();
            HasJustChanged = s.ReadBool();
            EmptyingPercentStart = s.ReadFloat();
            EmptyingPercentStop = s.ReadFloat();
            EmptyingTimeStart = s.ReadFloat();
            EmptyingTimeEnd = s.ReadFloat();
        }

        public void AfterDeserialize(DataSerializer s) { }
    }
}