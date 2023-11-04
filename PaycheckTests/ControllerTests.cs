using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PayrollCalculator;

namespace PayrollCalculatorTests
{
    [TestClass]
    public class ControllerTests : Controller
    {
        [TestMethod]
        public void ParseDateTimeTest()
        {
            Controller controller = new Controller();

            DateTime testDate = new DateTime(2001, 01, 01, 08, 00, 00);
            testDate = DateTime.Today;
            testDate = testDate.AddHours(8.0d);
            
            string testString = "08:00";
            Assert.AreEqual(testDate, controller.ParseDateTime(testString));
            testString = "asdf";
            Assert.AreEqual( new DateTime(), controller.ParseDateTime(testString));
        }
        [TestMethod]
        public void ParseRateTypeTest()
        {
            Controller controller = new Controller();

            string testString = "0";
            Assert.AreEqual(RateMode.Automatic, controller.ParseRateType(testString));
            testString = "3";
            Assert.AreEqual(RateMode.BankHoliday, controller.ParseRateType(testString));
            testString = "2";
            Assert.AreEqual(RateMode.Weekend, controller.ParseRateType(testString));
            testString = "x";
            Assert.AreEqual(RateMode.Automatic, controller.ParseRateType(testString));

        }

        [TestMethod]
        public void ParseTimeSpanTests()
        {
            ControllerTests controller = new ControllerTests();
            TimeSpan testTime = new TimeSpan(8, 0, 0);
            Assert.AreEqual(testTime, controller.ParseTimeSpan("08:00"));
             testTime = new TimeSpan(8, 15, 0);
            Assert.AreEqual(testTime, controller.ParseTimeSpan("08:15"));
             testTime = new TimeSpan(0, 0, 0);
            Assert.AreEqual(testTime, controller.ParseTimeSpan("00:00"));
             testTime = new TimeSpan(0, 0, 0);
            try
            {
                testTime = new TimeSpan(0, 0, 0);
                Assert.AreEqual(testTime, controller.ParseTimeSpan("43:00"));
                Assert.Fail();
            }
            catch(Exception e) { }
            try
            {
                testTime = new TimeSpan(0, 0, 0);
                Assert.AreEqual(testTime, controller.ParseTimeSpan("asdf"));
                Assert.Fail();
            }
            catch (Exception e) { }

            
        }
    }
}
