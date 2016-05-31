using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lab5
{
    class SectionClient
    {
        private const string PipeName = "lab5_pipe";
        private int id;
        private int __extId;

        public SectionClient()
        {
            id = 0;
            __extId = 0;
        }

        private NamedPipeClientStream CreatePipeClientStream()
        {
            return new NamedPipeClientStream(
                ".",
                PipeName,
                PipeDirection.InOut,
                PipeOptions.Asynchronous
            );
        }

        public void Start()
        {
            Console.WriteLine("Отдел создан. Pipe: {0}.", PipeName);            
            var initResp = SendCommand(Request.NewVendor);
            while (initResp != Response.OK)
            {
                Console.WriteLine("Продавец пришел на работу");                
                initResp = SendCommand(Request.NewVendor);
            }
            Console.WriteLine("Отдел открыт.");
            Console.WriteLine("Незанят");
            Thread.Sleep(1000);    
            while (true)
            {
                
                try
                {
                    var response = SendCommand(Request.IsVendorVacant);
                    Console.WriteLine("Продавец {0}", id);
                    while (response != Response.OK)
                    {
                        Console.WriteLine("Обслуживаю клиента {0}", __extId);
                        Thread.Sleep(1000);
                        response = SendCommand(Request.IsVendorVacant);
                        Console.WriteLine("Продавец {0}", id);
                    }
                    Console.WriteLine("Незанят");
                    Thread.Sleep(1000);                    
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Thread.Sleep(1000);
                }
            }
        }

        private void TryConnect(NamedPipeClientStream pipeStream)
        {
            try
            {
                pipeStream.Connect();
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Не могу найти магазин");
                Thread.Sleep(1000);
                TryConnect(pipeStream);
            }
        }

        private Response SendCommand(Request command)
        {
            var pipeStream = CreatePipeClientStream();
            TryConnect(pipeStream);
            pipeStream.WriteByte((byte)id);
            pipeStream.WriteByte((byte)command);
            pipeStream.WaitForPipeDrain();
            id = pipeStream.ReadByte();           
            var response = (Response)pipeStream.ReadByte();
            __extId = pipeStream.ReadByte();
            pipeStream.WaitForPipeDrain();
            return response;
        }
    }
}
