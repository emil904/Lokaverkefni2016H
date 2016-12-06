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
        private BinaryWriter writer;
        private BinaryReader reader;
        private NetworkStream socketStream;
        private string message;
        private int port = 50000;
        private bool connected = false;


        private List<TcpClient> clients = new List<TcpClient>();
        private List<string> currentUsers = new List<string>();
        private int counter;
        private Socket connection;
        private string cu;
        static dbClass db = new dbClass();



        static void Main(string[] args)
        {
            //Keyrir function inní dbClass.cs sem tengir við tsuts.tskoli.is db'ið
            db.TengingVidDB();

            new Program().RunServer();
        }

        public void RunServer()
        {
            Thread clientThread;
            TcpListener listener;

            try
            {
                Console.WriteLine("Waiting for connections ...");
                listener = new TcpListener(IPAddress.Any, port);
                listener.Start();
                counter = 0;
                while (!connected)
                {
                    counter += 1;
                    connection = listener.AcceptSocket();

                    Console.WriteLine("User " + counter + " connected");

                    socketStream = new NetworkStream(connection);
                    writer = new BinaryWriter(socketStream);
                    reader = new BinaryReader(socketStream);

                    clientThread = new Thread(new ThreadStart(ClientStart));
                    clientThread.Start();
                     
                }


            }
            catch (Exception e)
            {
                Console.WriteLine("Port " + port + " may be busy. Try another.");
                Console.WriteLine(e);
                Console.ReadKey();
            }
        }

        public void ClientStart()
        {
            try
            {
                Login();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.ReadKey();
            }

            try
            {
                getUserList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.ReadKey();
            }
            try
            {
                Chat();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.ReadKey();
            }

            finally
            {
                reader.Close();
                writer.Close();
            }
        }

        public void Login()
        {
            try
            {
                //message = LOGIN:/REGISTER:email:password
                message = reader.ReadString();
                if (message.Contains("LOGIN:"))
                {
                    message = message.Remove(0, 6);
                    validateUser(message);
                }
                else if (message.Contains("REGISTER:"))
                {
                    message = message.Remove(0, 9);
                    registerUser(message);
                    message = reader.ReadString();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.ReadKey();
            }
        }

        public void Chat()
        {
            string reply = "";

            while (true)
            {
                try
                {
                    reply = reader.ReadString();

                    Console.WriteLine(reply);
                    writer.Write(reply);

                }
                catch (Exception)
                {
                    break;
                }
            }
        }

        public void getUserList()
        {
            cu = "";
            foreach (string user in currentUsers)
            {
                cu += user + "\r\n";
            }
            writer.Write("USERLIST:" + cu);
        }


        // -- Login/Register --

        public void validateUser(string toValidate)
        {
            string[] splitInfo = toValidate.Split(':'); //Splittar toValidate breytunni svo hægt sé að senda í findUserToValidate fallið.

            string validated = db.findUserToValidate(splitInfo[0].ToString(), splitInfo[1].ToString()); //Það sem findUserToValidate skilar

            if (validated == "U/P incorrect")
            {
                writer.Write("U/P incorrect");
            }
            else
            {
                string[] splitValidated = validated.Split(':'); //Splitta validated breytunni svo hægt sé að compare'a

                if (splitValidated[0] + splitValidated[2] == splitInfo[0] + splitInfo[1]) //hér compareum við breytuna.
                {
                    currentUsers.Add(splitInfo[1]);
                    Console.WriteLine("User " + splitValidated[2] + " verified.");
                    writer.Write("User validated:" + splitValidated[2]);

                }
            }

        }

        public void registerUser(string toRegister)
        {
            string[] splitRegister = toRegister.Split(':'); //Splittar strengnum sem er sendur frá client
            db.userRegister(splitRegister[0], splitRegister[1], splitRegister[2]); //Keyrir userRegister methodið sem bætir user við gagnagrunn
            writer.Write("User registered.");
        }


        // -- Login/Register end --
    }

}
