using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication3
{
    class Program
    {
        static void Main(string[] args)
        {
            student breet = new student("Breet Dalton");
            student emily = new student();

            emily.setName ( "Emily VanCamp" );

            var newName = emily.getName();

            Person chris = new Person();
            chris.Title = "Chris";
            chris.FamilyName = "Evans";
            chris.Ages = 20;
            chris.Display();

            Person may = new Person("May");
            may.Display();

            Console.WriteLine($"There are {Person.Count} people.");

            Person fall = new Person { Title = "Fall", FamilyName="Simple", Ages = 25 };
            fall.Display();

            Person noone = new Person();
            noone.Display();

            Console.WriteLine($"There are {Person.Count} people.");
        }
    }
}
