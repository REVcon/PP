using System;
using System.IO;
using System.IO.Pipes;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Runtime.Remoting.Contexts;

namespace Lab5
{   
    class BuyerClient
    {
        private const string PipeName = "lab5_pipe";
        private int __id;
        private int __extID;

        public BuyerClient()
        {
            __id = 0;
            __extID = 0;
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
            Console.WriteLine("Покупатель создан. Pipe: {0}.", PipeName);
            var initResp = SendCommand(Request.NewBuyer);
            while (initResp != Response.OK)
            {                
                Thread.Sleep(1000);
                initResp = SendCommand(Request.NewBuyer);
            }
            Console.WriteLine("Покупатель выбирает отдел");           
            while (true)
            {
                try
                {                   
                    var response = SendCommand(Request.GoSection);
                    Console.WriteLine("Покупатель {0}", __id);
                    switch (response)
                    {
                        case Response.OK:                        
                            Console.WriteLine("Зашел в отдел {0}", __extID);
                            Thread.Sleep(2000);
                            Console.WriteLine("Покинул отдел {0}", __extID);
                            SendCommand(Request.LeftSection);
                            Thread.Sleep(2000);
                            break;
                        case Response.NO:
                            Console.WriteLine("В очереди в отделе {0}...", __extID);
                            while (response != Response.OK)
                            {
                                Thread.Sleep(1000);
                                Console.WriteLine("В очереди в отделе {0}...", __extID);
                                response = SendCommand(Request.GoSection);
                            }
                            Console.WriteLine("Зашел в отдел {0}", __extID);
                            Thread.Sleep(2000);
                            Console.WriteLine("Покинул отдел {0}", __extID);
                            SendCommand(Request.LeftSection);
                            Thread.Sleep(2000);
                            break;
                        case Response.WithoutVendor:
                            Console.WriteLine("В магазине нет продавцов");
                            Thread.Sleep(2000);
                            break;
                        default:
                            Console.WriteLine("Неизвестная команда");
                            Thread.Sleep(1000);
                            break;
                    }
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
                Console.WriteLine("Не могу найти отдел");
                Thread.Sleep(1000);                
                TryConnect(pipeStream);
            }
        }

        private Response SendCommand(Request request)
        {
            var pipeStream = CreatePipeClientStream();           
            TryConnect(pipeStream);            
            pipeStream.WriteByte((byte)__id);
            pipeStream.WriteByte((byte)request);
            pipeStream.WaitForPipeDrain();          
            __id = (int)pipeStream.ReadByte();           
            var response = (Response)pipeStream.ReadByte();           
            __extID = (int)pipeStream.ReadByte();            
            pipeStream.WaitForPipeDrain();           
            return response;
        }
    }
}
