using System;

namespace AnotherHello
{
    public class HelloWorld
    {
        public void Hello()
        {
            Console.WriteLine("Hello World!");
        }
        public void WaitLastLine()
        {
            Console.Write("Press any key to continue...");
            Console.ReadKey(true);
        }
    }
}
