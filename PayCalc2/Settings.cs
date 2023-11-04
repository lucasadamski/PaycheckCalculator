using System.Text.Json.Serialization;

namespace PayrollCalculator
{
    public struct Settings
    {
        [JsonInclude]
        public string Name { get; set; }
        [JsonInclude]
        public  TimeSpan GuaranteedHours { get; set; }
        [JsonInclude]
        public  TimeSpan OverTimeTres { get; set; }
        [JsonInclude]
        public  TimeSpan DeductableBreak { get; set; }
        [JsonInclude]
        public  TimeSpan DayHoursStart { get; set; }
        [JsonInclude]
        public  TimeSpan NightHoursStart { get; set; }
        [JsonInclude]
        public CurrentRates CurrentRates { get; set; }
        [JsonInclude]
        public RateMode RateMode { get; set; }

        public Settings()
        {
           
        }
        public Settings(string name)
        {
            Name = name;
            GuaranteedHours = new TimeSpan(8, 0, 0);
            OverTimeTres = new TimeSpan(8, 0, 0);
            DeductableBreak = new TimeSpan(0, 45, 0);
            NightHoursStart = new TimeSpan(18, 0, 0);
            DayHoursStart = new TimeSpan(6, 0, 0);
            RateMode = RateMode.Automatic;
        }
        public Settings(string name, CurrentRates rate)
        {
            Name = name;
            CurrentRates = rate;
        }

        public Settings(string name, TimeSpan guaranteedHours, TimeSpan overTimeTres, TimeSpan deductableBreak,
            TimeSpan dayHoursStart, TimeSpan nightHoursStart)
        {
            Name = name;
            GuaranteedHours = guaranteedHours;
            OverTimeTres = overTimeTres;
            DeductableBreak = deductableBreak;
            NightHoursStart = dayHoursStart;
            DayHoursStart = nightHoursStart;
            RateMode = RateMode.Automatic;
        }

    }
}


