using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab5
{
    class Program
    {
        static void ArgumentsError()
        {
            Console.WriteLine("Используйте:");
            Console.WriteLine("Lab5.exe shop");
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
