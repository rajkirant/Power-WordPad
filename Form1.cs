using System.Net;
using System.Text.RegularExpressions;
namespace Power_WordPad
{

    public partial class Form1 : Form
    {
      
        string validateInputSentance = @"^([a-zA-Z ]{2,20}) of ([a-zA-Z ]{2,20}) is ([a-zA-Z ]{2,20}).$";
        string IncumbentExtract = "<b>Incumbent<br />[a-zA-Z<=\"/_>. ]+\">([a-zA-Z .]+)</a>";
        string toolTip;
        AutoCompleteStringCollection autoText = new AutoCompleteStringCollection();
        public Form1()
        {
            InitializeComponent();
        }
        public void scanText(object pattern)
        {

            this.Invoke((MethodInvoker)delegate ()
            {
                Match m = Regex.Match(richTextBox1.Text.Trim(), validateInputSentance);
                if (m.Success)
                {
                    string[] patterns = { IncumbentExtract };
                    string toBeValidatedData = m.Groups[3].ToString();
                    string correctData = fetchinfo("https://en.wikipedia.org/w/index.php?search=" + m.Groups[1] + " of " + m.Groups[2], patterns);
                    if (toBeValidatedData.Length < 3 || !correctData.ToLower().Contains(toBeValidatedData.ToLower()))
                    {
                        int startIndexVerifyData = richTextBox1.Text.IndexOf(toBeValidatedData);
                        richTextBox1.Select(startIndexVerifyData, toBeValidatedData.Length);
                        richTextBox1.SelectionColor = Color.Red;
                        richTextBox1.SelectionStart = richTextBox1.Text.Length;
                        richTextBox1.SelectionLength = 0;
                    }

                    toolStripMenuItem1.Visible = true;
                    toolStripMenuItem1.ToolTipText = "aaa";
                    toolTip = m.Groups[1].ToString() + " of " + m.Groups[2].ToString() + " is " + correctData;
                }
            });
        }
        private string fetchinfo(string url, string[] patterns)
        {
            WebRequest request = WebRequest.Create(url);
            Console.WriteLine(url);
            request.Credentials = CredentialCache.DefaultCredentials;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            System.IO.Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            Console.WriteLine(responseFromServer);
            reader.Close();
            dataStream.Close();
            response.Close();
            foreach (string pattern in patterns)
            {
                Match m = Regex.Match(responseFromServer.Trim(), pattern);
                if (m.Success)
                {
                    return m.Groups[1].ToString();
                }
            }
            return null;
        }
        private void richTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
           
            if (e.KeyData.ToString() == "OemPeriod")
            {
                new Thread(scanText).Start(validateInputSentance);

            }
            else if (e.KeyData.ToString() == "Back")
            {
                richTextBox1.Select(0, richTextBox1.Text.Length);
                richTextBox1.SelectionColor = Color.Black;
                richTextBox1.SelectionStart = richTextBox1.Text.Length;
                richTextBox1.SelectionLength = 0;
                toolStripMenuItem1.Visible = false;
            }
        }
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(richTextBox1.SelectionLength > 0)
            {
                richTextBox1.Copy();
            }
        }
        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Text))
            {
                richTextBox1.Paste();
            }
        }
        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr = fontDialog1.ShowDialog();
            if(dr == DialogResult.OK)
            {
                richTextBox1.Font = fontDialog1.Font;
            }
        }
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(toolTip);
       
        }
        private void toolStripMenuItem1_MouseEnter(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(menuStrip1, toolTip);
        }
        private void toolStripMenuItem1_MouseLeave(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(menuStrip1, null);
        }
    }

}