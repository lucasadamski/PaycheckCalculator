using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayrollCalculator
{

    public class PaydayCalculator
    {
        
        public PaydayCalculator(DateTime shiftStart, DateTime shiftEnd, Settings settings, bool test = false)
        {
            ShiftStart = shiftStart;
            ShiftEnd = shiftEnd;
            HourRates = settings.CurrentRates;
            ShiftDuration = ShiftEnd - ShiftStart;

            SetSettings(settings);
            SetRateMode(settings.RateMode);
            if(!test)
                CalculateAll();
        }

      
        //TimeSpan output
        public TimeSpan payableHoursDayRegular { get; set; }
        public TimeSpan payableHoursNightRegular { get; set; }
        public TimeSpan payableHoursDayOvertime { get; set; }
        public TimeSpan payableHoursNightOvertime { get; set; }
        public TimeSpan payableHoursBankHoliday { get; set; }

        //decimal output
        public decimal TotalHours { get; set; }
        public decimal DayRegular { get; set; }
        public decimal DayOvertime { get; set; }
        public decimal NightRegular { get; set; }
        public decimal NightOvertime { get; set; }
        public decimal BankHolidayDay { get; set; }
        public decimal BankHolidayNight { get; set; }


        //Fields
        protected TimeSpan DeductedBreak;
        protected TimeSpan ShiftDuration;
        protected Settings _settings;
        //Other props
        public DateTime ShiftStart { get; set; }
        public DateTime ShiftEnd { get; set; }
        public CurrentRates HourRates { get; set; }
        public RateMode RateMode { get; set; }
        public string? PayableHoursReport { get; set; }
        public string? PaydayReport { get; set; }
        public string? ReportRateAndEquation { get; set; }
        public string? SmallReport { get; set; }
        public string? BigReport { get; set; }


        public void GenerateBigReport()
        {
            DateTime _shiftEnd = ShiftStart + ShiftDuration;


            if(_settings.RateMode == RateMode.Week)
            {
                StringBuilder output = new StringBuilder("----------------------------------" + Environment.NewLine);
                output.Append(String.Format("{0, -20}{1}", "Shift Start:", ShiftStart.ToString("g")) + Environment.NewLine);
                output.Append(String.Format("{0, -20}{1}", "Shift End:", _shiftEnd.ToString("g")) + Environment.NewLine);
                output.Append(String.Format("{0, -20}{1}", "Rate Type:", RateMode.ToString()) + Environment.NewLine);
                output.Append(String.Format("{0, -20}{1}", "Rate Setting:", _settings.CurrentRates.Name) + Environment.NewLine);
                output.Append(String.Format("{0, -20}{1:F2} h", "Shift Length:", ShiftDuration.TotalHours) + Environment.NewLine);
                output.Append(String.Format("{0, -20}{1:F2} h", "Break Deducted:", DeductedBreak.TotalHours) + Environment.NewLine);
                output.Append("----------------------------------" + Environment.NewLine);
                output.Append("        HOURS           RATE            TOTAL" + Environment.NewLine);
                output.Append(String.Format("{0,-10}{1,-7}{2,-7}{3,-9:F2}{4,-7}{5:F2}", "Day:", payableHoursDayRegular.TotalHours, '*', _settings.CurrentRates.Days, '=', DayRegular) + Environment.NewLine);
                output.Append(String.Format("{0,-10}{1,-7}{2,-7}{3,-9:F2}{4,-7}{5:F2}", "Day OT:", payableHoursDayOvertime.TotalHours, '*', _settings.CurrentRates.DaysOT, '=', DayOvertime) + Environment.NewLine);
                output.Append(String.Format("{0,-10}{1,-7}{2,-7}{3,-9:F2}{4,-7}{5:F2}", "Night:", payableHoursNightRegular.TotalHours, '*', _settings.CurrentRates.Nights, '=', NightRegular) + Environment.NewLine);
                output.Append(String.Format("{0,-10}{1,-7}{2,-7}{3,-9:F2}{4,-7}{5:F2}", "Night OT:", payableHoursNightOvertime.TotalHours, '*', _settings.CurrentRates.NightsOT, '=', NightOvertime) + Environment.NewLine);
                output.Append(String.Format("{0,40}{1,-9:F2}{2}", "*** ", TotalHours, "***"));
                BigReport = output.ToString();
                return;
            }
            else if (_settings.RateMode == RateMode.Weekend)
            {
                StringBuilder output = new StringBuilder("----------------------------------" + Environment.NewLine);
                output.Append(String.Format("{0, -20}{1}", "Shift Start:", ShiftStart.ToString("g")) + Environment.NewLine);
                output.Append(String.Format("{0, -20}{1}", "Shift End:", _shiftEnd.ToString("g")) + Environment.NewLine);
                output.Append(String.Format("{0, -20}{1}", "Rate Type:", RateMode.ToString()) + Environment.NewLine);
                output.Append(String.Format("{0, -20}{1}", "Rate Setting:", _settings.CurrentRates.Name) + Environment.NewLine);
                output.Append(String.Format("{0, -20}{1:F2} h", "Shift Length:", ShiftDuration.TotalHours) + Environment.NewLine);
                output.Append(String.Format("{0, -20}{1:F2} h", "Break Deducted:", DeductedBreak.TotalHours) + Environment.NewLine);
                output.Append("----------------------------------" + Environment.NewLine);
                output.Append("        HOURS           RATE            TOTAL" + Environment.NewLine);
                output.Append(String.Format("{0,-10}{1,-7}{2,-7}{3,-9:F2}{4,-7}{5:F2}", "Day:", payableHoursDayRegular.TotalHours, '*', _settings.CurrentRates.WeekendDays, '=', DayRegular) + Environment.NewLine);
                output.Append(String.Format("{0,-10}{1,-7}{2,-7}{3,-9:F2}{4,-7}{5:F2}", "Day OT:", payableHoursDayOvertime.TotalHours, '*', _settings.CurrentRates.WeekendDaysOT, '=', DayOvertime) + Environment.NewLine);
                output.Append(String.Format("{0,-10}{1,-7}{2,-7}{3,-9:F2}{4,-7}{5:F2}", "Night:", payableHoursNightRegular.TotalHours, '*', _settings.CurrentRates.WeekendNights, '=', NightRegular) + Environment.NewLine);
                output.Append(String.Format("{0,-10}{1,-7}{2,-7}{3,-9:F2}{4,-7}{5:F2}", "Night OT:", payableHoursNightOvertime.TotalHours, '*', _settings.CurrentRates.WeekendNightsOT, '=', NightOvertime) + Environment.NewLine);
                output.Append(String.Format("{0,40}{1,-9:F2}{2}", "*** ", TotalHours, "***"));
                BigReport = output.ToString();
                return;
            }
            if (_settings.RateMode == RateMode.BankHoliday)
            {
                StringBuilder output = new StringBuilder("----------------------------------" + Environment.NewLine);
                output.Append(String.Format("{0, -20}{1}", "Shift Start:", ShiftStart.ToString("g")) + Environment.NewLine);
                output.Append(String.Format("{0, -20}{1}", "Shift End:", _shiftEnd.ToString("g")) + Environment.NewLine);
                output.Append(String.Format("{0, -20}{1}", "Rate Type:", "Bank Holiday") + Environment.NewLine);
                output.Append(String.Format("{0, -20}{1}", "Rate Setting:", _settings.CurrentRates.Name) + Environment.NewLine);
                output.Append(String.Format("{0, -20}{1:F2} h", "Shift Length:", ShiftDuration.TotalHours) + Environment.NewLine);
                output.Append(String.Format("{0, -20}{1:F2} h", "Break Deducted:", DeductedBreak.TotalHours) + Environment.NewLine);
                output.Append("----------------------------------" + Environment.NewLine);
                output.Append("        HOURS           RATE            TOTAL" + Environment.NewLine);
                output.Append(String.Format("{0,-10}{1,-7}{2,-7}{3,-9:F2}{4,-7}{5:F2}", "Day:", (payableHoursDayRegular.TotalHours + payableHoursDayOvertime.TotalHours), '*', _settings.CurrentRates.BHDays, '=', BankHolidayDay) + Environment.NewLine);
                output.Append(String.Format("{0,-10}{1,-7}{2,-7}{3,-9:F2}{4,-7}{5:F2}", "Night:", (payableHoursNightRegular.TotalHours + payableHoursNightOvertime.TotalHours), '*', _settings.CurrentRates.BHNights, '=', BankHolidayNight) + Environment.NewLine);
                output.Append(String.Format("{0,40}{1,-9:F2}{2}", "*** ", TotalHours, "***"));
                BigReport = output.ToString();
                return;
            }

        }

        public void GenerateSmallReport()
        {
            StringBuilder output = new StringBuilder("----------------------------------" + Environment.NewLine);
            output.Append(String.Format("{0, -20}{1:F2}", "Total Pay:", TotalHours));
            output.Append(Environment.NewLine + "----------------------------------");
            SmallReport = output.ToString();
        }

        public void SetSettings(Settings settings)
        {
            _settings = settings;
        }

        public void SetRateMode(RateMode rateMode)
        {
            if (rateMode == RateMode.Automatic)
            {
                if (ShiftStart.DayOfWeek == DayOfWeek.Saturday || ShiftStart.DayOfWeek == DayOfWeek.Sunday)
                {
                    this.RateMode = RateMode.Weekend;
                    return;
                }
                else
                {
                    this.RateMode = RateMode.Week;
                    return;
                }
            }
            else
            {
                this.RateMode = rateMode;
                return;
            }
        }
        public void CalculateAll()
        {
            DeductBreakTime();
            CalculateOvertimeAndDayNightHours(ShiftStart, ShiftEnd); // fires CalculateDayAndNightHoursOnly(), SetPayableHours()
            CalculatePay();
            GenerateSmallReport();
            GenerateBigReport();
        }
        public void DeductBreakTime()
        {
            TimeSpan ShiftLength = ShiftEnd - ShiftStart;
            TimeSpan GuarateedAndDeductableBreak = _settings.GuaranteedHours + _settings.DeductableBreak;
            if (ShiftLength <= _settings.GuaranteedHours) { ShiftEnd = ShiftStart + _settings.GuaranteedHours; }
            else if (ShiftLength > _settings.GuaranteedHours && ShiftLength <= GuarateedAndDeductableBreak) { ShiftEnd = ShiftStart + _settings.GuaranteedHours; }
            else if (ShiftLength > GuarateedAndDeductableBreak) { ShiftEnd -= _settings.DeductableBreak; DeductedBreak = _settings.DeductableBreak; }

        }
        public void SetPayableHours(TimeSpan dayRegular, TimeSpan nightRegular, TimeSpan dayOvertime, TimeSpan nightOvertime)
        {
            payableHoursDayRegular = dayRegular;
            payableHoursNightRegular = nightRegular;
            payableHoursDayOvertime = dayOvertime;
            payableHoursNightOvertime = nightOvertime;

        }
        

       

        public void CalculatePay()
        {
            if (RateMode == RateMode.Week)
            {
                DayRegular = (decimal)payableHoursDayRegular.TotalHours * HourRates.Days;
                DayOvertime = (decimal)payableHoursDayOvertime.TotalHours * HourRates.DaysOT;
                NightRegular = (decimal)payableHoursNightRegular.TotalHours * HourRates.Nights;
                NightOvertime = (decimal)payableHoursNightOvertime.TotalHours * HourRates.NightsOT;

                TotalHours = DayRegular + DayOvertime + NightRegular + NightOvertime;
            }
            if (RateMode == RateMode.Weekend)
            {
                DayRegular = (decimal)payableHoursDayRegular.TotalHours * HourRates.WeekendDays;
                DayOvertime = (decimal)payableHoursDayOvertime.TotalHours * HourRates.WeekendDaysOT;
                NightRegular = (decimal)payableHoursNightRegular.TotalHours * HourRates.WeekendNights;
                NightOvertime = (decimal)payableHoursNightOvertime.TotalHours * HourRates.WeekendNightsOT;

                TotalHours = DayRegular + DayOvertime + NightRegular + NightOvertime;
            }
            if (RateMode == RateMode.BankHoliday)
            {
                BankHolidayDay = ((decimal)payableHoursDayRegular.TotalHours + (decimal)payableHoursDayOvertime.TotalHours) * HourRates.BHDays;
                BankHolidayNight = ((decimal)payableHoursNightRegular.TotalHours + (decimal)payableHoursNightOvertime.TotalHours) * HourRates.BHNights;

                TotalHours = BankHolidayDay + BankHolidayNight;
            }
        }

        public List<TimeSpan> CalculateDayNightHoursOnly(DateTime shiftStartTime, DateTime shiftEndTime)
        {
            if (shiftStartTime.CompareTo(shiftEndTime) >= 0)
            {
                return new List<TimeSpan>() { new TimeSpan(), new TimeSpan() };
            }

            DateTime start = shiftStartTime;
            DateTime finish = shiftEndTime;
            /******************************************************************************
                 * Below algorithm checks how many hour of worked shift is day time hours (between 0600 and 1800) and
                 * how many is night time hours (outside of day time hours bracket). 
                 * First IF is to check if the start time is between midnight and 0600.
                 * Second IF deals with day hours between 0600 and 1800.
                 * Third IF deals with night hours between 1800 and 0600 of a next day.
                 * After every of above IFs we check if the finish time is in that scope, if yes, we close the loop.
                 * Otherwise we add the hours to the dayHours/nightHours counters, set iterator to the
                 * end of that scope and start the loop again.
                 * ******************************************************************************/
            DateTime dayHoursStart = new DateTime(start.Year, start.Month, start.Day, _settings.DayHoursStart.Hours, _settings.DayHoursStart.Minutes, 0);
            DateTime dayHoursEnd = new DateTime(start.Year, start.Month, start.Day, _settings.NightHoursStart.Hours, _settings.NightHoursStart.Minutes, 0);
            DateTime midnightSameDay = new DateTime(start.Year, start.Month, start.Day, 0, 0, 0);
            TimeSpan dayHours = TimeSpan.FromHours(0);
            TimeSpan nightHours = TimeSpan.FromHours(0);

            DateTime iterator = start;
            do
            {
                if ((iterator >= midnightSameDay) && (iterator < dayHoursStart))
                {
                    if (finish <= dayHoursStart) { nightHours = finish - iterator; iterator = finish; break; } //NO ADDING!!!
                    else { nightHours = dayHoursStart - iterator; iterator = dayHoursStart; continue; } //NO ADDING!!!
                }
                else if ((iterator >= dayHoursStart) && (iterator < dayHoursEnd))
                {
                    if (finish <= dayHoursEnd) { dayHours = dayHours.Add(finish - iterator); iterator = finish; break; }
                    else { dayHours = dayHours.Add(dayHoursEnd - iterator); iterator = dayHoursEnd; continue; }
                }
                else if ((iterator >= dayHoursEnd) && (iterator < dayHoursStart.AddDays(1)))
                {
                    if (finish <= dayHoursStart.AddDays(1)) { nightHours = nightHours.Add(finish - iterator); iterator = finish; break; }
                    else { dayHoursStart = dayHoursStart.AddDays(1); dayHoursEnd = dayHoursEnd.AddDays(1); nightHours = nightHours.Add(dayHoursStart - iterator); iterator = dayHoursStart; continue; }
                }

            }
            while (iterator != finish);
            /********
             * End of algorithm
             * ******/

            return new List<TimeSpan>() { dayHours, nightHours };
        }
        /// <summary>
        /// Calculates which hours are regular day, regular night, overtime day and overtime night
        /// </summary>
        /// <param name="shiftStartTime"></param>
        /// <param name="shiftEndTime"></param>
        /// <returns>If Count == 2 only regular time, if Cout == 4 also overtime</returns>
        public void CalculateOvertimeAndDayNightHours(DateTime shiftStartTime, DateTime shiftEndTime)
        {
            if (shiftStartTime.CompareTo(shiftEndTime) >= 0)
            {
                SetPayableHours(TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero);
                return;
            }
            DateTime start = shiftStartTime;
            DateTime finish = shiftEndTime;

            TimeSpan shiftDuration = finish - start;

            List<TimeSpan> hoursRegular;
            List<TimeSpan> hoursOvertime;

            if (shiftDuration <= _settings.OverTimeTres)
            {
                hoursRegular = CalculateDayNightHoursOnly(start, finish);
                SetPayableHours(hoursRegular[0], hoursRegular[1], TimeSpan.Zero, TimeSpan.Zero);
            }
            else
            {
                DateTime regularHoursFinish = start.Add(_settings.OverTimeTres);
                hoursRegular = CalculateDayNightHoursOnly(start, regularHoursFinish);
                hoursOvertime = CalculateDayNightHoursOnly(regularHoursFinish, finish);
                SetPayableHours(hoursRegular[0], hoursRegular[1], hoursOvertime[0], hoursOvertime[1]);
            }

        }
    }
}

