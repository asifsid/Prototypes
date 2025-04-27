namespace AssemblyA
{
    using AssemblyC;
    using System;

    public class ClassA
    {
        public void InvokeA()
        {
            Console.WriteLine("A invoking V1 of C ...");
            var C = new ClassC();

            C.InvokeV1();
            C.InvokeV1Ext();
        }
    }
}
