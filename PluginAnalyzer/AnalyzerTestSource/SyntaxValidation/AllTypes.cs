namespace AnalyzerTestSource.SyntaxValidation
{
    using Microsoft.Xrm.Sdk;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading.Tasks;

    public class TestClass
    {
        private string _field;

        private string _fieldWithAssignment = "Some String";

        private string _fieldWithExpression = Environment.MachineName;

        private string _fieldWithExpression2 = Path.Combine(Environment.CurrentDirectory, "Test");

        private event EventHandler _theEvent;

        public string StringProperty { get; set; }

        public string PropertyWithInit { get; } = "Value";

        public string PropertyWithExp => "Value";


        public event EventHandler TheEvent2
        {
            add { _theEvent = value; }
            remove { _theEvent = value; }
        }

        public TestClass()
        {
            Console.WriteLine("This is a test");
        }

        public void Method1(string args)
        {
            Console.WriteLine("This is a test method");
        }

        [ComVisible(true)] //testing attribute
        public void Method2(string args) => Console.WriteLine("Second method");


        public class NestedType
        {
            public string NestedProperty { get; set; }

            public void NestedMethod(string args)
            {
                Console.WriteLine("This is a test nested method");
            }

            public class NestedType2
            {
                public string DeepProperty { get; set; }
            }
        }

        public struct TestStruct
        {
            public string A { get; set; }

            public string B { get; set; }   
        }
    }
}
