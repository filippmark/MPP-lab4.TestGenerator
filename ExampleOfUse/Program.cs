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
            List<string> files = new List<string>(Directory.GetFiles(@"C:\Users\lenovo\source\repos\MPP.lab4\ClassesForGeneratingTests"));
            string dir = Directory.GetCurrentDirectory() + @"\Tests" + DateTime.Now.Ticks.ToString();
            var gen = new TestGenerator(dir);
            gen.GenerateTests(files, new DegreeOfParallelism(3, 3, 3)).Wait();
        }

    }

}
