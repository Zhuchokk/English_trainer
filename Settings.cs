using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Speech.Synthesis;
using System.Linq;
using System.IO;
using Newtonsoft.Json;
using System.Windows.Forms;

namespace English_trainer
{
    public partial class Settings : Form
    {
        UserSettings settings;
        public Settings()
        {
            using (StreamReader r = new StreamReader(Application.StartupPath + @"\settings.json"))
            {
                string json = r.ReadToEnd();
                settings = JsonConvert.DeserializeObject<UserSettings>(json);
            }

            InitializeComponent();

            try
            {
                using (SpeechSynthesizer synth = new SpeechSynthesizer())
                {
                    foreach (var v in synth.GetInstalledVoices().Select(v => v.VoiceInfo))
                    {
                        if (v.Culture.ToString().Contains("en"))
                        {
                            comboBox1.Items.Add(v.Name);
                        }
                        
                        
                    }
                }
            }
            catch { }
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            numericUpDown1.Value = settings.words_qty;
            numericUpDown2.Value = settings.tests_qty;
            comboBox1.SelectedItem = settings.voice;
        }

        private void Settings_FormClosed(object sender, FormClosedEventArgs e)
        {
            settings.words_qty = Convert.ToInt32(numericUpDown1.Value);
            settings.tests_qty = Convert.ToInt32( numericUpDown2.Value);
            settings.voice = comboBox1.SelectedItem.ToString();
            using (StreamWriter w = new StreamWriter(Application.StartupPath + @"\settings.json"))
            {
                string json = JsonConvert.SerializeObject(settings);
                w.Write(json);
            }

        }
    }
}
