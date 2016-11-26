using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace LokaverkefniServer
{
    class Program
    {
        private Socket connection; //Socket sem acceptar connectionum
        private int counter = 0; //counter fyrir fjölda usera á server
        private int port = 8190;
        private string message = null; //Message frá userum á server
        private NetworkStream socketStream = null;
        private BinaryWriter writer = null;
        private BinaryReader reader = null;

        static void Main(string[] args)
        {
            dbClass db = new dbClass(); //býr til db object
            db.TengingVidDB(); //Keyrir function inní dbClass.cs sem tengir við tsuts.tskoli.is db'ið

            new Program().Run();
        }

        void Run() 
        {
            new Thread(new ThreadStart(RunServer)).Start();
        }

        public void RunServer()
        {
            Thread readThread; //
            bool done = false;

            TcpListener listener;
            try
            {
                listener = new TcpListener(IPAddress.Any, port);
                listener.Start();
                Console.WriteLine("Waiting for connections ...");

                while(!done)
                {
                    connection = listener.AcceptSocket();
                    counter++;
                    Console.WriteLine("User connected. There are currently " + counter + " users connected");
                    readThread = new Thread(new ThreadStart(GetClient));
                }
            }
            catch (Exception)
            {

                Console.WriteLine("Port " + port + " may be busy. Try another.");
            }

        }

        public void GetClient() 
        {
            Socket socket = connection;
            int count = counter;

            try
            {
                socketStream = new NetworkStream(socket);
                reader = new BinaryReader(socketStream);
                writer = new BinaryWriter(socketStream);
                writer.Write("Connection successful. \n");

                bool okay = false;
                for (int tries = 0; tries < 3 && !okay; tries++)
                {
                    writer.Write("Please type in your PIN number or press CANCEL");
                    message = reader.ReadString();
                    Console.WriteLine("Client " + count + ":" + message);
                    okay = true;

                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e.ToString()); ;
            }
            finally
            {
                reader.Close();
                writer.Close();
                socketStream.Close();
                socket.Close();
            }
        }
        
    }


}
