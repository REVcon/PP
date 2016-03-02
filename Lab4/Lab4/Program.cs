using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Lab4
{ 
    class Program
    {
        private static readonly int  BUYERS_COUNT = 3;
        private static readonly int SECTIONS_COUNT = 5;
      
        static void Main(string[] args)
        {            
            List<Section> sections = new List<Section>();
            for (int i = 0; i < SECTIONS_COUNT; i++)
            {
                Section section = new Section(i);
                sections.Add(section);
            }
            Console.WriteLine("Shop was opened, {0} sections", sections.Count);

            for (int i = 0; i < BUYERS_COUNT ; i++)
            {
                MyBuyer buyer = new MyBuyer(i, sections);               
            }

            Console.ReadLine();
        }
    }
}
