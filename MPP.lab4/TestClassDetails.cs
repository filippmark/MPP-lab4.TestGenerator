using System;
using System.Collections.Generic;
using System.Text;

namespace TestGeneratorImpl
{
    public class TestClassDetails
    {
        public string TestClassName { get; set; }
        public string Code { get; set; }

        public TestClassDetails(string testClassName, string code)
        {
            this.TestClassName = testClassName;
            this.Code = code;
        }
    }
}
