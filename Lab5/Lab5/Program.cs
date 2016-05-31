using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab5
{
    class Program
    {
        //private static readonly int BUYERS_COUNT = 3;
        //private static readonly int SECTIONS_COUNT = 5;

        //static void Main(string[] args)
        //{
        //    List<Section> sections = new List<Section>();
        //    for (int i = 0; i < SECTIONS_COUNT; i++)
        //    {
        //        Section section = new Section(i);
        //        sections.Add(section);
        //    }
        //    Console.WriteLine("Shop was opened, {0} sections", sections.Count);

        //    for (int i = 0; i < BUYERS_COUNT; i++)
        //    {
        //        CBuyer buyer = new CBuyer(i, sections);
        //    }

        //    Console.ReadLine();
        //}
        static void ArgumentsError()
        {
            Console.WriteLine("Используйте:");
            Console.WriteLine("Lab5.exe shop <отделов в магазине>");
            Console.WriteLine("Lab5.exe section");
            Console.WriteLine("Lab5.exe buyer");
            Environment.Exit(1);
        }

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                ArgumentsError();
            }
            switch (args[0])
            {
                case "shop":                   
                    var hiveServer = new ShopServer();
                    hiveServer.Start();
                    break;
                case "section":                   
                    var sectionClient = new SectionClient();
                    sectionClient.Start();
                    break;
                case "buyer":                    
                    var buyerClient = new BuyerClient();
                    buyerClient.Start();
                    break;
            }
        }
    }
}
