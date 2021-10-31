using System;
using System.Collections.Generic;

namespace Epam.SoftwearDevelopment.Task1
{
    public class Program
    {
        public static void Main()
        {
            CustomDictionary<int, int> list = new CustomDictionary<int, int>();
            list.Add(new KeyValuePair<int, int>(1, 1));
            list.Add(new KeyValuePair<int, int>(2, 1));
            list.Add(new KeyValuePair<int, int>(1, 3));
            list.Add(new KeyValuePair<int, int>(1, 4));
            list.Add(new KeyValuePair<int, int>(3, 4));
            list.Add(new KeyValuePair<int, int>(4, 4));
            list.Add(new KeyValuePair<int, int>(5, 4));
            Console.WriteLine(list.Count);
            list.Remove(new KeyValuePair<int, int>(5, 4));
            var flag = list.Contains(new KeyValuePair<int, int>(5, 8));
            Console.WriteLine(list.Count);
            Console.WriteLine(flag);
        }
    }
}
