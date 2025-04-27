namespace ThisIsTest
{
    using System;
    
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                Console.WriteLine("Hello World. " + new FooBar().Name);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }

    class FooBar
    {
        public string Name { get; } = "This is a test";
    }
}