using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UDA_server_communication
{
    public partial class Form2 : Form
    {
        private string Myval;
        public string check1;
        public string MyVal
        {
            get { return Myval; }
            set { Myval = value; }
        }
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            var fr1 = new Form1(MyVal,1);
            fr1.Closed += (s, args) => this.Close();
            fr1.Show();
        }


        // secondo me c'è il controllo radio-group che ti permette di avere una callback sola che dice quale dei 5 è selezionato.
        private void radioButton1_CheckedChanged_1(object sender, EventArgs e)
        {
            MyVal = "1";
            button1.Enabled = true;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            MyVal = "2";
            button1.Enabled = true;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            MyVal = "3";
            button1.Enabled = true;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            MyVal = "4";
            button1.Enabled = true;
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            MyVal = "5";
            button1.Enabled = true;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
        }
    }
}
