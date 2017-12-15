using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication4
{
    class Tmath<Tvar>
    {
        public delegate Tvar TMethod(Tvar numA, Tvar numB);
        public TMethod ATMethod;
        public Tvar TFun(Tvar numA, Tvar numB) => ATMethod(numA, numB);
    }

    class SimpleArithmetic
    {
        public static Int32 Addition(Int32 numA, Int32 numB) => numA + numB;

        public static Int32 Subtraction(Int32 numA, Int32 numB) => numA - numB;

        public static Int32 Multiply(Int32 numA, Int32 numB) => numA * numB;

        public static Int32 Divide(Int32 numA, Int32 numB) => numA / numB;

        public static Int32 SquareRoot(Int32 numA, Int32 numB) => (Int32)Math.Sqrt(numA * numA + numB * numB);

        static Random rnd;

        static SimpleArithmetic()
        {
            rnd = new Random((int)DateTime.Now.Ticks);
        }

        public static Int16 rndInt16 {
            private set { }
            get
            {
                return (Int16)rnd.Next(0,(int)Math.Sqrt( Int16.MaxValue ) );
            }
        }
        public static Int32 rndInt32
        {
            private set { }
            get
            {
                return (Int32)rnd.Next(0, (int)Math.Sqrt(Int32.MaxValue));
            }
        }
        public static Int64 rndInt64
        {
            private set { }
            get
            {
                return (Int64)rnd.Next(0, (int)Math.Sqrt(Int32.MaxValue))* rnd.Next(0, (int)Math.Sqrt(Int32.MaxValue));
            }
        }

        public static Int64 Addition(Int64 numA, Int64 numB)
        {
            var ObjTmathInt64 = new Tmath<Int64>();
            ObjTmathInt64.ATMethod = (TnumA, TnumB) => { return TnumA + TnumB; };
            return ObjTmathInt64.TFun(numA, numB);
        }
        

    }
}
