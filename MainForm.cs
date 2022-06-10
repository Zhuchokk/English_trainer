using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using Newtonsoft.Json;
using System.Windows.Forms;

namespace English_trainer
{
    public partial class MainForm : Form
    {
        User userdata;
        public MainForm()
        {
            using (StreamReader r = new StreamReader(Application.StartupPath + @"\userdata.json"))
            {
                string json = r.ReadToEnd();
                userdata = JsonConvert.DeserializeObject<User>(json);
            }
            InitializeComponent();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form form = new Trainer();
            form.Show();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            label3.Text = "Wrong answers: " + userdata.wrong_answer;
            label4.Text = "True answers: " + userdata.true_answer;
            label5.Text = "Days spent: " + userdata.days;

            string words = Convert.ToString( 5000 - userdata.words);
            Console.WriteLine(words.Length);
            int l = words.Length;
            for (int i=0; i < 4 - l ; i++)
            {
                words = "0" + words;
                Console.WriteLine(12345);
            }

            label1.Text = words;
            if(DateTime.Parse(userdata.last).Date == DateTime.Now.Date)
            {
                button2.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            Form form = new Settings();
            form.Show();
        }
    }
}
