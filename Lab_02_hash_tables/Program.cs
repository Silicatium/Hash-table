/* Variant 26 Lab 1 */

using static System.Math;


namespace Lab01
{
    public class HashTable
    {
        private int?[] hashTable;
        private int t;
        private int countOfFilledCells = 0;
        private int countOfFillingSteps = 0;
        private int countOfRemovedElements = 0;

        public HashTable(int m)
        {
            t = (int)(m * 1.5);
            hashTable = new int?[t];
        }
        private int Function(int key)
        {
            return ((key + 7) / 41) % t;
        }
        public int Fill(int key)
        {
            //f(k) = ((k + 7) / 41) % t
            //linear sampling

            var index = Find(key);
            if (index != -1) return -2;

            if (countOfFilledCells != t)
            {
                var address_0 = Function(key);
                var address = address_0;
                var i = 1;
                while ((hashTable[address] != null) && (hashTable[address] != -1))
                {
                    address = (address_0 + i) % t;
                    i++;
                }
                countOfFilledCells++;
                hashTable[address] = key;
                countOfFillingSteps += i;
                return address;
            }
            return -1;
        }
        public void Show()
        {
            for (int i = 0; i < t; i++)
            {
                Console.WriteLine($"[{i:D2}] = " + hashTable[i]);
            }
        }
        public void Show(int digitsCount)
        {
            for (int i = 0; i < t; i++)
            {
                string? number = hashTable[i].ToString();
                if (number == "") number = string.Concat(Enumerable.Repeat(" ", digitsCount));
                else number += string.Concat(Enumerable.Repeat(" ", digitsCount - number.Length));
                Console.Write($"[{i:D2}] = " + number + "  ");
                if ((i + 1) % 5 == 0 || i == t - 1) Console.WriteLine();
            }
        }
        public int Find(int element)
        {
            var address_0 = Function(element);
            var address = address_0;
            var i = 1;
            while (hashTable[address] != element)
            {
                address = (address_0 + i) % t;
                i++;
                if (hashTable[address] == null) return -1;
                if (i > 100) { Console.WriteLine("Превышено количество итераций"); return -1; }
            }
            return address;            
        }
        public int Remove(int element)
        {
            var index = Find(element);
            if (index == -1) return -1;
            hashTable[index] = -1;
            countOfRemovedElements++;
            countOfFilledCells--;
            return index;
        }
        public double Fullness()
        {
            if (t == 0) return 0;
            return countOfFilledCells / (double)t;
        }
        public double AvgStepCount()
        {
            if (countOfFilledCells == 0) return 0;
            return countOfFillingSteps / (double)(countOfFilledCells + countOfRemovedElements);
        }
    }

    public class Program
    {
        private static List<int> GenerateNumbers(int m, int n)
        {
            var list = new List<int>();
            int element;
            var random = new Random(DateTime.Now.Microsecond);
            while (list.Count < m)
            {
                element = random.Next((int)Pow(10, n - 1), (int)Pow(10, n) - 1);
                if (!list.Contains(element)) list.Add(element);
            }

            return list;
        }
        private static HashTable PrevLab(int m, int n)
        {
            var keys = GenerateNumbers(m, n);
            foreach (int key in keys)
            {
                Console.Write(key);
                Console.Write(' ');
            }
            Console.WriteLine('\n');

            var hashTable = new HashTable(m);
            foreach (var key in keys)
            {
                hashTable.Fill(key);
            }
            hashTable.Show(n);
            Console.WriteLine();

            Console.WriteLine("Fullness = " + Round(hashTable.Fullness(), 3));
            Console.WriteLine("Average Steps Count = " + Round(hashTable.AvgStepCount(), 3));

            return hashTable;
        }
        private static void Find(HashTable hashTable)
        {
            Console.Write("\nВведите значение элемента: ");
            if (int.TryParse(Console.ReadLine(), out int element))
            {
                Console.WriteLine();
                var index = hashTable.Find(element);
                if (index == -1) Console.WriteLine("Элемет не найден");
                else Console.WriteLine($"Элемент {element} найден под индексом {index}");
            }
            else Console.WriteLine("\nВведено не число, повторите попытку");
        }
        private static void Remove(HashTable hashTable, int n)
        {
            Console.Write("\nВведите значение удаляемого элемента: ");
            if (int.TryParse(Console.ReadLine(), out int element))
            {
                Console.WriteLine();
                var index = hashTable.Remove(element);
                if (index == -1) Console.WriteLine("Элемет не найден");
                else
                {
                    Console.WriteLine($"Элемент {element} удален из индекса {index}\n"); hashTable.Show(n);
                    Console.WriteLine("\nFullness = " + Round(hashTable.Fullness(), 3));
                    Console.WriteLine("Average Steps Count = " + Round(hashTable.AvgStepCount(), 3));
                }
            }
            else Console.WriteLine("\nВведено не число, повторите попытку");
        }
        private static void Fill(HashTable hashTable, int n)
        {
            Console.WriteLine(" Выберите нужный вариант: \n  1 - Ввести с клавиатуры\n  2 - Сгенерировать случайным образом\n");
            switch (Console.ReadLine())
            {
                case "1":
                    Console.Write("\nВведите значение добавляемого элемента: ");
                    if (int.TryParse(Console.ReadLine(), out int element))
                    {
                        Console.WriteLine();
                        var index = hashTable.Fill(element);
                        if (index == -2) Console.WriteLine("Элемент уже существует в таблице");
                        else if (index == -1) Console.WriteLine("Таблица переполнена");
                        else
                        {
                            Console.WriteLine($"Элемент {element} добавлен под индексом {index}\n"); hashTable.Show(n);
                            Console.WriteLine("\nFullness = " + Round(hashTable.Fullness(), 3));
                            Console.WriteLine("Average Steps Count = " + Round(hashTable.AvgStepCount(), 3));
                        }
                    }
                    else Console.WriteLine("\nВведено не число, повторите попытку");
                    break;
                case "2":
                    var random = new Random(DateTime.Now.Microsecond);
                    element = random.Next((int)Pow(10, n - 1), (int)Pow(10, n) - 1);
                    var adress = hashTable.Fill(element);
                    if (adress == -2) Console.WriteLine("Элемент уже существует в таблице");
                    else if (adress == -1) Console.WriteLine("Таблица переполнена");
                    else
                    {
                        Console.WriteLine($"Элемент {element} добавлен под индексом {adress}\n"); hashTable.Show(n);
                        Console.WriteLine("\nFullness = " + Round(hashTable.Fullness(), 3));
                        Console.WriteLine("Average Steps Count = " + Round(hashTable.AvgStepCount(), 3));
                    }
                    break;
                default:
                    Console.WriteLine("\nВведено неверное значение, повторите попытку");
                    break;
            }
        }
        static void Replace(HashTable hashTable, int n)
        {
            Console.Write("\nВведите значение удаляемого элемента: ");
            if (int.TryParse(Console.ReadLine(), out int element))
            {
                Console.WriteLine();
                if (hashTable.Find(element) == -1) Console.WriteLine("Элемент не найден");
                else
                {
                    Console.WriteLine($"Элемент {element} удален из индекса {hashTable.Remove(element)}");
                    Console.WriteLine("\nFullness = " + Round(hashTable.Fullness(), 3));
                    Console.WriteLine("Average Steps Count = " + Round(hashTable.AvgStepCount(), 3));
                    Fill(hashTable, n);
                }
            }
            else Console.WriteLine("\nВведено не число, повторите попытку");
        }
        static void Main()
        {
            var m = 64; //Count of numbers
            var n = 6;  //Count of digits in number
            var hashTable = PrevLab(m, n);
            bool cycle = true;
            while (cycle)
            {
                Console.WriteLine("\nВыберите нужный вариант:\n 1 - Найти элемент\n 2 - Удалить элемент\n 3 - Добавить новый элемент\n 4 - Заменить элемент\n 5 - Заврешить работу программы\n");
                switch (Console.ReadLine())
                {
                    case "1":
                        Find(hashTable);
                        Console.ReadLine();
                        break;
                    case "2":
                        Remove(hashTable, n);
                        Console.ReadLine();
                        break;
                    case "3":
                        Fill(hashTable, n);
                        Console.ReadLine();
                        break;
                    case "4":
                        Replace(hashTable, n);
                        Console.ReadLine();
                        break;
                    case "5":
                        cycle = false;
                        break;
                    default:
                        Console.WriteLine("\nВведено неверное значение, повторите попытку");
                        break;
                }
            }
        }
    }
}