using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication5
{
    class Program
    {
        static void Main(string[] args)
        {
            BaseClass[] ClassCollection = new BaseClass[]
                {
                    new Class1(),
                    new Class2(),
                    new Class3(),
                    new Class4(),
                    new Class5(),
                    new Class6(),
                    new Class7()
                };
            

            foreach (var item in ClassCollection)
            {
                item.Display(); 
            }
        }
    }
}
