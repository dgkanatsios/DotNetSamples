using System;
using System.Windows.Forms;

namespace MicConsoleApplication
{
    class Program
    {
        public static void Main()
        {
            Form form = new Form();
            Button button = new Button();
            button.Name = "button";
            button.Text = "Click me!";
            button.Click += new EventHandler(button_Click);

            form.Controls.Add(button);

            Application.Run(form);

            Console.ReadLine();
        }

        static void button_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This was triggered by the Click event! :)");
        }
    }
}

