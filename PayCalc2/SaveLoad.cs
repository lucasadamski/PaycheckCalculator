using System.Text.Json;

namespace PayrollCalculator
{
    public static class SaveLoad
    {
        public static void SaveRates(CurrentRates rates)
        {
            var jsonString = JsonSerializer.Serialize(rates);
            File.WriteAllText("settings.txt", jsonString);
        }

        public static CurrentRates LoadRates()      
        {
            string output = File.ReadAllText("settings.txt");
            CurrentRates rates = JsonSerializer.Deserialize<CurrentRates>(output);
            return rates;
        }

        public static void SaveSettings(Settings settings)
        {
            var jsonString = JsonSerializer.Serialize(settings);
            File.WriteAllText("settings.txt", jsonString);
        }

        public static Settings LoadSettings()
        {
            string output = File.ReadAllText("settings.txt");
            Settings settings = JsonSerializer.Deserialize<Settings>(output);
            return settings;
        }
    }
}