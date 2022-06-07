using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Windows.Forms;

namespace English_trainer
{
    public partial class Trainer : Form
    {
        User userdata;
        string[] words;


        public Trainer()
        {
            using (StreamReader r = new StreamReader(Application.StartupPath + @"\userdata.json"))
            {
                string json = r.ReadToEnd();
                userdata = JsonConvert.DeserializeObject<User>(json);
            }
            using (StreamReader r = new StreamReader(Application.StartupPath + @"\data.txt"))
            {
                words = r.ReadToEnd().Split('\n');
            }
            InitializeComponent();
        }

        public string[] spliter(string str)
        {
            string[] splited = new string[2];
            if(!("0123456789".Contains(str[0])))
            {
                splited[0] = str.Substring(0, str.IndexOf("] ") + 2);
            }
            else
            {
                splited[0] = str.Substring(str.IndexOf(" ") + 1, str.IndexOf("] ") - str.IndexOf(" "));
            }
            
            splited[1] = str.Substring(str.IndexOf("] ") + 2);
            return splited;
        }

        private void next_word()
        {
            
            string[] word_translate = spliter(words[userdata.last_number]);
            string num = only_number(words[userdata.last_number]);
            
            label2.Text = word_translate[0];
            label3.Text = word_translate[1];

            if(num != "no")
            {
                label1.Text = "word number: " + num;
                label4.Text = "type: new word";
            }
            else
            {
                label1.Text = "word number: -";
                label4.Text = "type: word variation";
            }
            userdata.last_number += 1;
        }

        private string only_number(string str)
        {
            string number = "";

            if ("0123456789".Contains(str[0]))
            {
                for(int i=0; i < str.Length; i++)
                {
                    if ("0123456789".Contains(str[i])){
                        number += str[i];
                    }
                    else
                    {
                        break;
                    }
                }
                return number;
            }
            else
            {
                return "no";
            }
        }

        private string onlyword(string str)
        {
            string tmp;
            if (!("0123456789".Contains(str[0])))
            {
                if (str.Contains("("))
                {
                    tmp = str.Substring(0, str.IndexOf('(') - 2);
                }
                else
                {
                    tmp = str.Substring(0, str.IndexOf('[') - 1);

                }
            }
            else
            {
                if (str.Contains("("))
                {
                    tmp = str.Substring(str.IndexOf(" ") + 1, str.IndexOf('(') - str.IndexOf(" ") - 2);
                }
                else
                {
                    tmp = str.Substring(str.IndexOf(" ") + 1, str.IndexOf('[') - str.IndexOf(" ") - 2);
                }
            }
            return tmp.Trim();
            
        }

        private void Trainer_Load(object sender, EventArgs e)
        {
            next_word();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string only_word = onlyword(words[userdata.last_number - 1]);

            if(textBox1.Text == only_word)
            {
                textBox1.Clear();
                next_word();
            }
            else
            {
                MessageBox.Show("You should write a word: '" + only_word + "' in the text field. Then you can press 'Next' button");
            }

            
        }
    }
}
