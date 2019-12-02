using System;
using System.Collections.Generic;
using System.IO;
using TestGeneratorImpl;

namespace ExampleOfUse
{
    public class Program
    {
        public static void Main(string[] args)
        {
            TestUse();
        }

        static async void TestUse()
        {
            List<string> files = new List<string>(Directory.GetFiles(@"C:\Users\lenovo\source\repos\MPP-lab2.Faker1\Generators"));
            var gen = new TestGenerator("testsdirpath");
            gen.GenerateTests(files).Wait();
            Console.WriteLine("Finish...");
            Console.ReadKey();
        }
    }

}
