using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        enum Season
        {
            Winter = 0,
            Spring = 1,
            Summer = 2,
            Autumn = 3,
        }

        static void Main(string[] args)
        {
            int[] nArr = new int[10];
            for (var i = 0; i < 10; i++)
            {
                nArr[i]++;
            }
            foreach(var item in nArr)
            {

            }
            for(;;)
            {
                break;
            }
            //======================================================
            List<Season> SeasonCollector = new List<Season>();
            for (var i = 0; i < 5; i++)
            {
                SeasonCollector.Add((Season)i);
            }
            foreach (var item in SeasonCollector)
            {
                System.Console.WriteLine(item);
                System.Console.WriteLine();
            }
            Season SeasonOne = new Season();
            SeasonOne = Season.Winter;
            SeasonOne++;
            System.Console.WriteLine(SeasonOne);
            System.Console.WriteLine();
            //======================================================
            var idx = 0;
            while (idx<10)
            {
                ++idx;
            }
            System.Console.WriteLine(idx);
            System.Console.WriteLine();
            //======================================================
            try
            {

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                System.Console.WriteLine("Checked.");
                System.Console.WriteLine();
            }
            //======================================================
            List<int> collection = new List<int>();
            Random rnd = new Random();
            for (var i = 0; i < 30; i++)
            {
                if (i>9)
                {
                    collection.Add( rnd.Next(0, 9) ); 
                }
                else
                {
                    collection.Add(i);
                }
            }
            collection.Sort();
            collection.Reverse();
            var index = 0;
            foreach (var item in collection)
            {
                if (index%2==0 && item<5)
                {
                    System.Console.Write(item + " "); 
                }
                index++;
            }
            System.Console.WriteLine();
            System.Console.WriteLine();
            //======================================================
            switch (true)
            {
                case true:
                    break;

                default:
                    break;
            }
            //======================================================
            if (true)
            {

            }
            else
            {

            }
            //======================================================
            while (true)
            {
                break;
            }

            do
            {
                break; 
            } while (true);
            //======================================================
            System.Decimal numberM01 = 0.1m;
            System.Console.WriteLine(numberM01);
            System.Console.WriteLine();


        }
    }
}
