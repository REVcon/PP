using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Lab4
{
    class Section
    {
        private object m_lock = new object();
        private string m_name;

        public Section(int i)
        {
            m_name = string.Format("section {0}", i);
        }
        public void Activate()
        {
            Console.WriteLine("{0} wait at queue {1}", Thread.CurrentThread.Name, m_name);
            lock (m_lock)
            {
                Console.WriteLine("{0} entered to {1}", Thread.CurrentThread.Name, m_name);
                Thread.Sleep(5000);
                Console.WriteLine("{0} left {1}", Thread.CurrentThread.Name, m_name);
            }      
        }

    }
}
