using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Net.Sockets;
using System.Net;

namespace lokaverkefniLogin
{
    public partial class Form2 : Form
    {
        public Thread chatThread;
        private string message = "";
        private Form1 f1;

        public Form2(Form1 form1)
        {
            InitializeComponent();
            this.f1 = form1;
            this.ShowDialog();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            chatThread = new Thread(new ThreadStart(ClientStart));
            chatThread.Start();
        }

        public void ClientStart()
        {
            try
            {
                DisplayMessage("\r\n Connection Successful \r\n");
            }
            catch (Exception e)
            {
                DisplayMessage(e.ToString());
            }

            txtMessagebox.ReadOnly = false;

            try
            {
                Chat();
            }
            catch (Exception e)
            {
                f1.writer.Write(e.ToString());
            }

            finally
            {
                f1.writer.Close();
                f1.reader.Close();
                f1.output.Close();
            }

        }
        public void Chat()
        {
            do
            {
                try
                {
                    message = f1.reader.ReadString();
                    if(message.Contains("USERLIST:"))
                    {
                        f1.usersstring = message.Remove(0, 9);
                        DisplayUsersClear(f1.usersstring);
                        DisplayUsers(f1.usersstring);
                    }
                    else
                    {
                        DisplayMessage(message);
                    }
                    
                    
                }
                catch (Exception e)
                {
                    f1.writer.Write(e.ToString());
                }
            } while (message != "SERVER>>> TERMINATE");

        }

        public void Send()
        {
            try
            {
                f1.writer.Write(f1.username + ": " + txtMessagebox.Text);
                txtMessagebox.Clear();
            }
            catch (SocketException e)
            {
                f1.writer.Write(e.ToString());
            }
        }

        private void DisplayMessage(string message)
        {
            if (txtUsers.InvokeRequired)
            {
                Invoke(new DisplayDelegate(DisplayMessage),
                    new object[] { "\r\n" + message });
            }
            else
                txtChatbox.Text += message;
        }

        private void DisplayUsers(string message)
        {
            if (txtUsers.InvokeRequired)
            {
                Invoke(new DisplayDelegate(DisplayUsers),
                    new object[] { message });
            }
            else
                txtUsers.Text += message;
        }
        private void DisplayUsersClear(string message) 
        {
            message = "";
            if (txtUsers.InvokeRequired)
            {
                Invoke(new DisplayDelegate(DisplayUsersClear),
                    new object[] { message });
            }
            else
                txtUsers.Text = message;
        }

        private delegate void DisplayDelegate(string message);
        private delegate void DisableInputDelegate(bool value);

        private void DisableInput( bool value )
        {
            if (txtMessagebox.InvokeRequired)
            {
                Invoke(new DisableInputDelegate(DisableInput),
                    new object[] { value });
            }
            else
                txtMessagebox.ReadOnly = value;
        }

        private void btSend_Click(object sender, EventArgs e)
        {
            if (txtMessagebox.ReadOnly == false)
                Send();
        }

        private void txtMessagebox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && txtMessagebox.ReadOnly == false)
                Send();
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Environment.Exit(System.Environment.ExitCode);
        }
    }
}
