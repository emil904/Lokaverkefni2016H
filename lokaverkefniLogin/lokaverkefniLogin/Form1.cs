using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Drawing;
using System.Windows.Forms;

namespace lokaverkefniLogin
{
    public partial class Form1 : Form
    {
        public NetworkStream output;
        public BinaryWriter writer;
        public BinaryReader reader;
        public string message;
        public string clientInput;
        private bool done = false;
        public string username;
        public string usersstring;
            
        public Form1()
        {
            InitializeComponent();
        }

        void Connect()
        {
            TcpClient client = null;
            try
            {
                client = new TcpClient();
                client.Connect("89.17.134.18", 50000);
                output = client.GetStream();
                writer = new BinaryWriter(output);
                reader = new BinaryReader(output);
                Login();
            } 

            catch (Exception error)
            {
                MessageBox.Show(error.ToString());
            }     
        }

        void Login()
        {
            do
            {
                writer.Write(clientInput);
                message = reader.ReadString();

                if (message.Contains("User validated"))
                {
                    string[] msgSplit = message.Split(':');
                    username = msgSplit[1];
                    this.Invoke(new MethodInvoker(this.Hide));
                    Form2 f2 = new Form2(this);
                    f2.Show();

                    Application.Run(new Form2(this));
                    done = true;
                }

                else if (message == "User registered.")
                {
                    DisplayRegErrorLabel("User succesfully registered! Please sign in.");
                    done = true;
                    
                }
                else if (message == "U/P incorrect")
                {
                    DisplayErrorLabel("Incorrect username or password, please try again.");
                    done = true;
                }

            } while (!done);
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            clientInput ="LOGIN:" + EmailTextBox.Text + ":" + PasswordTextBox.Text;
            Connect();
        }

        private void signupButton_Click(object sender, EventArgs e)
        {
            clientInput = "REGISTER:" + SignupEmailTextBox.Text + ":" + SignupDisplayNameTextBox.Text + ":" + SignupPasswordTextBox.Text;
            if (SignupEmailTextBox.Text == "" || SignupDisplayNameTextBox.Text == "" || SignupPasswordTextBox.Text == "")
            {
                    regErrorLabel.Text = "None of the register fields may be empty";
                    SignupDisplayNameTextBox.Text = "";
                    SignupEmailTextBox.Text = "";
                    SignupPasswordTextBox.Text = "";
            }
            else if(!SignupEmailTextBox.Text.Contains("@"))
            {
                regErrorLabel.Text = "Please enter a valid email address";
            }
            else
            {
                Connect();
            }
        }

        private delegate void DisplayDelegate(string message);

        private void DisplayRegErrorLabel(string message)
        {
            if (regErrorLabel.InvokeRequired)
            {
                Invoke(new DisplayDelegate(DisplayRegErrorLabel),
                    new object[] { message });
            }
            else
                regErrorLabel.Text = message;
        }

        private void DisplayErrorLabel(string message)
        {
            if (errorLabel.InvokeRequired)
            {
                Invoke(new DisplayDelegate(DisplayErrorLabel),
                    new object[] { message });
            }
            else
                errorLabel.Text = message;
        }
        
        private void exitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
