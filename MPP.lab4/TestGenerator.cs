using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace TestGeneratorImpl
{
    public class TestGenerator
    {
        private Writer writer;
        private Reader reader;

        public TestGenerator(string dirPath)
        {
            writer = new Writer(dirPath);
            reader = new Reader();
        }

        public Task GenerateTests(List<string> files, DegreeOfParallelism degree)
        {
            var readFile = new TransformBlock<string, string>(
                new Func<string, Task<string>>(reader.ReadCodeFromFile), new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = degree.AmountOfReadParallelProcess
                });
            var generateTests = new TransformBlock<string, List<TestClassDetails>>(
                new Func<string, List<TestClassDetails>>(GetDetailsFromSourceCode), new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = degree.AmountOfGenerateParallelProcess
                });
            var writeTestsToFile = new ActionBlock<List<TestClassDetails>>(
                 tests => writer.WriteTestsToFiles(tests), new ExecutionDataflowBlockOptions
                 {
                     MaxDegreeOfParallelism = degree.AmountOfWriteParallelProcess
                 });

            var linkOptions = new DataflowLinkOptions { PropagateCompletion = true };

            readFile.LinkTo(generateTests, linkOptions);
            generateTests.LinkTo(writeTestsToFile, linkOptions);

            foreach (var file in files)
            {
                readFile.Post(file);
            }

            readFile.Complete();
            return writeTestsToFile.Completion;
        }

        public List<TestClassDetails> GetDetailsFromSourceCode(string sourceCode)
        {

            List<TestClassDetails> classesDetails = null;

            SyntaxTree tree = CSharpSyntaxTree.ParseText(sourceCode);
            CompilationUnitSyntax root = tree.GetCompilationUnitRoot();

            NamespaceDeclarationSyntax ns = root.DescendantNodes().OfType<NamespaceDeclarationSyntax>().FirstOrDefault();

            if (ns != null)
            {
                classesDetails = GetClassesInfoFromSoureCode(ns);
            }

            return classesDetails;
        }

        private List<TestClassDetails> GetClassesInfoFromSoureCode(NamespaceDeclarationSyntax ns)
        {
            List<TestClassDetails> classesDetails = new List<TestClassDetails>();
            if (ns != null)
            {
                var classes = ns.Members.OfType<ClassDeclarationSyntax>().Where(decl => !decl.Modifiers.Any(mod => mod.IsKind(SyntaxKind.AbstractKeyword)) && decl.Modifiers.Any(mod => mod.IsKind(SyntaxKind.PublicKeyword)));
                foreach (var classDecl in classes)
                {
                    List<MemberDeclarationSyntax> methodDeclarations = new List<MemberDeclarationSyntax>();
                    MemberDeclarationSyntax classDeclaration = null;
                    CompilationUnitSyntax testDeclaration = null;

                    var methods = classDecl.Members.OfType<MethodDeclarationSyntax>().Where(decl => !decl.Modifiers.Any(mod => mod.IsKind(SyntaxKind.AbstractKeyword)) && decl.Modifiers.Any(mod => mod.IsKind(SyntaxKind.PublicKeyword)));
                    foreach (var method in methods)
                    {
                        methodDeclarations.Add(DeclareMethodForTest(method.Identifier.ValueText));
                    }

                    string testClassName = classDecl.Identifier.ValueText + "UnitTest";

                    classDeclaration = DeclareClassForTest(testClassName, methodDeclarations);
                    testDeclaration = DeclareTest(CreateDirectivesForTest(ns.Name.ToString()), DeclareNameSpaceForTest(ns.Name.ToString(), classDeclaration));
                    Console.WriteLine(testDeclaration.NormalizeWhitespace().ToString());


                    classesDetails.Add(new TestClassDetails(testClassName + ".cs", testDeclaration.NormalizeWhitespace().ToString()));
                }
            }
            return classesDetails;
        }


        private List<UsingDirectiveSyntax> CreateDirectivesForTest(string nameSpace)
        {
            List<UsingDirectiveSyntax> usings = new List<UsingDirectiveSyntax>();
            usings.Add(UsingDirective(QualifiedName(IdentifierName("Microsoft.VisualStudio"), IdentifierName("TestTools.UnitTesting"))));
            usings.Add(UsingDirective(IdentifierName(nameSpace)));
            return usings;
        }

        private CompilationUnitSyntax DeclareTest(List<UsingDirectiveSyntax> usings, MemberDeclarationSyntax member)
        {
            return CompilationUnit().WithUsings(List(usings)).WithMembers(SingletonList(member));
        }

        private MemberDeclarationSyntax DeclareNameSpaceForTest(string nameSpace, MemberDeclarationSyntax member)
        {
            return NamespaceDeclaration(QualifiedName(IdentifierName(nameSpace), IdentifierName("Test"))).WithMembers(SingletonList(member));
        }

        private MemberDeclarationSyntax DeclareClassForTest(string className, List<MemberDeclarationSyntax> methods)
        {
            return ClassDeclaration(className).WithAttributeLists(SingletonList(AttributeList(SingletonSeparatedList(Attribute(IdentifierName("TestClass"))))))
                .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
                .WithMembers(List(methods));
        }

        private MemberDeclarationSyntax DeclareMethodForTest(string methodName)
        {
            return MethodDeclaration(PredefinedType(Token(SyntaxKind.VoidKeyword)), Identifier(methodName + "MethodTest")).WithAttributeLists(SingletonList(AttributeList(SingletonSeparatedList(Attribute(IdentifierName("TestMethod"))))))
                .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
                .WithBody(Block(ExpressionStatement(InvocationExpression(MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, IdentifierName("Assert"), IdentifierName("Fail")))
                .WithArgumentList(ArgumentList(SingletonSeparatedList(Argument(LiteralExpression(SyntaxKind.StringLiteralExpression, Literal("autogenerated")))))))));
        }

    }
}