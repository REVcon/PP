using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Lab4
{
    public class MyShop
    {
        private object m_firstLock = new object();
        private object m_secondLock = new object();
        private object m_thirdtLock = new object();

        public void FirstSection()
        {

            lock (m_firstLock)
            {               
                Console.WriteLine("{0} entered to first section", Thread.CurrentThread.Name);
                Thread.Sleep(5000);
                Console.WriteLine("{0} left first section", Thread.CurrentThread.Name);
            }
            
        }
        public void SecondSection()
        {
            lock (m_secondLock)
            {              
                Console.WriteLine("{0} entered to second section", Thread.CurrentThread.Name);
                Thread.Sleep(5000);
                Console.WriteLine("{0} left second section", Thread.CurrentThread.Name);
            }
        }

        public void ThirdSection()
        {

            lock(m_thirdtLock)
            {                
                Console.WriteLine("{0} entered to third section", Thread.CurrentThread.Name);               
                Thread.Sleep(5000);
                Console.WriteLine("{0} left third section", Thread.CurrentThread.Name);
            }            
        }

        public void LeaveShop()
        {            
            Console.WriteLine("{0} left shop", Thread.CurrentThread.Name);          
        }
    }

    class MyBuyer
    {
        public enum Action
        {
            GO_TO_FIRST_SECTION,
            GO_TO_SECOND_SECTION,
            GO_TO_THIRD_SECTION,
            LEAVE
        };

        private MyShop m_shop;
        private Boolean m_leaveShop;
        private Random m_randomize;
        public MyBuyer(MyShop shop)
        {
            m_shop = shop;
            m_leaveShop = false;
            m_randomize = new Random();
        }
        public void GoToShop()
        {
            Console.WriteLine("{0} entered to the shop", Thread.CurrentThread.Name);           
            Array values = Enum.GetValues(typeof(Action));           
            while (!m_leaveShop)
            {
                Action randomAction = (Action)values.GetValue(m_randomize.Next(values.Length));
                switch (randomAction)
                {
                    case Action.GO_TO_FIRST_SECTION:
                        {
                            m_shop.FirstSection();
                            break;
                        }
                    case Action.GO_TO_SECOND_SECTION:
                        {
                            m_shop.SecondSection();
                            break;
                        }
                    case Action.GO_TO_THIRD_SECTION:
                        {
                            m_shop.ThirdSection();
                            break;
                        }
                    case Action.LEAVE:
                        {
                            m_leaveShop = true;
                            m_shop.LeaveShop();
                            break;
                        }
                }
            }            
        }
    }

    class Program
    {
        private static readonly int  BUYERS_COUNT = 3;
      
        static void Main(string[] args)
        {            
            MyShop shop = new MyShop();
            List<Thread> threads = new List<Thread>();

            for (int i = 0; i < BUYERS_COUNT ; i++)
            {
                MyBuyer buyer = new MyBuyer(shop);
                Thread thread = new Thread(new ThreadStart(buyer.GoToShop));
                thread.Name = string.Format("Buyer #{0}", i);
                thread.Start();
                threads.Add(thread);
            }

            Console.ReadLine();
        }
    }
}
