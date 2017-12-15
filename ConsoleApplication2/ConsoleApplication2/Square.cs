using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
    class Square
    {
        public void square()
        {
            const float Square = 3.0579f;
            float area;
            System.Console.Write("Please keyin area number : ");
            area = float.Parse(System.Console.ReadLine());
            System.Console.WriteLine($"{area} = {Square * area} m^2");
        }
        
    }
}
