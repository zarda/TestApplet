using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication5
{
    class BaseClass
    {
        public virtual void Display() { }

        public void OutputLine()
        {
            Console.WriteLine($"\n---------------------------------------");
        }

    }

    class Class1 : BaseClass
    {
        protected static int[] ArrayInt { get; } = { 23, 30, 44, 50 };

        protected static List<int> ListInt { get; } =   new List<int>(ArrayInt);
        
        public virtual double result => ListInt.Average(grade => Convert.ToDouble(grade));

        public override void Display()
        {
            Console.Write($"Average = {result}");
            base.OutputLine();
        }
    }

    class Class2 : Class1
    {
        public override double result => ListInt.Sum(grade => Convert.ToDouble(grade));

        public override void Display()
        {
            Console.Write($"Summation = {result}");
            base.OutputLine();
        }
    }

        class Class3 : BaseClass
    {
        protected static ArrayList OneList = new ArrayList();

        protected static int[] ArrayInt = {  10, 20, 30, 40, 50 };
        protected static string[] ArrayStr = { "N10", "N20", "N30", "N40", "N50" };

        public Class3()
        {
            int idx = 0;
            foreach (var item in ArrayInt)
            {
                OneList.Add(item);
                OneList.Add(ArrayStr[idx]);
                idx++;
            }
        }

        public override void Display()
        {
            foreach (var item in OneList)
            {
                Console.Write($" {item}   ");
                
            }
            base.OutputLine();
        }
    }

    class Class4 : BaseClass
    {
        Dictionary<string, int> student =
            new Dictionary<string, int>()
            {
                ["one"] = 1,
                ["ten"] = 10,
                ["hundred"] = 100,
                ["twenty"] = 20
            };

        public override void Display()
        {
            Console.WriteLine($"{"word",-10} {"number",3}");

            foreach (KeyValuePair<string,int> item in student)
            {
                Console.Write($"{item.Key,-10} {item.Value,3}");
            }
            base.OutputLine();
        }
    }

    class Class5 : BaseClass
    {
        public static Queue<string> numberQue = new Queue<string>();

        public static Stack<string> numberStk = new Stack<string>();

        public Class5()
        {
            for (int i = 0; i < 5; i++)
            {
                numberQue.Enqueue("Queue" + i.ToString());

                numberStk.Push("Stack" + i.ToString());
            }
        }

        public override void Display()
        {
            foreach (var item in numberQue)
            {
                Console.Write($"{item}  ");
            }
            base.OutputLine();
            foreach (var item in numberStk)
            {
                Console.Write($"{item}  ");
            }
            base.OutputLine();
        }
    }

    class Class6 : BaseClass
    {
        protected int[] intCollection= new int[50];

        public delegate bool Spectulation(int num);

        private static bool IsEven(int num) => num % 2 == 0;

        private static bool IsOdd(int num) => num % 2 == 1;

        private static bool IsDivide3(int num) => num % 3 == 0;

        private static List<int> ArrayPercolation(int[] ArrayInt, Spectulation OneAlgorithm)
        {
            List<int> result = new List<int>();
            foreach (var item in ArrayInt)
            {
                if (OneAlgorithm(item))
                {
                    result.Add(item);
                }
            }
            return result;
        }

        public Class6()
        {
            for (int i = 0; i < 50; i++)
            {
                intCollection[i] = i+12;
            }
        }

        public void ShowList<T>(string strHead, List<T> list)
        {
            Console.WriteLine(strHead);
            foreach (var item in list)
            {
                Console.Write($"{item}  ");
            }
            base.OutputLine();
        }

        public override void Display()
        {
            Spectulation evenPredicate = new Spectulation(IsEven);
            List<int> evenList = ArrayPercolation(intCollection, evenPredicate);
            ShowList<int>("Even Number List:", evenList);

            ShowList<int>("Odd Number List:", ArrayPercolation(intCollection, IsOdd));

            ShowList<int>("Numbers can be divided by 3  :", 
                            ArrayPercolation(intCollection, IsDivide3));

            ShowList<int>("Numbers can be divided by 5  :",
                            ArrayPercolation(intCollection, (num) => (num % 5 == 0)));

            ShowList<int>("Numbers can be divided by 7 & greater than 30  :",
                            ArrayPercolation(intCollection, (num) => (num % 7 == 0) && (num > 30)));

            ShowList<int>("3N+1 > 25  :",
                            ArrayPercolation(
                                intCollection, 
                                num => ( (num-1) % 3 == 0) && (num > 25) 
                                )
                            );

        }
    }

    class Class7 : Class6
    {
        private static Random randomInt = new Random((int)DateTime.Now.Ticks);

        public int rndInt { get { return randomInt.Next(1, 9); }  }

        private List<int> ListInt = new List<int>();

        public Class7()
        {
            for (int i = 0; i < 20; i++)
            {
                ListInt.Add(rndInt);
            }
        }

        public override void Display()
        {
            ShowList<int>("List:", ListInt);
            Console.WriteLine($"Result = { ListInt.Aggregate( (current, next) => ( current > next ? current : next )  )}");
        }

    }
}
