using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PortQuizTester
{
    public partial class Main : Form
    {
        bool running = false;

        public Main()
        {
            InitializeComponent();
        }

        private void MethodWithParameter(int port)
        {
            try
            {
                TcpClient tcpclnt = new TcpClient() { SendTimeout = 2000, ReceiveTimeout = 2000 };

                tcpclnt.Connect("portquiz.net", port);
                // use the ipaddress as in the server program

                this.PerformSafely(() =>
                {
                    richTextBox1.AppendText($"Outgoing Port {port} Ok{Environment.NewLine}");
                    richTextBox1.SelectionStart = richTextBox1.Text.Length;
                    richTextBox1.ScrollToCaret();
                });

                //Console.WriteLine($"Port {port} Connected");

                tcpclnt.Close();
            }

            catch (Exception ex)
            {
                //this.PerformSafely(() =>
                //{
                //    listBox1.Items.Add($"Outgoing Port {port} Error - {ex.Message}");
                //    int visibleItems = listBox1.ClientSize.Height / listBox1.ItemHeight;
                //    listBox1.TopIndex = Math.Max(listBox1.Items.Count - visibleItems + 1, 0);
                //});
                Console.WriteLine($"Outgoing Port {port} Error - {ex.Message}");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            running = true;
            for (decimal i=numericUpDown1.Value;i<numericUpDown2.Value;i++)
            {
                var localParam = (int)i;

                //Task.Run(async () => MethodWithParameter(localParam));
                MethodWithParameter(localParam);

                Application.DoEvents();

                if(!running)
                {
                    MessageBox.Show("Stopped");
                    return;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            running = false;
        }
    }


    public static class CrossThreadExtensions
    {
        public static void PerformSafely(this Control target, Action action)
        {
            if (target.InvokeRequired)
            {
                target.Invoke(action);
            }
            else
            {
                action();
            }
        }

        public static void PerformSafely<T1>(this Control target, Action<T1> action, T1 parameter)
        {
            if (target.InvokeRequired)
            {
                target.Invoke(action, parameter);
            }
            else
            {
                action(parameter);
            }
        }

        public static void PerformSafely<T1, T2>(this Control target, Action<T1, T2> action, T1 p1, T2 p2)
        {
            if (target.InvokeRequired)
            {
                target.Invoke(action, p1, p2);
            }
            else
            {
                action(p1, p2);
            }
        }
    }
}
