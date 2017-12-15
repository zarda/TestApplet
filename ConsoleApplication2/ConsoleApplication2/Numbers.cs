using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
    class Numbers
    {
        static void Main(string[] args)
        {
            add addClass = new add();
            addClass.prime();
            
            if (true)
            {
                Square squareClass = new Square();
                //squareClass.square(); 
            }

            System.Console.Write($"Please input any key to continue...");
            System.Console.ReadKey();
        }
    }
}
