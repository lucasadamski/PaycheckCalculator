using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PayrollCalculator
{
    internal class Program
    {
        public static Controller controller;

        static void Main(string[] args)
        {
            controller = new Controller();
            string[] options = new string[] { "Calculate Pay", "Settings", "Save Settings", "Load Settings", "Exit" };
            Menu menu = new Menu(options, "Payroll Calculator For Lorry Drivers");

            while (true)
            {
                switch (menu.Initialize())
                {
                    case 0:
                        controller.Initialize();
                        break;
                    case 1:
                        controller.InitializeSettingsMenu();
                        break;
                    case 2:
                        controller.SaveSettings(); //saves only current rates, no settings
                        break;
                    case 3:
                        controller.LoadSettings();
                        break;
                    case 4:
                        Environment.Exit(0);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}