using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Collections
{
    public static class CollectionTasks
    {
        private const int MinSchool = 1;
        private const int MaxSchool = 99;
        private const int MinScore = 1;
        private const int MaxScore = 100;
        private const int MinStudentCount = 5;

        private static readonly string[] s_surnames;
        private static readonly string[] s_names;
        private static readonly int[] s_schools;

        static CollectionTasks()
        {
            s_surnames = new string[] { "Иванов", "Петров", "Сидоров", "Козлов", "Смирнов", "Попов", "Волков" };
            s_names = new string[] { "Сергей", "Иван", "Анна", "Мария", "Олег", "Павел", "Ольга" };
            s_schools = new int[] { 18, 23, 50, 87 };
        }

        public static int CountDistinct(List<int> list)
        {
            ArgumentNullException.ThrowIfNull(list);

            var count = 0;
            for (var index = 0; index < list.Count; index++)
            {
                if (list.IndexOf(list[index]) == index)
                {
                    count++;
                }
            }

            return count;
        }

        public static int ReplaceUnequalNeighbours(LinkedList<int> list, int e, int f)
        {
            ArgumentNullException.ThrowIfNull(list);

            var neighbours = new List<LinkedListNode<int>>();
            for (var node = list.First; node != null; node = node.Next)
            {
                if (node.Value != e || node.Previous == null || node.Next == null)
                {
                    continue;
                }

                if (node.Previous.Value != node.Next.Value)
                {
                    neighbours.Add(node.Previous);
                    neighbours.Add(node.Next);
                }
            }

            foreach (var neighbour in neighbours)
            {
                neighbour.Value = f;
            }

            return neighbours.Count / 2;
        }

        public static HashSet<string> FindGrownEverywhere(HashSet<string>[] cooperatives)
        {
            ArgumentNullException.ThrowIfNull(cooperatives);
            if (cooperatives.Length == 0)
            {
                return new HashSet<string>();
            }

            var result = new HashSet<string>(cooperatives[0]);
            for (var index = 1; index < cooperatives.Length; index++)
            {
                result.IntersectWith(cooperatives[index]);
            }

            return result;
        }

        public static HashSet<string> FindGrownSomewhere(HashSet<string>[] cooperatives)
        {
            ArgumentNullException.ThrowIfNull(cooperatives);

            var grownAnywhere = new HashSet<string>();
            foreach (var cooperative in cooperatives)
            {
                grownAnywhere.UnionWith(cooperative);
            }

            grownAnywhere.ExceptWith(FindGrownEverywhere(cooperatives));
            return grownAnywhere;
        }

        public static HashSet<string> FindGrownInExactlyOne(HashSet<string>[] cooperatives)
        {
            ArgumentNullException.ThrowIfNull(cooperatives);

            var grownOnce = new HashSet<string>();
            var grownTwiceOrMore = new HashSet<string>();
            var repeated = new HashSet<string>();
            foreach (var cooperative in cooperatives)
            {
                repeated.Clear();
                repeated.UnionWith(cooperative);
                repeated.IntersectWith(grownOnce);
                grownTwiceOrMore.UnionWith(repeated);
                grownOnce.UnionWith(cooperative);
            }

            grownOnce.ExceptWith(grownTwiceOrMore);
            return grownOnce;
        }

        public static HashSet<char> FindCharsInEveryWord(string text)
        {
            ArgumentNullException.ThrowIfNull(text);

            var words = SplitIntoWords(text);
            if (words.Count == 0)
            {
                return new HashSet<char>();
            }

            var result = new HashSet<char>(words[0]);
            for (var index = 1; index < words.Count; index++)
            {
                result.IntersectWith(words[index]);
            }

            return result;
        }

        public static void FillStudentsFile(string path, int count, Random random)
        {
            ArgumentNullException.ThrowIfNull(random);
            ArgumentOutOfRangeException.ThrowIfLessThan(count, MinStudentCount);

            using var writer = new StreamWriter(path);
            writer.WriteLine(count.ToString(CultureInfo.InvariantCulture));
            string surname;
            string name;
            int school;
            int score;
            for (var index = 0; index < count; index++)
            {
                surname = s_surnames[random.Next(s_surnames.Length)];
                name = s_names[random.Next(s_names.Length)];
                school = s_schools[random.Next(s_schools.Length)];
                score = random.Next(MinScore, MaxScore + 1);
                writer.WriteLine(
                    string.Format(CultureInfo.InvariantCulture, "{0} {1} {2} {3}", surname, name, school, score));
            }
        }

        public static SortedList<int, double> FindSchoolsAboveDistrictAverage(string path, out double districtAverage)
        {
            var lines = File.ReadAllLines(path);
            if (lines.Length == 0)
            {
                throw new InvalidDataException("Файл пустой.");
            }

            var count = ParseInt(lines[0], 1, MinStudentCount, lines.Length - 1);

            var sums = new SortedList<int, int>();
            var counts = new SortedList<int, int>();
            var totalScore = 0;
            string[] parts;
            int school;
            int score;
            for (var index = 1; index <= count; index++)
            {
                parts = lines[index].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length != 4)
                {
                    throw new InvalidDataException($"Строка {index + 1}: нужен формат «Фамилия Имя Школа Балл».");
                }

                school = ParseInt(parts[2], index + 1, MinSchool, MaxSchool);
                score = ParseInt(parts[3], index + 1, MinScore, MaxScore);
                if (!sums.ContainsKey(school))
                {
                    sums.Add(school, 0);
                    counts.Add(school, 0);
                }

                sums[school] += score;
                counts[school]++;
                totalScore += score;
            }

            districtAverage = (double)totalScore / count;
            var result = new SortedList<int, double>();
            double average;
            foreach (var schoolNumber in sums.Keys)
            {
                average = (double)sums[schoolNumber] / counts[schoolNumber];
                if (average > districtAverage)
                {
                    result.Add(schoolNumber, average);
                }
            }

            return result;
        }

        private static List<HashSet<char>> SplitIntoWords(string text)
        {
            var words = new List<HashSet<char>>();
            var word = new StringBuilder();
            foreach (var symbol in text + " ")
            {
                if (char.IsLetterOrDigit(symbol) || symbol == '-')
                {
                    word.Append(char.ToLower(symbol, CultureInfo.InvariantCulture));
                    continue;
                }

                if (word.Length > 0)
                {
                    words.Add(new HashSet<char>(word.ToString()));
                    word.Clear();
                }
            }

            return words;
        }

        private static int ParseInt(string text, int lineNumber, int minValue, int maxValue)
        {
            var isNumber = int.TryParse(text, CultureInfo.InvariantCulture, out var value);
            if (!isNumber || value < minValue || value > maxValue)
            {
                throw new InvalidDataException($"Строка {lineNumber}: нужно целое число от {minValue} до {maxValue}.");
            }

            return value;
        }
    }
}
