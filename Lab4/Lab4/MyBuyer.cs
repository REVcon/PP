using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Lab4
{
    class MyBuyer
    {
        private Boolean m_leaveShop;
        private Random m_randomize;
        private Thread m_thread;
        private List<Section> m_sections;
        public MyBuyer(int i, List<Section> sections)
        {
            m_leaveShop = false;
            m_sections = sections;
            m_randomize = new Random(i);
            m_thread = new Thread(new ThreadStart(this.Activate));
            m_thread.Name = string.Format("Buyer {0}", i);
            m_thread.Start();
        }
        public void Activate()
        {
            Console.WriteLine("{0} entered to the shop", Thread.CurrentThread.Name);
            while (!m_leaveShop)
            {
                int randomAction = m_randomize.Next(m_sections.Count + 1);
                if (randomAction < m_sections.Count)
                {
                    m_sections[randomAction].Activate();
                }
                else
                {
                    m_leaveShop = true;
                    Console.WriteLine("{0} left the shop", Thread.CurrentThread.Name);
                }                
            }
        }
    }
}
