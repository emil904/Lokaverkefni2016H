﻿using System;
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
        private NetworkStream output;
        private BinaryWriter writer;
        private BinaryReader reader;
        private string message = "";
        private string clientInput = "";
            
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
                client.Connect("localhost", 50000);
                output = client.GetStream();
                writer = new BinaryWriter(output);
                reader = new BinaryReader(output);
                
                do
                {
                     clientInput = emailTextBox.Text + ":" + passwordTextBox.Text;
                     writer.Write(clientInput);
                     message = reader.ReadString();
                     if (message == "User validated.")
                     {
                         this.Invoke(new MethodInvoker(this.Hide));
                         Form2 clientWindow = new Form2();
                         clientWindow.Show();
                     }
                     else
                     {
                         labelInvoke("Incorrect username or password, please try again.");
                         emailTextBox.Text = "";
                         passwordTextBox.Text = "";
                         
                     }
                    
                } while (message != "User validated.");
                
            } // end try
            catch (Exception error)
            {
                MessageBox.Show(error.ToString());
            } 
             
        }

        private void exitButton_Click(object sender, EventArgs e) // Þegar ýtt er á "Exit" takkann
        {
            this.Close(); // Ef ýtt er á exit takka þá slökknar á forritinu
        }

        private void loginButton_Click(object sender, EventArgs e) // Þegar ýtt er á "Sign in" takkann
        {
            new Thread(new ThreadStart(Connect)).Start();
            
        }

        public void labelInvoke(string value) //Method til að geta breytt um texta í errorLabel úr öðrum en "main" þræði.
        {
            if (errorLabel.InvokeRequired)
            {
                this.BeginInvoke(new Action<string>(labelInvoke), new object[] { value });
                return;
            }
            errorLabel.Text += value;
        }
    }
}
