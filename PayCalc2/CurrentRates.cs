using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PayrollCalculator
{
    public struct CurrentRates
    {
        [JsonInclude]
        public string Name { get; private set; }
        [JsonInclude]
        public decimal Days             { get; private set; }
        [JsonInclude]
        public decimal DaysOT           { get; private set; }
        [JsonInclude]
        public decimal Nights           { get; private set; }
        [JsonInclude]
        public decimal NightsOT         { get; private set; }
        [JsonInclude]
        public decimal WeekendDays      { get; private set; }
        [JsonInclude]
        public decimal WeekendDaysOT    { get; private set; }
        [JsonInclude]
        public decimal WeekendNights    { get; private set; }
        [JsonInclude]
        public decimal WeekendNightsOT  { get; private set; }
        [JsonInclude]
        public decimal BHDays           { get; private set; }
        [JsonInclude]
        public decimal BHNights         { get; private set; }
        

        public CurrentRates(string name, decimal days, decimal daysOT, decimal nights, decimal nightsOT, decimal weekendDays, 
            decimal weekendDaysOT, decimal weekendNights, decimal weekendNightsOT, decimal bhDays, decimal bhNights)
        {
            Name            = name;
            Days            = days;
            DaysOT          = daysOT;
            Nights          = nights;
            NightsOT        = nightsOT;
            WeekendDays     = weekendDays;
            WeekendDaysOT   = weekendDaysOT;
            WeekendNights   = weekendNights;
            WeekendNightsOT = weekendNightsOT;
            BHDays          = bhDays;
            BHNights        = bhNights;
        }

        public CurrentRates()
        {
            Name = "Default";
            Days = 0.0M;
            DaysOT = 0.0M;
            Nights = 0.0M;
            NightsOT = 0.0M;
            WeekendDays = 0.0M;
            WeekendDaysOT = 0.0M;
            WeekendNights = 0.0M;
            WeekendNightsOT = 0.0M;
            BHDays = 0.0M;
            BHNights = 0.0M;
        }
    }
}
