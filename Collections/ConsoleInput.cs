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
            while (true)
            {
                Console.Write(prompt);
                var isNumber = int.TryParse(ReadLine(), CultureInfo.InvariantCulture, out var value);
                if (isNumber && value >= minValue && value <= maxValue)
                {
                    return value;
                }

                Console.WriteLine($"Нужно целое число от {minValue} до {maxValue}.");
            }
        }

        public static List<int> ReadIntList(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (TryParseIntList(ReadLine(), out var numbers))
                {
                    return numbers;
                }

                Console.WriteLine("Нужно ввести целые числа через пробел, например: 1 2 3 4 5.");
            }
        }

        private static bool TryParseIntList(string text, out List<int> numbers)
        {
            numbers = new List<int>();
            foreach (var part in text.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            {
                if (!int.TryParse(part, CultureInfo.InvariantCulture, out var number))
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
