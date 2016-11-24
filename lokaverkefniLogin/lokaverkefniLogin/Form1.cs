using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace lokaverkefniLogin
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void exitButton_Click(object sender, EventArgs e) // Þegar ýtt er á "Exit" takkann
        {
            this.Close(); // Ef ýtt er á exit takka þá slökknar á forritinu
        }

        private void loginButton_Click(object sender, EventArgs e) // Þegar ýtt er á "Sign in" takkann
        {

            this.Hide();
            Form2 clientWindow = new Form2();
            clientWindow.Show();
        }
    }
}
