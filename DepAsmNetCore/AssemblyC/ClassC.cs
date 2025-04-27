namespace AssemblyC
{
    using System;

    public class ClassC
    {
#if V1
        public void InvokeV1()
        {
            Console.WriteLine("Invoked V1");
        }
#elif V2
        public void InvokeV2()
        {
            Console.WriteLine("Invoked V2");
        }
#elif V3
        public void InvokeV3()
        {
            Console.WriteLine("Invoked V3");
        }
#endif
    }

    public static class CExtension
    {
#if V1
        public static void InvokeV1Ext(this ClassC c)
        {
            Console.WriteLine("Invoked the extension method on C-V1");
        }
#endif

#if V2
        public static void InvokeV2Ext(this ClassC c)
        {
            Console.WriteLine("Invoked the extension method on C-V2");
        }
#endif
    }
}