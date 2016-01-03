using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WindowsFormsApplication1.ServiceReference1;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (NorthwindServiceClient n = new NorthwindServiceClient())
            {
                dataGridView1.DataSource =
                    n.GetAllCustomers();
            }
        }

        NorthwindServiceClient c = new NorthwindServiceClient();

        private void button2_Click(object sender, EventArgs e)
        {
            c.GetAllCustomersCompleted += new EventHandler<GetAllCustomersCompletedEventArgs>(c_GetAllCustomersCompleted);
            c.GetAllCustomersAsync();
        }

        void c_GetAllCustomersCompleted(object sender, GetAllCustomersCompletedEventArgs e)
        {
            dataGridView1.DataSource = e.Result;
            c.Close();
        }
    }
}
