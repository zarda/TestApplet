using System;

namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            Initialization();
            Run();
        }

        private static AnotherHello.HelloWorld helloWorld;
        private static void Initialization()
        {
            helloWorld = new AnotherHello.HelloWorld();
        }

        private static void Run()
        {
            //helloWorld.Hello();

            Enumerator.ClassAsync.ConsumeNumbersAsync();
            Enumerator.ClassNormal.ConsumeNumbers();

            helloWorld.WaitLastLine();
        }
    }
}
