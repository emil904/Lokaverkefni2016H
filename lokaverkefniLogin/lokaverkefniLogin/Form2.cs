using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace lokaverkefniLogin
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private NetworkStream output; // stream for receiving data           
        private BinaryWriter writer; // facilitates writing to the stream    
        private BinaryReader reader; // facilitates reading from the stream  
        private Thread readThread; // Thread for processing incoming messages
        private string message = "";

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Environment.Exit(System.Environment.ExitCode);
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            readThread = new Thread(new ThreadStart(RunClient));
            readThread.Start();    
        }

        private delegate void DisplayDelegate(string message);

        private void DisplayMessage(string message)
        {
            // if modifying txtChatbox is not thread safe
            if (txtChatbox.InvokeRequired)
            {
                // use inherited method Invoke to execute DisplayMessage
                // via a delegate                                       
                Invoke(new DisplayDelegate(DisplayMessage),
                   new object[] { message });
            } // end if
            else // OK to modify txtChatbox in current thread
                txtChatbox.Text += message;
        } // end method DisplayMessage

        private delegate void DisableInputDelegate(bool value);

        // method DisableInput sets txtMessagebox's ReadOnly property
        // in a thread-safe manner
        private void DisableInput(bool value)
        {
            // if modifying txtMessagebox is not thread safe
            if (txtMessagebox.InvokeRequired)
            {
                // use inherited method Invoke to execute DisableInput
                // via a delegate                                     
                Invoke(new DisableInputDelegate(DisableInput),
                   new object[] { value });
            } // end if
            else // OK to modify txtMessagebox in current thread
                txtMessagebox.ReadOnly = value;
        } // end method DisableInput

        // sends text the user typed to server
        private void inputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter && txtMessagebox.ReadOnly == false)
                {
                    writer.Write("CLIENT>>> " + txtMessagebox.Text);
                    txtChatbox.Text += "\r\nCLIENT>>> " + txtMessagebox.Text;
                    txtMessagebox.Clear();
                } // end if
            } // end try
            catch (SocketException)
            {
                txtChatbox.Text += "\nError writing object";
            } // end catch
        } // end method inputTextBox_KeyDown

        // connect to server and display server-generated text
        public void RunClient()
        {
            TcpClient client;

            // instantiate TcpClient for sending data to server
            try
            {
                DisplayMessage("Attempting connection\r\n");

                // Step 1: create TcpClient and connect to server
                client = new TcpClient();
                client.Connect("127.0.0.1", 50000);

                // Step 2: get NetworkStream associated with TcpClient
                output = client.GetStream();

                // create objects for writing and reading across stream
                writer = new BinaryWriter(output);
                reader = new BinaryReader(output);

                DisplayMessage("\r\nGot I/O streams\r\n");
                DisableInput(false); // enable txtMessagebox

                // loop until server signals termination
                do
                {
                    // Step 3: processing phase
                    try
                    {
                        // read message from server        
                        message = reader.ReadString();
                        DisplayMessage("\r\n" + message);
                    } // end try
                    catch (Exception)
                    {
                        // handle exception if error in reading server data
                        System.Environment.Exit(System.Environment.ExitCode);
                    } // end catch
                } while (message != "SERVER>>> TERMINATE");

                // Step 4: close connection
                writer.Close();
                reader.Close();
                output.Close();
                client.Close();

                Application.Exit();
            } // end try
            catch (Exception error)
            {
                // handle exception if error in establishing connection
                MessageBox.Show(error.ToString(), "Connection Error",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Environment.Exit(System.Environment.ExitCode);
            } // end catch
        }
    }
}
