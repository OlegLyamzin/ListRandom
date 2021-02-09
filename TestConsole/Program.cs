using DataStrucrture;
using System;
using System.IO;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var random = new Random();
            var list = new ListRandom();

            var list1 = new ListRandom();
            for (int i = 0; i < 20; i++)
            {
                list.Add("string " + random.Next());
            }

            Console.WriteLine(list.ToJSON());

            using (FileStream fs = File.Open("list.txt", FileMode.Create))
            {
                list.Serialize(fs);
            }


            using (FileStream fs = File.Open("list.txt", FileMode.Open))
            {
                list1.Deserialize(fs);
            }

            Console.WriteLine(list1.ToJSON());
        }
    }
}
