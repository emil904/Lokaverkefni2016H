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
        private int port = 50000;
        private string message = null; //Message frá userum á server
        private NetworkStream socketStream = null;
        private BinaryWriter writer = null;
        private BinaryReader reader = null;
        static dbClass db = new dbClass(); //býr til db object

        static void Main(string[] args)
        {
            db.TengingVidDB(); //Keyrir function inní dbClass.cs sem tengir við tsuts.tskoli.is db'ið
            new Program().Run();
        }

        void Run() 
        {
            new Thread(new ThreadStart(RunServer)).Start();
        }

        public void RunServer()
        {   
            Thread readThread;
 
            try
            {
                TcpListener listener = new TcpListener(IPAddress.Any, port);
                listener.Start();
                Console.WriteLine("Waiting for connections ...");

                while(true)
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
            
            try
            {
                socketStream = new NetworkStream(socket);
                reader = new BinaryReader(socketStream);
                writer = new BinaryWriter(socketStream);
                
                message = reader.ReadString(); //Hér koma email + password í breytuna í email:password formatti.
                Console.WriteLine(message);
                validateUser(message);

            }
            catch (Exception e)
            {

                Console.WriteLine(e.ToString());
            }
            finally
            {
                reader.Close();
                writer.Close();
                socketStream.Close();
                socket.Close();
            }
        }
        
        public void validateUser(string toValidate)
        {
            string[] splitInfo = toValidate.Split(':'); //Splittar toValidate breytunni svo hægt sé að senda í findUserToValidate fallið.

            string validated = db.findUserToValidate(splitInfo[0].ToString(), splitInfo[1].ToString()); //Það sem findUserToValidate skilar
            
            string[] splitValidated = validated.Split(':'); //Splitta validated breytunni svo hægt sé að compare'a

            if (splitValidated[0] + splitValidated[1] == splitInfo[0] + splitInfo[1])
            {
                Console.WriteLine("User verified.");
                writer.Write("User verified.");
            }
        }
    }


}
