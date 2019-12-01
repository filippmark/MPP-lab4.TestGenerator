using System;
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
            string programText =
            @"using System;
            using System.Collections;
            using System.Linq;
            using System.Text;
 
            namespace HelloWorld
            {
                public class Program
                {
                    public void Main(string[] args)
                    {
                        Console.WriteLine(""Hello, World!"");
                    }

                    public void MainKeke(string[] args)
                    {
                        Console.WriteLine(""Hello, World!"");
                    }
        
                    private void Mai212n(string[] args)
                    {
                        Console.WriteLine(""Hello, World!"");
                    }            
                }
            }";
            var gen = new TestGenerator();
            /*Console.WriteLine("Hello World!");
            var gen = new TestGenerator();
            gen.GetDetailsFromSourceCode(programText);
            string code = await gen.ReadCodeFromFile(@"C:\Users\lenovo\Desktop\5 сем\СПП\mpp.lab2\Lab2\Faker\Faker.cs");
            gen.GetDetailsFromSourceCode(code);*/
        }
    }

}
