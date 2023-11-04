using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace PayrollCalculator
{
    internal class Menu
    {
        protected int _selectedIndex;
        protected string _prompt;
        protected string[] _options;  
        internal Menu(string[] options, string prompt = "")
        {
            _prompt = prompt;
            _options = options;
            _selectedIndex = 0;
        }

        protected Menu() 
        {
            _selectedIndex = 0;
        }
        
        protected virtual void DrawItems()
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
        }

        /// <summary>
        /// Draws a menu
        /// </summary>
        /// <returns>Positive zero-based number option for entering or -1 for escaping</returns>
        public int Initialize()
        {
            while (true)
            {
                Clear();
                Console.WriteLine(string.Format("{0, 50}", _prompt));
                DrawItems();
                ConsoleKeyInfo key = ReadKey(true);
                if ((key.Key == ConsoleKey.Escape) || (key.Key == ConsoleKey.LeftArrow))
                {
                    return -1;
                }
                if ((key.Key == ConsoleKey.Enter) || (key.Key == ConsoleKey.RightArrow))
                {
                    return _selectedIndex;
                }
                else if (key.Key == ConsoleKey.DownArrow)
                {
                    if (_selectedIndex == _options.Length - 1) _selectedIndex = 0;
                    else _selectedIndex++;
                }
                else if (key.Key == ConsoleKey.UpArrow)
                {
                    if (_selectedIndex == 0) _selectedIndex = _options.Length - 1;
                    else _selectedIndex--;
                }
            }
        }
    }
}
