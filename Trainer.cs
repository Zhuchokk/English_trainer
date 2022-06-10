using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Speech.Synthesis;
using System.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;

namespace English_trainer
{
    public partial class Trainer : Form
    {
        public SpeechSynthesizer synth;
        User userdata;
        UserSettings usersettings;
        string[] words;
        string[,] tests;

        int index = 0;


        public Trainer()
        {
            using (StreamReader r = new StreamReader(Application.StartupPath + @"\userdata.json"))
            {
                string json = r.ReadToEnd();
                userdata = JsonConvert.DeserializeObject<User>(json);
            }
            using (StreamReader r = new StreamReader(Application.StartupPath + @"\settings.json"))
            {
                string json = r.ReadToEnd();
                usersettings = JsonConvert.DeserializeObject<UserSettings>(json);
            }
            using (StreamReader r = new StreamReader(Application.StartupPath + @"\data.txt"))
            {
                words = r.ReadToEnd().Split('\n');
            }

            tests = new string[usersettings.tests_qty + usersettings.words_qty, 2];

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

            tests[index, 0] = onlyword(words[userdata.last_number]);
            tests[index, 1] = only_rus(words[userdata.last_number]);
            index += 1;

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

        private string only_rus(string str)
        {
            return str.Substring(str.IndexOf(']') + 1);
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
                if (str.Substring(0, str.IndexOf('[')).Contains("("))
                {
                    tmp = str.Substring(0, str.IndexOf('(') - 1);
                }
                else
                {
                    tmp = str.Substring(0, str.IndexOf('[') - 1);

                }
            }
            else
            {
                if (str.Substring(0, str.IndexOf('[')).Contains("("))
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
            usersettings.words_qty -= 1;
            next_word();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (usersettings.words_qty > 0)
            {
                usersettings.words_qty -= 1;
                userdata.words += 1;
                string only_word = onlyword(words[userdata.last_number - 1]);

                if (textBox1.Text.ToLower() == only_word)
                {
                    textBox1.Clear();
                    next_word();
                }
                else
                {
                    MessageBox.Show("You should write a word: '" + only_word + "' in the text field. Then you can press 'Next' button");
                }
            }
            else if (usersettings.words_qty == 0)
            {
                label1.Text = "word number: -";
                label4.Text = "type: -";
                label2.Text = "TESTS";
                label3.Text = "Click the 'Next' button to start";
                MessageBox.Show("Word CHECK!" + usersettings.tests_qty + "tests");
                Random random = new Random();
                for (int i=0; i< usersettings.tests_qty; i++)
                {
                    int tmp = random.Next(0, userdata.last_number);
                    tests[index + i, 0] = onlyword(words[tmp]);
                    tests[index + i, 1] = only_rus(words[tmp]);
                }
                
                usersettings.words_qty -= 1;
                
            } else if(usersettings. tests_qty > 0)
            {
                if(label2.Text == "TESTS")
                {
                    label2.Text = tests[0, 1];
                    index = 1;
                    usersettings.tests_qty -= 1;
                    label3.Text = "";
                    textBox1.Clear();
                    button1.Dispose();
                    button3.Dispose();
                }
                else
                {
                    if(textBox1.Text == tests[index - 1, 0]){
                        MessageBox.Show("True Answer!");
                        userdata.true_answer += 1;
                    } else
                    {
                        MessageBox.Show("Wrong Answer, true answer is'" + tests[index - 1, 0] + "'");
                        userdata.wrong_answer += 1;
                    }

                    
                    label2.Text = tests[index, 1];
                    index += 1;

                    
                    usersettings.tests_qty -= 1;
                    textBox1.Clear(); 
                }
            }
            else
            {
                userdata.days += 1;
                userdata.last = DateTime.Now.ToShortDateString();

                MessageBox.Show("You have completed the daily lesson! Have a nice day!");
                using (StreamWriter w = new StreamWriter(Application.StartupPath + @"\userdata.json"))
                {
                    string json = JsonConvert.SerializeObject(userdata);
                    w.Write(json);
                } 
                Application.Exit();
            }
            

        }

        private void button1_Click(object sender, EventArgs e)
        {
            synth = new SpeechSynthesizer();
            synth.SelectVoice(usersettings.voice);
            synth.SetOutputToDefaultAudioDevice();
            synth.SpeakAsync(onlyword(words[userdata.last_number - 1]));
            synth.Dispose();
            GC.Collect();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Process translate = Process.Start(string.Format( "https://translate.google.com/?hl=ru&sl=en&tl=ru&text={0}&op=translate", onlyword(words[userdata.last_number - 1])));
        }
    }
}
