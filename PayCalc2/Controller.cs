using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Transactions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PayrollCalculator
{

    public class Controller
    {
        public string Status { get; private set; }
        public string Prompt { get; private set; }
        public DateTime Start { get; private set; } = new DateTime();
        public DateTime Finish { get; private set; } = new DateTime();
        public RateMode RateMode { get; set; }
        public Settings DefaultSettings { get; private set; } = new Settings();
        public Settings _currentSettings;

        private List<CurrentRates> _rates = new List<CurrentRates>();
        private PaydayCalculator _paydayCalc;

        public Controller()
        {
            Status = string.Empty;
            Prompt = string.Empty;
            //Updated 2023 rates
            _rates.Add(new CurrentRates("Pre Parity", 14.65m, 21.47m, 17.17m, 24.00m, 22.47m, 25.38m, 25.00m, 27.91m, 26.38m, 28.91m));
            _rates.Add(new CurrentRates("Pre Parity With Holiday", 16.42m, 24.06m, 19.24m, 26.90m, 25.18m, 28.44m, 28.02m, 31.28m, 29.56m, 32.40m));
            _rates.Add(new CurrentRates("Post Parity", 15.65m, 23.47m, 18.17m, 26.00m, 23.47m, 27.38m, 26.00m, 29.91m, 27.38m, 29.91m));
            _rates.Add(new CurrentRates("Post Parity With Holiday", 17.54m, 26.30m, 20.36m, 29.14m, 26.30m, 30.68m, 29.14m, 33.52m, 30.68m, 33.52m));

            _currentSettings = new Settings("Default");
            _currentSettings.CurrentRates = _rates[3];

            

        }

        public void InitializeSettingsMenu()
        {
            string[] options = new string[] { "Guaranteed hours", "Over time treshold", "Deductable break", "Night hours start", "Day hours start", "Active rate", "Add new rate" };
            
            while (true)
            {
                SettingsMenu SettingsMenu = new SettingsMenu(options, "SETTINGS", ref _currentSettings);
                switch (SettingsMenu.Initialize())
                {
                    case 0:
                        Console.WriteLine("Enter guaranteed hours (HH:MM):  ");
                        try
                        {
                            _currentSettings.GuaranteedHours = ParseTimeSpan(Console.ReadLine());
                        }
                        catch (InvalidTimeFormatException e)
                        {
                            Console.WriteLine(e.MyMessage);
                            goto case 0;
                        }
                        break;
                    case 1:
                        Console.WriteLine("Enter overtime treshold (HH:MM):  ");
                        try
                        {
                            _currentSettings.OverTimeTres = ParseTimeSpan(Console.ReadLine());
                        }
                        catch (InvalidTimeFormatException e)
                        {
                            Console.WriteLine(e.MyMessage);
                            goto case 1;
                        }
                        break;
                    case 2:
                        Console.WriteLine("Enter deductable break (HH:MM):  ");
                        try
                        {
                            _currentSettings.DeductableBreak = ParseTimeSpan(Console.ReadLine());
                        }
                        catch (InvalidTimeFormatException e)
                        {
                            Console.WriteLine(e.MyMessage);
                            goto case 2;
                        }
                        break;
                    case 3:
                        Console.WriteLine("Enter night hours start time (HH:MM):  ");
                        try
                        {
                            _currentSettings.NightHoursStart = ParseTimeSpan(Console.ReadLine());
                        }
                        catch (InvalidTimeFormatException e)
                        {
                            Console.WriteLine(e.MyMessage);
                            goto case 3;
                        }
                        break;
                    case 4:
                        Console.WriteLine("Enter day hours start time (HH:MM):  ");
                        try
                        {
                            _currentSettings.DayHoursStart = ParseTimeSpan(Console.ReadLine());
                        }
                        catch (InvalidTimeFormatException e)
                        {
                            Console.WriteLine(e.MyMessage);
                            goto case 4;
                        }
                        break;
                    case 5:
                        RatesMenu chooseRateMenu = new RatesMenu(_rates, "Choose which rate to set");
                        while (true)
                        {
                            int menuOption = chooseRateMenu.Initialize();
                            if (menuOption < 0)
                                break;
                            else
                            {
                                _currentSettings.CurrentRates = _rates[menuOption];
                                break;
                            }
                        }
                        break;
                    case 6:
                        _rates.Add(AddNewRate());
                        break;
                    default:
                        return;

                };
            }
        }


        public void Initialize()
        {
            while (true)
            {
                Console.WriteLine("Please enter start of shift (DD.MM.YY HH:MM) if the finish is the other day or (HH:MM) if finish is the same day: ");
                string input = Console.ReadLine();
                Start = ParseDateTime(input);
                if (Start == new DateTime())
                {
                    Console.WriteLine($"I don't recognize {input} as a DD.MM.YY HH:MM or HH:MM format. Try again.");
                    continue;
                }
                break;
            }
            while (true)
            {
                Console.WriteLine("Please enter end of shift (DD.MM.YY HH:MM): ");
                string input = Console.ReadLine();
                Finish = ParseDateTime(input);
                if (Finish == new DateTime())
                {
                    Console.WriteLine($"I don't recognize {input} as a DD.MM.YY HH:MM or HH:MM format. Try again.");
                    continue;
                }
                if (Finish <= Start)
                {
                    Console.WriteLine($"Finish is earlier or same date as start. Try again.");
                    continue;
                }
                break;
            }

            while (true)
            {
                Console.WriteLine($"Please enter the rate.{Environment.NewLine}1. Week{Environment.NewLine}2. Weekend" +
                    $"{Environment.NewLine}3. Bank holiday{Environment.NewLine}0. Automatic (based on day of the week - bank holidays not included)");
                char key = Console.ReadKey(true).KeyChar;
                if ((key < '0') || (key > '3'))
                {
                    Console.WriteLine($"The key \'{key}\' is not between 0-3. Try again.");
                    continue;
                }
                RateMode = ParseRateType(key.ToString());
                _currentSettings.RateMode = RateMode;
                break;
            }

            _paydayCalc = new PaydayCalculator(Start, Finish, _currentSettings);

            ///Console.WriteLine(_paydayCalc.PaydayReport);
            Console.WriteLine(_paydayCalc.SmallReport);

            while (true)
            {
                Console.WriteLine($"1. Show more details{Environment.NewLine}2. Help - result is wrong." +
                    $"{Environment.NewLine}3. Main Menu{Environment.NewLine}4. Quit");
                char key = Console.ReadKey(true).KeyChar;
                if ((key < '1') || (key > '4'))
                {
                    Console.WriteLine($"The key \'{key}\' is not between 1-4. Try again.");
                    continue;
                }
                if (key == '1')
                {
                    //Console.WriteLine(_paydayCalc.PayableHoursReport);
                    Console.WriteLine(_paydayCalc.BigReport);
                    continue;
                }
                if (key == '2')
                {
                    Console.WriteLine("Please check the SETTINGS in main menu, such as guaranteed shift hours, automatic break deduction, hour rate");
                    continue;
                }
                if (key == '3')
                {
                    return;
                }
                if (key == '4')
                {
                    Environment.Exit(0);
                }

            }

        }

        /// <summary>
        /// Parses string "DD.MM.YY YY:HH" to a DateTime Object
        /// </summary>
        /// <param name="date"></param>
        /// <returns>Valid DateTime object or new DateTime() if string can't be parsed</returns>
        public DateTime ParseDateTime(string date)
        {
            
            if (!date.Contains(":")) return new DateTime();
            if (DateTime.TryParse(date, out DateTime dateValue))
            { 
                return dateValue;
            }
            
            return new DateTime();
        }
        /// <summary>
        /// Parses the string into RateMode type
        /// </summary>
        /// <param name="rate"></param>
        /// <returns>RateType or RateType.Automatic if char can't be parsed</returns>
        public RateMode ParseRateType(string rate)
        {
            RateMode rateMode;
            
            if (Enum.TryParse(rate.Trim(), out rateMode))
            {
                if (Enum.IsDefined(typeof(RateMode), rateMode))
                {
                    return rateMode;
                }
            }
            return RateMode.Automatic;
        }

        /// <summary>
        /// Parses string to a TimeSpan object
        /// </summary>
        /// <returns>TimeSpan object</returns>
        protected TimeSpan ParseTimeSpan(string hour)
        {
            if (!hour.Contains(":")) throw new InvalidTimeFormatException();
            if (TimeSpan.TryParse(hour, out TimeSpan hourValue))
            {
                return hourValue;
            }
            throw new InvalidTimeFormatException();
        }

        private decimal ParseHourRate(string rate)
        {
            while (true)
            {
                try
                { 
                    Console.Write($"Enter rate: {rate}");
                    string input = Console.ReadLine();
                    if (input.Contains(','))
                    {
                        Console.WriteLine("Please use dot '.' instead of comma ','");
                        continue;
                    }

                    return decimal.Parse(input);
                }
                catch(Exception e)
                {
                    Console.WriteLine($"I can't recognize a number, valide examples: 9 ;  10.25 ; 9.50");
                    continue;
                }
            }
        }

        private string ParseStringName()
        {
            while (true)
            {
                Begin:
                Console.Write("Enter name: (eg \"new rate\")");
                string str = Console.ReadLine();
                str = str.Trim();
                if (string.IsNullOrEmpty(str))
                {
                    Console.WriteLine("Name you entered is empty.");
                    continue;
                }
                if (str.Length > 32)
                {
                    Console.WriteLine("Enter name shorter than 32 characters.");
                    continue;
                }
                foreach (var item in _rates)
                {
                    if(item.Name == str)
                    {
                        Console.WriteLine("This name already exists.");
                        goto Begin;
                    }
                }
                return str;

            }
        }

        private CurrentRates AddNewRate()
        {
            CurrentRates addedRate = new CurrentRates();
            
            string _rateName = ParseStringName();

            decimal _days, _daysOT, _nights, _nightsOT, _weekendDays, _weekendDaysOT, _weekendNights, _weekendNightsOT,  
                _bhDays, _bhNights;
            
            _days = ParseHourRate("Days");
            _daysOT = ParseHourRate("DaysOT");
            _nights = ParseHourRate("Nights");
            _nightsOT = ParseHourRate("NightsOT");
            _weekendDays = ParseHourRate("WeekendDays");
            _weekendDaysOT = ParseHourRate("WeekendDaysOT");
            _weekendNights = ParseHourRate("WeekendNights");
            _weekendNightsOT = ParseHourRate("WeekendNightsOT");
            _bhDays = ParseHourRate("Bank Holiday Days");
            _bhNights = ParseHourRate("Bank Holiday Nights");

            return  new CurrentRates(_rateName, _days, _daysOT, _nights, _nightsOT, _weekendDays, _weekendDaysOT, _weekendNights, 
                _weekendNightsOT, _bhDays, _bhNights);

        }

        internal void SaveSettings()
        {
            // SaveLoad.SaveRates(_currentSettings.CurrentRates);

            SaveLoad.SaveSettings(_currentSettings);
        }

        internal void LoadSettings()
        {
            //  _currentSettings.CurrentRates = SaveLoad.LoadRates();
            _currentSettings = SaveLoad.LoadSettings();
        }



    }
}


