using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp
{
    internal class Student : Person
    {
        public Student(int Id, string Name, int Age) : base(Id, Name, Age)
        {
        }
        public void Linq()
        {
            // Sample list of numbers
            List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            // LINQ query to select even numbers
            var evenNumbers = from num in numbers
                              where num % 2 == 0
                              select num;

            // Display the result
            Console.WriteLine("Even Numbers:");
            foreach (var num in evenNumbers)
            {
                Console.WriteLine(num);
            }
        }
    }
}
