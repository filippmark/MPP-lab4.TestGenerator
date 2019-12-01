using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestGeneratorImpl;

namespace UnitTests
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            TestGenerator g = new TestGenerator();
            Assert.IsFalse(false);
        }
    }
}
