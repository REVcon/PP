using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Pipes;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.IO.MemoryMappedFiles;

namespace Lab5
{
    enum Request
    {
        GoSection = 1,
        LeftSection = 2,
        IsVendorVacant = 3,
        NewVendor = 4,
        NewBuyer = 5,
    }

    enum Response
    {
        Service = 0,
        Queue = 1,
        OK = 2,
        NO = 3,
        WithoutVendor = 4
    }

    class ShopServer
    {
        private const string PipeName = "lab5_pipe";

        private List<int> __vendorsId;
        private Hashtable __buyers;
        private Hashtable __vendors;
        private Hashtable __buyerByVendor;
        private NamedPipeServerStream pipeServer;
        int id;


        public ShopServer()
        {
            __vendorsId = new List<int>();
            __buyers = new Hashtable();
            __vendors = new Hashtable();
            __buyerByVendor = new Hashtable();
            id = 0;
        }
        
        private static NamedPipeServerStream CreatePipeServer()
        {
            return new NamedPipeServerStream(
                PipeName,
                PipeDirection.InOut,
                NamedPipeServerStream.MaxAllowedServerInstances,
                PipeTransmissionMode.Message,
                PipeOptions.Asynchronous
            );
        }

        public void Start()
        {
            Console.WriteLine("Магазин создан. Pipe: {0}.", PipeName);
           
            while (true)
            {
                pipeServer = CreatePipeServer();
                pipeServer.WaitForConnection();                
                try
                {
                    int userId = pipeServer.ReadByte();
                    var command = (Request)pipeServer.ReadByte();
                    if (userId == 0)
                    {
                        id += 1;
                        pipeServer.WriteByte((byte)id);
                        userId = id;
                    }
                    else
                    {
                        pipeServer.WriteByte((byte)userId);
                    }
                    
                    switch (command)
                    {
                        case Request.GoSection:
                            HandleGoSection(userId);
                            break;
                        case Request.LeftSection:
                            HandleLeftSection(userId);
                            break;
                        case Request.IsVendorVacant:
                            HandleIsVendorVacant(userId);
                            break;
                        case Request.NewVendor:
                            HandleNewVendor(userId);
                            break;
                        case Request.NewBuyer:
                            HandleNewBuyer(userId);
                            break;
                        default:
                            Console.WriteLine("Неизвестная команда");
                            break;
                    }
                    pipeServer.WaitForPipeDrain();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        private void HandleGoSection(int userId)
        {
            var random = new Random();
            if ((int)__buyers[userId] == 0)
            {
                if (__vendorsId.Count != 0)
                {
                    int vendor = random.Next(0, __vendorsId.Count);
                    int vendorID = __vendorsId[vendor];
                    __buyers[userId] = vendorID;
                    if ((bool)__vendors[vendorID])
                    {
                        __buyerByVendor[vendorID] = userId;
                        __vendors[vendorID] = false;                       
                        Console.WriteLine("Продавец {0} обслуживает покупателя {1}", vendorID, userId);
                        pipeServer.WriteByte((byte)Response.OK);
                        pipeServer.WriteByte((byte)vendorID);
                        return;
                    }
                }
                else
                {
                    pipeServer.WriteByte((byte)Response.WithoutVendor);
                    pipeServer.WriteByte((byte)0);
                    return;
                }
            }
            else
            {
                int vendorId = (int)__buyers[userId];
                if ((bool)__vendors[vendorId])
                {
                    __buyerByVendor[vendorId] = userId;
                    __vendors[vendorId] = false;                    
                    Console.WriteLine("Продавец {0} обслуживает покупателя {1}", vendorId, userId);
                    pipeServer.WriteByte((byte)Response.OK);
                    pipeServer.WriteByte((byte)vendorId);
                    return;
                }
            }                   
            pipeServer.WriteByte((byte)Response.NO);
            int some = (int)__buyers[userId];
            pipeServer.WriteByte((byte)some);
            return;
        }

        private void HandleLeftSection(int userId)
        {
            int vendor = (int)__buyers[userId];
            Console.WriteLine("Покупатель {0} покидает отдел {1}", userId, vendor);
            pipeServer.WriteByte((byte)Response.OK);
            pipeServer.WriteByte((byte)vendor);
            __buyers[userId] = 0;
            __vendors[vendor] = true;            
        }

        private void HandleIsVendorVacant(int userId)
        {
            if ((bool)__vendors[userId])
            {        
                pipeServer.WriteByte((byte)Response.OK);
                pipeServer.WriteByte((byte)0);
                return;
            }            
            pipeServer.WriteByte((byte)Response.NO);
            //pipeServer.WriteByte((byte)__buyers.Keys.OfType<int>().Single(s =>(int)__buyers[s] == userId));
            int buyer = (int)__buyerByVendor[userId];
            pipeServer.WriteByte((byte)buyer);
        }

        private void HandleNewVendor(int userId)
        {
            __buyerByVendor[userId] = 0;
            __vendorsId.Add(userId);
            __vendors[userId] = true;
            pipeServer.WriteByte((byte)Response.OK);
            pipeServer.WriteByte((byte)0);
        }

        private void HandleNewBuyer(int userId)
        {
            __buyers[userId] = 0;
            pipeServer.WriteByte((byte)Response.OK);
            pipeServer.WriteByte((byte)0);
        }
    }
}
