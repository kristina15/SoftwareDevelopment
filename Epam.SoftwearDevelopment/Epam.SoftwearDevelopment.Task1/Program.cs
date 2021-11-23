using System;
using System.Collections.Generic;

namespace Epam.SoftwearDevelopment.Task1
{
    public class Program
    {
        public static void Main()
        {
            CustomDictionary<int, int> list = new CustomDictionary<int, int>();
            list.Add(new KeyValuePair<int, int>(9, 10));
            list.Add(new KeyValuePair<int, int>(1, 1));
            list.Add(new KeyValuePair<int, int>(2, 1));
            list.Add(new KeyValuePair<int, int>(7, 4));
            list.Add(new KeyValuePair<int, int>(3, 4));
            list.Add(new KeyValuePair<int, int>(4, 4));
            list.Add(new KeyValuePair<int, int>(5, 4));
            Console.WriteLine("Count at first " + list.Count);
            list.Remove(new KeyValuePair<int, int>(5, 4));
            Console.WriteLine("Count after delete " + list.Count);
            var flag = list.Contains(new KeyValuePair<int, int>(5, 8));
            var flag2 = list.Contains(new KeyValuePair<int, int>(1, 1));
            Console.WriteLine(flag);
            Console.WriteLine(flag2);
            foreach (var item in list)
            {
                Console.WriteLine($"{item.Key} {item.Value}");
            }

            Console.Write("Keys: ");
            foreach (var item in list.Keys)
            {
                Console.Write($"{item} ");
            }

            Console.WriteLine();
            Console.Write("Values: ");
            foreach (var item in list.Values)
            {
                Console.Write($"{item} ");
            }

            Console.WriteLine();
            Console.WriteLine(list.TryGetValue(10, out int value));
            
            CustomDictionary<string, int> otherList = new CustomDictionary<string, int>();
            otherList.Add(new KeyValuePair<string, int>("hello", 10));
            otherList.Add(new KeyValuePair<string, int>("hello1", 1));
            otherList.Add(new KeyValuePair<string, int>("hello2", 1));
            otherList.Add(new KeyValuePair<string, int>("hello3", 4));
            otherList.Add(new KeyValuePair<string, int>("hello4", 4));
            otherList.Add(new KeyValuePair<string, int>("hello5", 4));
            otherList.Add(new KeyValuePair<string, int>("hello6", 4));
            Console.WriteLine("Count at first " + otherList.Count);
            otherList.Remove(new KeyValuePair<string, int>("hello6", 4));
            Console.WriteLine("Count after delete " + otherList.Count);
            flag = otherList.Contains(new KeyValuePair<string, int>("bye", 8));
            flag2 = otherList.Contains(new KeyValuePair<string, int>("hello1", 1));
            Console.WriteLine(flag);
            Console.WriteLine(flag2);
            foreach (var item in otherList)
            {
                Console.WriteLine($"{item.Key} {item.Value}");
            }
        }
    }
}
