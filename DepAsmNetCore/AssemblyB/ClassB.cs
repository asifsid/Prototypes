namespace AssemblyB
{
    using AssemblyC;
    using System;

    public class ClassB
    {
        public void InvokeB()
        {
            Console.WriteLine("B Invoking V2 of C ...");
            var C = new ClassC();
            
            C.InvokeV2();
            C.InvokeV2Ext();
        }
    }
}
