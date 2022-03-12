namespace EmulatorsRequester.Models
{
    public class ConfigDescriptor
    {
        public uint ReadInterval { get; set; }
        public bool IsLoopOverwrite { get; set; }
        public bool IsStartOnHighTemp { get; set; }
        public bool IsStartOnLowTemp { get; set; }
        public bool IsStartOnButtonPress { get; set; }
        public bool IsAlarmOnHighLimit { get; set; }
        public bool IsAlarmOnLowLimit { get; set; }
        public string TaskConfiguredTime { get; set; }
        public string DelayStartTime { get; set; }
        public int StartHighTemp { get; set; }
        public int StartLowTemp { get; set; }
        public int AlarmHighLimit { get; set; }
        public uint AlarmHighDelay { get; set; }
        public int AlarmLowLimit { get; set; }
        public uint AlarmLowDelay { get; set; }

    }
}
