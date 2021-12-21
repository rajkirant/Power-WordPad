using System;
using System.Threading;

namespace Power_WordPad
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            Thread t1 = new Thread(Threads.scanText);
            t1.Start("test");
        }
        private void Form1_Load(object sender, EventArgs e)
        {
        }

    }
    public class Threads
    {
        public static void scanText(object pattern)
        {
            MessageBox.Show(pattern.ToString());
        }
    }
}