﻿using System;
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
            button1.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            var fr1 = new Form1(MyVal, 1);
            fr1.Closed += (s, args) => this.Close();
            fr1.Show();
        }


        //Alberto: secondo me c'è il controllo radio-group che ti permette di avere una callback sola che dice quale dei 5 è selezionato.
        // Walter: Ho raggruppato i Radio-Buttons in un'unica groupbox invece che avere dei moduli separati

        private void Form2_Load(object sender, EventArgs e)
        {
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                MyVal = "1";
                button1.Enabled = true;
            }
            if (btn2.Checked == true)
            {
                MyVal = "2";
            }
            if (btn3.Checked == true)
            {
                MyVal = "3";
            }
            if (btn4.Checked == true)
            {
                MyVal = "4";
            }
            if (btn5.Checked == true)
            {
                MyVal = "5";
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
