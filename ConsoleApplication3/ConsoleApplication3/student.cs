using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication3
{
    class Person
    {
        public static int Count { get; private set; }
        
        private string name = "No name";
        public string Title
        {
            get { return name; }
            set { name = value; }
        } 
        public string FamilyName { set; get; } = "Family";
        public int Ages { set; get; } = 0;

        public void Display()
        {
            Console.WriteLine($"Hello {Title} {FamilyName}. Ages: {Ages}");
        }
        static Person() { Count = 0; }
        public Person() { Count++; }
        public Person(string title)
        {
            Title = title;
            Count++;
        }
        public Person(string title, string familyName)
        {
            Title = title;
            FamilyName = familyName;
            Count++;
        }
        public Person(string title, string familyName, int ages)
        {
            Title = title;
            FamilyName = familyName;
            Ages = ages;
            Count++;
        }
        ~Person()
        {
            Console.WriteLine($"{Title}'s memory be freed.");
            Count--;
        }
    }

    class student
    {
        private string name;

        public student(string Name)
        {
            name = Name;
        }

        public student() { }

        public void setName(string Name)
        {
            name = Name;
        }
         public string getName()
        {
            return name;
        }
    }
}
