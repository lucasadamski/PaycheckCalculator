using PayrollCalculator;
using System.Text;
//using ApprovalTests;
//using ApprovalTests.Reporters;

namespace PayrollCalculatorTests
{
    [TestClass]
   // [UseReporter(typeof(DiffReporter))]
    public class PaydayCalculatorTests
    {
        CurrentRates postParityInLieu = new CurrentRates("Post Parity With Holiday", 16.7m, 25.05m, 19.4m, 27.75m, 25.05m, 29.24m, 27.75m, 31.93m, 29.24m, 31.93m);
        Settings sets = new Settings("test");
 
       [TestInitialize]
        public void TestInit()
        {
            sets.CurrentRates = postParityInLieu;
        }


        [TestMethod]
        public void CalculateDayAndNightHoursTest()
        {
  
            DateTime start = new DateTime(2023, 03, 7, 03, 0, 0);
            DateTime finish = new DateTime(2023, 03, 7, 15, 00, 0);

            PaydayCalculator PayDay = new PaydayCalculator(start, finish, sets);

            TimeSpan dayHours = new TimeSpan(9, 0, 0);
            TimeSpan nightHours = new TimeSpan(3, 0, 0);
            List<TimeSpan> testList = PayDay.CalculateDayNightHoursOnly(start, finish);
            Assert.AreEqual(dayHours, testList[0]);
            Assert.AreEqual(nightHours, testList[1]);

            start = new DateTime(2023, 03, 7, 03, 0, 0);
            finish = new DateTime(2023, 03, 7, 21, 00, 0);
            PayDay = new PaydayCalculator(start, finish, sets);
            dayHours = new TimeSpan(12, 0, 0);
            nightHours = new TimeSpan(6, 0, 0);
            testList = PayDay.CalculateDayNightHoursOnly(start, finish);
            Assert.AreEqual(dayHours, testList[0]);
            Assert.AreEqual(nightHours, testList[1]);

            start = new DateTime(2023, 03, 7, 05, 45, 0);
            finish = new DateTime(2023, 03, 7, 06, 15, 0);
            PayDay = new PaydayCalculator(start, finish, sets);
            dayHours = new TimeSpan(0, 15, 0);
            nightHours = new TimeSpan(0, 15, 0);
            testList = PayDay.CalculateDayNightHoursOnly(start, finish);
            Assert.AreEqual(dayHours, testList[0]);
            Assert.AreEqual(nightHours, testList[1]);

            start = new DateTime(2023, 03, 7, 06, 15, 0);
            finish = new DateTime(2023, 03, 7, 18, 15, 0);
            PayDay = new PaydayCalculator(start, finish, sets);
            dayHours = new TimeSpan(11, 45, 0);
            nightHours = new TimeSpan(0, 15, 0);
            testList = PayDay.CalculateDayNightHoursOnly(start, finish);
            Assert.AreEqual(dayHours, testList[0]);
            Assert.AreEqual(nightHours, testList[1]);

            start = new DateTime(2023, 03, 7, 06, 15, 0);
            finish = new DateTime(2023, 03, 7, 06, 00, 0);
            PayDay = new PaydayCalculator(start, finish, sets);
            dayHours = new TimeSpan(0, 0, 0);
            nightHours = new TimeSpan(0, 0, 0);
            testList = PayDay.CalculateDayNightHoursOnly(start, finish);
            Assert.AreEqual(dayHours, testList[0]);
            Assert.AreEqual(nightHours, testList[1]);

            start = new DateTime(2023, 03, 7, 00, 00, 0);
            finish = new DateTime(2023, 03, 10, 00, 00, 0);
            PayDay = new PaydayCalculator(start, finish, sets);
            dayHours = new TimeSpan(36, 0, 0);
            nightHours = new TimeSpan(36, 0, 0);
            testList = PayDay.CalculateDayNightHoursOnly(start, finish);
            Assert.AreEqual(dayHours, testList[0]);
            Assert.AreEqual(nightHours, testList[1]);
        }


        [TestMethod]
        public void DeductBreakTest()
        {
            DateTime start = new DateTime(2023, 03, 7, 8, 0, 0);
            DateTime finish = new DateTime(2023, 03, 7, 15, 00, 0);
            PaydayCalculator PayDay = new PaydayCalculator(start, finish, sets, true);
            TimeSpan shiftLenght = new TimeSpan();
            PayDay.DeductBreakTime();
            shiftLenght = PayDay.ShiftEnd - PayDay.ShiftStart;
            Assert.AreEqual(new TimeSpan(8,0,0), shiftLenght);
           

            start = new DateTime(2023, 03, 7, 8, 0, 0);
            finish = new DateTime(2023, 03, 7, 16, 00, 0);
            PayDay = new PaydayCalculator(start, finish, sets, true);
            PayDay.DeductBreakTime();
            shiftLenght = PayDay.ShiftEnd - PayDay.ShiftStart;
            Assert.AreEqual(new TimeSpan(8, 0, 0), shiftLenght);

            start = new DateTime(2023, 03, 7, 8, 0, 0);
            finish = new DateTime(2023, 03, 7, 16, 15, 0);
            PayDay = new PaydayCalculator(start, finish, sets, true);
            PayDay.DeductBreakTime();
            shiftLenght = PayDay.ShiftEnd - PayDay.ShiftStart;
            Assert.AreEqual(new TimeSpan(8, 0, 0), shiftLenght);

            start = new DateTime(2023, 03, 7, 8, 0, 0);
            finish = new DateTime(2023, 03, 7, 16, 45, 0);
            PayDay = new PaydayCalculator(start, finish, sets, true);
            PayDay.DeductBreakTime();
            shiftLenght = PayDay.ShiftEnd - PayDay.ShiftStart;
            Assert.AreEqual(new TimeSpan(8, 0, 0), shiftLenght);


            start = new DateTime(2023, 03, 7, 8, 0, 0);
            finish = new DateTime(2023, 03, 7, 17, 00, 0);
            PayDay = new PaydayCalculator(start, finish, sets, true);
            PayDay.DeductBreakTime();
            shiftLenght = PayDay.ShiftEnd - PayDay.ShiftStart;
            Assert.AreEqual(new TimeSpan(8, 15, 0), shiftLenght);

            start = new DateTime(2023, 03, 7, 8, 0, 0);
            finish = new DateTime(2023, 03, 7, 18, 00, 0);
            PayDay = new PaydayCalculator(start, finish, sets, true);
            PayDay.DeductBreakTime();
            shiftLenght = PayDay.ShiftEnd - PayDay.ShiftStart;
            Assert.AreEqual(new TimeSpan(9, 15, 0), shiftLenght);
        }

        [TestMethod]
        public void CalculatePayTest()
        {

            DateTime start = new DateTime(2023, 03, 7, 8, 0, 0);
            DateTime finish = new DateTime(2023, 03, 7, 15, 00, 0);
            PaydayCalculator PayDay = new PaydayCalculator(start, finish, sets);
            //PayDay.CalculateAll();
            Assert.AreEqual(133.6M, PayDay.TotalHours);

            start = new DateTime(2023, 03, 7, 8, 0, 0);
            finish = new DateTime(2023, 03, 7, 16, 00, 0);
            PayDay = new PaydayCalculator(start, finish, sets);
           // PayDay.CalculateAll();
            Assert.AreEqual(133.6M, PayDay.TotalHours);

            start = new DateTime(2023, 03, 7, 8, 0, 0);
            finish = new DateTime(2023, 03, 7, 16, 15, 0);
            PayDay = new PaydayCalculator(start, finish, sets);
            //PayDay.CalculateAll();
            Assert.AreEqual(133.6M, PayDay.TotalHours);

            start = new DateTime(2023, 03, 7, 8, 0, 0);
            finish = new DateTime(2023, 03, 7, 17, 00, 0);
            PayDay = new PaydayCalculator(start, finish, sets);
            //PayDay.CalculateAll();
            Assert.AreEqual(139.8625M, PayDay.TotalHours);

            start = new DateTime(2023, 03, 7, 8, 0, 0);
            finish = new DateTime(2023, 03, 7, 18, 00, 0);
            PayDay = new PaydayCalculator(start, finish, sets);
            //PayDay.CalculateAll();
            Assert.AreEqual(164.9125M, PayDay.TotalHours);

            start = new DateTime(2023, 03, 7, 8, 0, 0);
            finish = new DateTime(2023, 03, 7, 21, 00, 0);
            PayDay = new PaydayCalculator(start, finish, sets);
            //PayDay.CalculateAll();
            Assert.AreEqual(246.1375M, PayDay.TotalHours);

            start = new DateTime(2023, 03, 7, 8, 0, 0);
            finish = new DateTime(2023, 03, 8, 8, 00, 0);
            PayDay = new PaydayCalculator(start, finish, sets);
            //PayDay.isdebugging = true;
            //PayDay.CalculateAll();
            Assert.AreEqual(548.0125M, PayDay.TotalHours);
        }

        [TestMethod]
        public void BankHolidayPayTest()
        {
            sets.RateMode = RateMode.BankHoliday;
            DateTime start = new DateTime(2023, 03, 7, 8, 0, 0);
            DateTime finish = new DateTime(2023, 03, 7, 16, 00, 0);
            PaydayCalculator PayDay = new PaydayCalculator(start, finish, sets);
            //PayDay.CalculateAll();
            Assert.AreEqual(233.92M, PayDay.TotalHours);

            start = new DateTime(2023, 03, 7, 8, 0, 0);
            finish = new DateTime(2023, 03, 7, 20, 00, 0);
            PayDay = new PaydayCalculator(start, finish, sets);
            //PayDay.CalculateAll();
            Assert.AreEqual(332.3125M, PayDay.TotalHours);

            start = new DateTime(2023, 03, 7, 8, 0, 0);
            finish = new DateTime(2023, 03, 8, 8, 00, 0);
            PayDay = new PaydayCalculator(start, finish, sets);
            //PayDay.CalculateAll();
            Assert.AreEqual(712.11M, PayDay.TotalHours);
        }

        [TestMethod]
        public void SmallReportTest()
        {
            string smallReport = "----------------------------------" + Environment.NewLine;
            smallReport += String.Format("{0, -20}215,01", "Total Pay:");
            smallReport += Environment.NewLine + "----------------------------------";


            DateTime start = new DateTime(2001, 1, 1, 6, 0, 0);
            DateTime finish = new DateTime(2001, 1, 1, 18, 0, 0);
            PaydayCalculator PayDay = new PaydayCalculator(start, finish, sets);
            PayDay.GenerateSmallReport();
            Assert.AreEqual(smallReport, PayDay.SmallReport);

        }

        [TestMethod]
        public void BigReportWeekTest()
        {
            sets.RateMode = RateMode.Week;
            
            string bigReport = @"----------------------------------
Shift Start:        01.01.2001 06:00
Shift End:          01.01.2001 18:00
Rate Type:          Week
Rate Setting:       Post Parity With Holiday
Shift Length:       12,00 h
Break Deducted:     0,75 h
----------------------------------
        HOURS           RATE            TOTAL
Day:      8      *      16,70    =      133,60
Day OT:   3,25   *      25,05    =      81,41
Night:    0      *      19,40    =      0,00
Night OT: 0      *      27,75    =      0,00
                                    *** 215,01   ***";

            DateTime start = new DateTime(2001, 1, 1, 6, 0, 0);
            DateTime finish = new DateTime(2001, 1, 1, 18, 0, 0);
            PaydayCalculator PayDay = new PaydayCalculator(start, finish, sets);
            PayDay.GenerateBigReport();

            Assert.AreEqual(bigReport, PayDay.BigReport);
        }

        [TestMethod]
        public void BigReportWeekendTest()
        {
            sets.RateMode = RateMode.Weekend;

            string bigReport = @"----------------------------------
Shift Start:        01.01.2001 06:00
Shift End:          01.01.2001 18:00
Rate Type:          Weekend
Rate Setting:       Post Parity With Holiday
Shift Length:       12,00 h
Break Deducted:     0,75 h
----------------------------------
        HOURS           RATE            TOTAL
Day:      8      *      25,05    =      200,40
Day OT:   3,25   *      29,24    =      95,03
Night:    0      *      27,75    =      0,00
Night OT: 0      *      31,93    =      0,00
                                    *** 295,43   ***";

            DateTime start = new DateTime(2001, 1, 1, 6, 0, 0);
            DateTime finish = new DateTime(2001, 1, 1, 18, 0, 0);
            PaydayCalculator PayDay = new PaydayCalculator(start, finish, sets);
            PayDay.GenerateBigReport();

            Assert.AreEqual(bigReport, PayDay.BigReport);
        }


        [TestMethod]
        public void BigReportBankHolidayTest()
        {
            sets.RateMode = RateMode.BankHoliday;

            string bigReport = @"----------------------------------
Shift Start:        01.01.2001 06:00
Shift End:          01.01.2001 18:00
Rate Type:          Bank Holiday
Rate Setting:       Post Parity With Holiday
Shift Length:       12,00 h
Break Deducted:     0,75 h
----------------------------------
        HOURS           RATE            TOTAL
Day:      11,25  *      29,24    =      328,95
Night:    0      *      31,93    =      0,00
                                    *** 328,95   ***";

            DateTime start = new DateTime(2001, 1, 1, 6, 0, 0);
            DateTime finish = new DateTime(2001, 1, 1, 18, 0, 0);
            PaydayCalculator PayDay = new PaydayCalculator(start, finish, sets);
            PayDay.GenerateBigReport();

            Assert.AreEqual(bigReport, PayDay.BigReport);
        }



    }
}