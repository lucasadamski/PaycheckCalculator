using static System.Console;

namespace PayrollCalculator
{
    class SettingsMenu : Menu
    {
        protected Settings _settings;
        public SettingsMenu(string[] options, string prompt, ref Settings settings) : base(options, prompt)
        {
            _settings = settings;
        }

        protected string SettingValue(int i) => i switch
        {
            0 => _settings.GuaranteedHours.ToString(),
            1 => _settings.OverTimeTres.ToString(),
            2 => _settings.DeductableBreak.ToString(),
            3 => _settings.NightHoursStart.ToString(),
            4 => _settings.DayHoursStart.ToString(),
            5 => _settings.CurrentRates.Name,
            _ => string.Empty
        };
        protected override void DrawItems()
        {
            for (int i = 0; i < _options.Length; i++)
            {
                if (_selectedIndex == i)
                {
                    ForegroundColor = ConsoleColor.Black;
                    BackgroundColor = ConsoleColor.White;
                    
                    Console.WriteLine("{0, -20} {1,30}", _options[i], SettingValue(i));
                    ResetColor();
                }
                else
                {
                    Console.WriteLine("{0, -20} {1,30}", _options[i], SettingValue(i));
                }
            }
        }
    }
}
