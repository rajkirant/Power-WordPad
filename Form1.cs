using System.Net;
using System.Text.RegularExpressions;
namespace Power_WordPad


{

    public partial class Form1 : Form
    {
      
        string validateInputSentance = @"^([a-zA-Z ]{2,20}) of ([a-zA-Z ]{2,20}) is ([a-zA-Z ]{2,20}).$";
        string IncumbentExtract = "<b>Incumbent<br />[a-zA-Z<=\"/_>. ]+\">([a-zA-Z .]+)</a>";
        AutoCompleteStringCollection autoText = new AutoCompleteStringCollection();
        
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
                        System.Diagnostics.Debug.WriteLine("success");
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
                        textBox1.Text = m.Groups[1].ToString() + " of " + m.Groups[2].ToString() + " is " + correctData;

                    }
                });
               
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           // richTextBox1.Select(0, richTextBox1.Text.Length);

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
                
                //e.SuppressKeyPress = true;

            }


        }
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
           
        }
    }

}