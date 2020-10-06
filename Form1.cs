﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Collections;
using System.Timers;
using System.Windows;
using System.Diagnostics;
using Timer = System.Timers.Timer;

namespace UDA_server_communication
{

    public partial class Form1 : Form
    {
        public int contatore;
        public string UDA_index;
        public ArrayList L1 = new ArrayList();

        // continua a riempirli fino a 7
        public ArrayList txts = new ArrayList() { "IDLE"};
        public ArrayList labels = new ArrayList() { "IDLE"};
        public ArrayList colors = new ArrayList() { Color.DarkGreen };

        public Form1(string x,int j)
        {
            ServerRequest r = new ServerRequest(this,x);
            UDA_index = x;
            InitializeComponent();
            contatore = 0;
            st.Visible = false;
            label1.Visible = false;
            textBox2.Visible = false;
        }
 
        public  void Show_String(string s, int i)
        {

            this.BeginInvoke((Action)delegate ()
            {
                if (i == 1)

                    textBox1.Text = s;
                else
                    textBox2.Text = s;
            });

        }
        public void Status_Changed(string k, int i)
        {
            this.BeginInvoke((Action)delegate ()
            {
                st.Visible = true;

                if (i == 2)
                {
                    textBox2.Visible = true;
                    label1.Visible = true;
                }
                setSelection(Int32.Parse(k));

                /*if (String.Equals(k, Convert.ToString(0)))
                {
                    st.ForeColor = Color.DarkGreen;
                    label1.ForeColor = Color.DarkGreen;
                    st.Text = "IDLE";
                    label1.Text = "IDLE";

                }
                if (String.Equals(k, Convert.ToString(1)))
                {
                    st.ForeColor = Color.Black;
                    label1.ForeColor = Color.Black;
                    st.Text = "START";
                    label1.Text = "STARTED";
                }
                if (String.Equals(k, Convert.ToString(2)))
                {
                    st.ForeColor = Color.DarkRed;
                    label1.ForeColor = Color.DarkRed;
                    st.Text = "ABORT";
                    label1.Text = "ABORTED";

                }
                if (String.Equals(k, Convert.ToString(3)))
                {
                    label1.ForeColor = Color.DarkOrange;
                    st.ForeColor = Color.DarkOrange;
                    st.Text = "PAUSE";
                    label1.Text = "PAUSED";
                }
                if (String.Equals(k, Convert.ToString(4)))
                {
                    st.ForeColor = Color.Brown;
                    label1.ForeColor = Color.Brown;
                    st.Text = "RESUME";
                    label1.Text = "RESUMED";
                }
                if (String.Equals(k, Convert.ToString(5)))
                {
                    st.ForeColor = Color.DarkOrchid;
                    label1.ForeColor = Color.DarkOrchid;
                    st.Text = "FINALIZE";
                    label1.Text = "COMPLETED";
                }
                if (String.Equals(k, Convert.ToString(6)))
                {
                    st.ForeColor = Color.DarkGreen;
                    label1.ForeColor = Color.DarkGreen;
                    st.Text = "FINISHED";
                    label1.Text = "FINALIZED";
                }
                if (String.Equals(k, Convert.ToString(7)))
                {
                    label1.ForeColor = Color.Purple;
                    st.ForeColor = Color.Purple;
                    st.Text = "NOT IMPLEMENTED";
                    label1.Text = "FINISHED";
                }*/
            });

        }

        private void setSelection(int k)
        {
            st.ForeColor        = (Color)colors[k];
            label1.ForeColor    = (Color)colors[k];
            st.Text             = (string)txts[k];
            label1.Text         = (string)labels[k];
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string nome;
            nome= string.Concat("UDA ", UDA_index);
            this.Text = nome;
        }

        private void button1_Click(object sender, EventArgs e)
        {
                this.Hide();
                var fr2 = new Form2();
                fr2.Closed += (s, args) => this.Close();
                fr2.Show();
        }
    }
}