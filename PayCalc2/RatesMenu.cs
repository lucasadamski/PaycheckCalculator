using System.Drawing;
using System.Xml.Linq;
using static System.Console;

namespace PayrollCalculator
{
    internal class RatesMenu : Menu
    {
        private List<CurrentRates> _ratesList;
        internal RatesMenu(List<CurrentRates> ratesList, string prompt = "") : base()
        {
            _ratesList = ratesList;
            _prompt = prompt;
            _options = new string[ratesList.Count()];
            for (int i = 0; i < ratesList.Count(); i++)
            {
                _options[i] = ratesList[i].Name;
            }
            
        }

        protected override void DrawItems()
        {
            for (int i = 0; i < _options.Length; i++)
            {
                if (_selectedIndex == i)
                {
                    ForegroundColor = ConsoleColor.Black;
                    BackgroundColor = ConsoleColor.White;
                    Console.WriteLine(_options[i]);
                    ResetColor();
                }
                else
                {
                    Console.WriteLine(_options[i]);
                }
                
            }
            WriteLine(Environment.NewLine + Environment.NewLine + Environment.NewLine);

            WriteLine("{0, -20}  {1, 5}","Days", _ratesList[_selectedIndex].Days);
            BackgroundColor = ConsoleColor.DarkBlue;
            WriteLine("{0, -20}  {1, 5}","DaysOT", _ratesList[_selectedIndex].DaysOT);
            ResetColor();
            WriteLine("{0, -20}  {1, 5}","Nights", _ratesList[_selectedIndex].Nights);
            BackgroundColor = ConsoleColor.DarkGray;
            WriteLine("{0, -20}  {1, 5}", "NightsOT", _ratesList[_selectedIndex].NightsOT);
            ResetColor();
            WriteLine("{0, -20}  {1, 5}", "Nights", _ratesList[_selectedIndex].Nights);
            BackgroundColor = ConsoleColor.DarkGray;
            WriteLine("{0, -20}  {1, 5}", "WeekendDays", _ratesList[_selectedIndex].WeekendDays);
            ResetColor();
            WriteLine("{0, -20}  {1, 5}", "WeekendDaysOT", _ratesList[_selectedIndex].WeekendDaysOT);
            BackgroundColor = ConsoleColor.DarkGray;
            WriteLine("{0, -20}  {1, 5}", "WeekendNights", _ratesList[_selectedIndex].WeekendNights);
            ResetColor();
            WriteLine("{0, -20}  {1, 5}", "WeekendNightsOT", _ratesList[_selectedIndex].WeekendNightsOT);
            BackgroundColor = ConsoleColor.DarkGray;
            WriteLine("{0, -20}  {1, 5}", "BHDays", _ratesList[_selectedIndex].BHDays);
            ResetColor();
            WriteLine("{0, -20}  {1, 5}", "BHNights", _ratesList[_selectedIndex].BHNights);
            

            /*
            Days            = days;
            DaysOT          = daysOT;
            Nights          = nights;
            NightsOT        = nightsOT;
            WeekendDays     = weekendDays;
            WeekendDaysOT   = weekendDaysOT;
            WeekendNights   = weekendNights;
            WeekendNightsOT = weekendNightsOT;
            BHDays          = bhDays;
            BHNights        = bhNights;*/



        }

    }
}
