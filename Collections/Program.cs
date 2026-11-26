using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Collections
{
    internal static class Program
    {
        private const int MaxStudentCount = 30;
        private const string TextPath = "task4_text.txt";
        private const string StudentsPath = "task5_students.txt";
        private const string SampleText = "Варвара рано брала старую карту. Тарас рад!";
        private const string Menu = """
            Лабораторная работа №4. Вариант 15. Задания 1-5
            1 - List: количество различных элементов
            2 - LinkedList: заменить неравных соседей элемента E на F
            3 - HashSet: культуры в сельскохозяйственных кооперативах
            4 - HashSet: символы, которые есть в каждом слове
            5 - SortedList: школы со средним баллом выше среднего по району
            0 - выход
            """;

        private static readonly string[] s_cooperativeNames;
        private static readonly HashSet<string>[] s_cooperatives;

        static Program()
        {
            s_cooperativeNames = new string[] { "Рассвет", "Колос", "Нива" };
            s_cooperatives = new HashSet<string>[]
            {
                new HashSet<string> { "Пшеница", "Рожь", "Картофель", "Свёкла" },
                new HashSet<string> { "Пшеница", "Картофель", "Ячмень" },
                new HashSet<string> { "Пшеница", "Рожь", "Лён", "Картофель" },
            };
        }

        private static void Main()
        {
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;
            int taskNumber;
            while (true)
            {
                Console.WriteLine(Menu);
                taskNumber = ConsoleInput.ReadInt32("Номер задания: ", 0, 5);
                if (taskNumber == 0)
                {
                    return;
                }

                RunTask(taskNumber);
            }
        }

        private static void RunTask(int taskNumber)
        {
            Console.WriteLine();
            try
            {
                switch (taskNumber)
                {
                    case 1:
                        CountDistinct();
                        break;
                    case 2:
                        ReplaceNeighbours();
                        break;
                    case 3:
                        ShowCrops();
                        break;
                    case 4:
                        ShowCharsInEveryWord();
                        break;
                    case 5:
                        ShowBestSchools();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(taskNumber));
                }
            }
            catch (IOException exception)
            {
                Console.WriteLine($"Ошибка при работе с файлом: {exception.Message}");
            }
            catch (InvalidDataException exception)
            {
                Console.WriteLine($"Ошибка в данных файла: {exception.Message}");
            }

            Console.WriteLine();
        }

        private static void CountDistinct()
        {
            Console.WriteLine("Задание 1. Количество различных элементов списка");
            var list = ConsoleInput.ReadIntList("L = ");
            Console.WriteLine($"Список: {string.Join(' ', list)}");
            Console.WriteLine($"Различных элементов: {CollectionTasks.CountDistinct(list)}");
        }

        private static void ReplaceNeighbours()
        {
            Console.WriteLine("Задание 2. Если у элемента E соседи не равны, заменить их на F");
            var list = new LinkedList<int>(ConsoleInput.ReadIntList("L = "));
            var e = ConsoleInput.ReadInt32("E = ", int.MinValue, int.MaxValue);
            var f = ConsoleInput.ReadInt32("F = ", int.MinValue, int.MaxValue);
            Console.WriteLine($"До:    {string.Join(' ', list)}");
            var changed = CollectionTasks.ReplaceUnequalNeighbours(list, e, f);
            Console.WriteLine($"После: {string.Join(' ', list)}");
            Console.WriteLine($"Элементов E с неравными соседями: {changed}");
        }

        private static void ShowCrops()
        {
            Console.WriteLine("Задание 3. Культуры в кооперативах");
            for (var index = 0; index < s_cooperatives.Length; index++)
            {
                Console.WriteLine($"{s_cooperativeNames[index]}: {FormatSorted(s_cooperatives[index])}");
            }

            var everywhere = CollectionTasks.FindGrownEverywhere(s_cooperatives);
            var somewhere = CollectionTasks.FindGrownSomewhere(s_cooperatives);
            var exactlyOne = CollectionTasks.FindGrownInExactlyOne(s_cooperatives);
            Console.WriteLine($"Во всех кооперативах: {FormatSorted(everywhere)}");
            Console.WriteLine($"Только в некоторых: {FormatSorted(somewhere)}");
            Console.WriteLine($"Ровно в одном: {FormatSorted(exactlyOne)}");
        }

        private static void ShowCharsInEveryWord()
        {
            Console.WriteLine("Задание 4. Символы, которые есть в каждом слове");
            if (!File.Exists(TextPath))
            {
                File.WriteAllText(TextPath, SampleText);
            }

            var text = File.ReadAllText(TextPath);
            Console.WriteLine($"{TextPath}: {text}");
            var symbols = new List<char>(CollectionTasks.FindCharsInEveryWord(text));
            symbols.Sort();
            if (symbols.Count == 0)
            {
                Console.WriteLine("Общих символов нет.");
                return;
            }

            Console.WriteLine($"Есть в каждом слове: {string.Join(' ', symbols)}");
        }

        private static void ShowBestSchools()
        {
            Console.WriteLine("Задание 5. Школы со средним баллом выше среднего по району");
            var count = ConsoleInput.ReadInt32("Количество учеников: ", 5, MaxStudentCount);
            CollectionTasks.FillStudentsFile(StudentsPath, count, Random.Shared);
            Console.WriteLine($"{StudentsPath}:");
            Console.WriteLine(File.ReadAllText(StudentsPath));

            var schools = CollectionTasks.FindSchoolsAboveDistrictAverage(StudentsPath, out var districtAverage);
            Console.WriteLine($"Средний балл по району = {FormatScore(districtAverage)}");
            foreach (var school in schools.Keys)
            {
                Console.WriteLine($"Школа {school}: средний балл {FormatScore(schools[school])}");
            }

            Console.WriteLine("Ответ:");
            if (schools.Count == 0)
            {
                Console.WriteLine("Таких школ нет.");
            }
            else if (schools.Count == 1)
            {
                Console.WriteLine(schools.Keys[0]);
                Console.WriteLine($"Средний балл = {FormatScore(schools.Values[0])}");
            }
            else
            {
                Console.WriteLine(string.Join(' ', schools.Keys));
            }
        }

        private static string FormatSorted(HashSet<string> set)
        {
            if (set.Count == 0)
            {
                return "нет";
            }

            var items = new List<string>(set);
            items.Sort(StringComparer.CurrentCulture);
            return string.Join(", ", items);
        }

        private static string FormatScore(double score)
        {
            return score.ToString("0.##", CultureInfo.InvariantCulture);
        }
    }
}
