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

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            label3.Text = "Wrong answers: " + userdata.wrong_answer;
            label4.Text = "True answers: " + userdata.true_answer;
            label5.Text = "Days spent: " + userdata.days;

            string words = Convert.ToString( userdata.words);
            Console.WriteLine(words.Length);
            int l = words.Length;
            for (int i=0; i < 4 - l ; i++)
            {
                words = "0" + words;
                Console.WriteLine(12345);
            }

            label1.Text = words;
            
            if(DateTime.Now == DateTime.Parse(userdata.last))
            {
                button1.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form form = new Settings();
            form.Show();
        }
    }
}
