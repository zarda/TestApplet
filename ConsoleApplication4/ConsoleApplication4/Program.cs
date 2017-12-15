using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication4
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < 2; i++)
            {
                var numA = SimpleArithmetic.rndInt32;
                var numB = SimpleArithmetic.rndInt32;
                
                Console.WriteLine($"{numA}+{numB}={SimpleArithmetic.Addition(numA,numB)}");
                Console.WriteLine($"{numA}-{numB}={SimpleArithmetic.Subtraction(numA, numB)}");
                Console.WriteLine($"{numA}*{numB}={SimpleArithmetic.Multiply(numA, numB)}");
                Console.WriteLine($"{numA}/{numB}={SimpleArithmetic.Divide(numA, numB)}");
                Console.WriteLine($"sqrt({numA}^2+{numB}^2)={SimpleArithmetic.SquareRoot(numA, numB)}");
            }

            Console.WriteLine();

            {
                var numA = SimpleArithmetic.rndInt16;
                var numB = SimpleArithmetic.rndInt16;

                var ObjTmath = new Tmath<Int32>();

                ObjTmath.ATMethod = (TnumA, TnumB) => { return TnumA + TnumB; };
                Console.WriteLine($"{numA}+{numB}={ObjTmath.TFun(numA, numB)}");

                ObjTmath.ATMethod = (TnumA, TnumB) => { return TnumA - TnumB; };
                Console.WriteLine($"{numA}-{numB}={ObjTmath.TFun(numA, numB)}");

             }
        }
    }
}
