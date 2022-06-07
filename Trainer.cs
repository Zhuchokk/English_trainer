using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace English_trainer
{
    public partial class Trainer : Form
    {
        public Trainer()
        {
            InitializeComponent();
        }

        private void Trainer_Load(object sender, EventArgs e)
        {
            label2.Text = "123456789123456789123456789";
            List<Control> ctls = new List<Control>();
            foreach (Control c in flowLayoutPanel2.Controls) ctls.Add(c);
            centerControls(ctls, flowLayoutPanel2);
        }
        void centerControls(List<Control> ctls, Control container)
        {
            int w = container.ClientSize.Width;
            int marge = (w - ctls.Sum(x => x.Width)) / 2;
            Padding oldM = ctls[0].Margin;
            ctls.First().Margin = new Padding(marge, oldM.Top, oldM.Right, oldM.Bottom);
            ctls.Last().Margin = new Padding(oldM.Left, oldM.Top, oldM.Right, marge);
        }
    }
}
