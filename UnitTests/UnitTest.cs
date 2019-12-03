using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestGeneratorImpl;

namespace UnitTests
{
    [TestClass]
    public class UnitTest
    {
        private string directory;
        private TestGenerator generator;

        [TestInitialize]
        public void StartBeforeEach()
        {
            List<string> files = new List<string>(Directory.GetFiles(@"C:\Users\lenovo\source\repos\MPP.lab4\ClassesForGeneratingTests"));
            directory = Directory.GetCurrentDirectory() + @"\Tests" + DateTime.Now.Ticks.ToString();
            generator = new TestGenerator(directory);
            generator.GenerateTests(files, new DegreeOfParallelism(3, 3, 3)).Wait();
        }


        [TestMethod]
        public void ShouldBe14GeneratedTests()
        {
            List<string> files = new List<string>(Directory.GetFiles(directory));
            Assert.AreEqual(14, files.Count);
        }

        [TestMethod]
        public void ShouldHave3GeneratedMethods()
        {
            int amountOfMethods = CountGeneratedMethods(directory + @"\BlaslslUnitTest.cs");
            Assert.AreEqual(3, amountOfMethods);
        }

        [TestMethod]
        public void ShouldHave5GeneratedMethods()
        {
            int amountOfMethods = CountGeneratedMethods(directory + @"\ListGeneratorUnitTest.cs");
            Assert.AreEqual(5, amountOfMethods);
        }

        [TestMethod]
        public void ShouldHaveCorrectNameBlaslslUnitTest()
        {
            string className = GetNameOfClass(directory + @"\BlaslslUnitTest.cs");
            Assert.AreEqual("BlaslslUnitTest", className);
        }

        [TestMethod]
        public void ShouldHaveCorrectNameBooleanGeneratorUnitTest()
        {
            string className = GetNameOfClass(directory + @"\BooleanGeneratorUnitTest.cs");
            Assert.AreEqual("BooleanGeneratorUnitTest", className);
        }

        public ClassDeclarationSyntax GetClassDeclaration(string pathToFile)
        {
            Console.WriteLine(pathToFile);
            string code = File.ReadAllText(pathToFile, Encoding.UTF8);
            Console.WriteLine(code);
            SyntaxTree tree = CSharpSyntaxTree.ParseText(code);
            CompilationUnitSyntax root = tree.GetCompilationUnitRoot();

            NamespaceDeclarationSyntax ns = root.DescendantNodes().OfType<NamespaceDeclarationSyntax>().FirstOrDefault();
            var classes = ns.Members.OfType<ClassDeclarationSyntax>().Where(decl => !decl.Modifiers.Any(mod => mod.IsKind(SyntaxKind.AbstractKeyword)) && decl.Modifiers.Any(mod => mod.IsKind(SyntaxKind.PublicKeyword)));
            return classes.First();
        }

        public int CountGeneratedMethods(string pathToFile)
        {
            var methods = GetClassDeclaration(pathToFile).Members.OfType<MethodDeclarationSyntax>().Where(decl => !decl.Modifiers.Any(mod => mod.IsKind(SyntaxKind.AbstractKeyword)) && decl.Modifiers.Any(mod => mod.IsKind(SyntaxKind.PublicKeyword)));
            return methods.Count();
        }

        public string GetNameOfClass(string pathToFile)
        {
            return GetClassDeclaration(pathToFile).Identifier.ValueText.ToString();
        }
    }
}
