using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Collections
{
    internal static class ConsoleInput
    {
        public static int ReadInt32(string prompt, int minValue, int maxValue)
        {
            bool isNumber;
            int value;
            while (true)
            {
                Console.Write(prompt);
                isNumber = int.TryParse(ReadLine(), CultureInfo.InvariantCulture, out value);
                if (isNumber && value >= minValue && value <= maxValue)
                {
                    return value;
                }

                Console.WriteLine($"Нужно целое число от {minValue} до {maxValue}.");
            }
        }

        public static List<int> ReadIntList(string prompt)
        {
            List<int> numbers;
            while (true)
            {
                Console.Write(prompt);
                if (TryParseIntList(ReadLine(), out numbers))
                {
                    return numbers;
                }

                Console.WriteLine("Нужно ввести целые числа через пробел, например: 1 2 3 4 5.");
            }
        }

        private static bool TryParseIntList(string text, out List<int> numbers)
        {
            numbers = new List<int>();
            int number;
            foreach (var part in text.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            {
                if (!int.TryParse(part, CultureInfo.InvariantCulture, out number))
                {
                    return false;
                }

                numbers.Add(number);
            }

            return true;
        }

        private static string ReadLine()
        {
            return Console.ReadLine() ?? throw new EndOfStreamException("Ввод с клавиатуры закончился.");
        }
    }
}
