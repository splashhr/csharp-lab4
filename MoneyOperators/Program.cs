using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace MoneyOperators
{
    internal static class Program
    {
        private const string Menu = """
            Лабораторная работа №4. Вариант 15. Задания 6-7 (Money)
            1 - конструкторы, свойства и ToString()
            2 - сравнение двух сумм
            3 - операции ++ и --
            4 - приведение к uint и double
            5 - вычитание копеек: Money - int и int - Money
            0 - выход
            """;

        private static void Main()
        {
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;
            long choice;
            while (true)
            {
                Console.WriteLine(Menu);
                choice = ReadInt64("Пункт: ", 0, 5);
                if (choice == 0)
                {
                    return;
                }

                RunItem(choice);
            }
        }

        private static void RunItem(long choice)
        {
            Console.WriteLine();
            try
            {
                switch (choice)
                {
                    case 1:
                        ShowConstructors();
                        break;
                    case 2:
                        ShowComparison();
                        break;
                    case 3:
                        ShowIncrementAndDecrement();
                        break;
                    case 4:
                        ShowCasts();
                        break;
                    case 5:
                        ShowSubtraction();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(choice));
                }
            }
            catch (InvalidOperationException exception)
            {
                Console.WriteLine($"Ошибка: {exception.Message}");
            }
            catch (OverflowException exception)
            {
                Console.WriteLine($"Ошибка: {exception.Message}");
            }

            Console.WriteLine();
        }

        private static void ShowConstructors()
        {
            var empty = new Money();
            Console.WriteLine($"new Money() = {empty}");

            var money = ReadMoney("Сумма");
            Console.WriteLine($"new Money(рубли, копейки) = {money}");
            Console.WriteLine($"Rubles = {money.Rubles}, Kopeks = {money.Kopeks}, TotalKopeks = {money.TotalKopeks}");

            var copy = new Money(money);
            Console.WriteLine($"new Money(копия) = {copy}");
            Console.WriteLine($"Копия - отдельный объект: {!ReferenceEquals(money, copy)}");
        }

        private static void ShowComparison()
        {
            var first = ReadMoney("Первая сумма");
            var second = ReadMoney("Вторая сумма");
            Console.WriteLine($"{first} равна {second}: {first.IsEqualTo(second)}");
            Console.WriteLine($"{first} больше {second}: {first.IsGreaterThan(second)}");
            Console.WriteLine($"{second} больше {first}: {second.IsGreaterThan(first)}");
        }

        private static void ShowIncrementAndDecrement()
        {
            var money = ReadMoney("Сумма");
            var original = money;
            money++;
            Console.WriteLine($"++ : {original} -> {money}");

            original = money;
            money--;
            money--;
            Console.WriteLine($"-- два раза: {original} -> {money}");
        }

        private static void ShowCasts()
        {
            var money = ReadMoney("Сумма");
            uint rubles = money;
            var kopeksInRubles = (double)money;
            Console.WriteLine($"uint (неявное): {rubles}");
            Console.WriteLine($"double (явное): {kopeksInRubles.ToString(CultureInfo.InvariantCulture)}");
        }

        private static void ShowSubtraction()
        {
            var money = ReadMoney("Сумма");
            var kopeks = (int)ReadInt64("Сколько копеек вычесть: ", int.MinValue, int.MaxValue);
            Console.WriteLine($"{money} - {kopeks} = {money - kopeks}");
            Console.WriteLine($"{kopeks} - {money} = {kopeks - money}");
            Console.WriteLine($"Исходная сумма не изменилась: {money}");
        }

        private static Money ReadMoney(string title)
        {
            Console.WriteLine($"{title}:");
            var rubles = (uint)ReadInt64("  рубли: ", 0, uint.MaxValue);
            var kopeks = (byte)ReadInt64("  копейки: ", 0, 99);
            return new Money(rubles, kopeks);
        }

        private static long ReadInt64(string prompt, long minValue, long maxValue)
        {
            string text;
            bool isNumber;
            long value;
            while (true)
            {
                Console.Write(prompt);
                text = Console.ReadLine() ?? throw new EndOfStreamException("Ввод с клавиатуры закончился.");
                isNumber = long.TryParse(text, CultureInfo.InvariantCulture, out value);
                if (isNumber && value >= minValue && value <= maxValue)
                {
                    return value;
                }

                Console.WriteLine($"Нужно целое число от {minValue} до {maxValue}.");
            }
        }
    }
}
