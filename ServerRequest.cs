using System;
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

    class ServerRequest
    {
        private static System.Timers.Timer aTimer;
        private Form1 main;
        public int counter;
        public string UDA_index1;
        public string char1;
        ArrayList L = new ArrayList();
        public ServerRequest(Form1 form, string f)
        {
            main = form;
            UDA_index1 = f;
            counter = 0;
            aTimer = new System.Timers.Timer(10000);
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Interval = 1000;
            aTimer.Enabled = true;

        }
        // sta funzione ritorna l'url del put?? allora chiamala cosi
        public string getPutUrl(string k)
        {
            int ik = Int32.Parse(k);
            if (ik >= 0 && ik < 8)
                return "https://www.sagosoft.it/_API_/cpim/luda/www/luda_20200901_0900//api/uda/put/?i="+ UDA_index1 + "&k=" + ik.ToString();
            else
                return "";


            /*if (String.Equals(k, Convert.ToString(0)))
            {
                url = "https://www.sagosoft.it/_API_/cpim/luda/www/luda_20200901_0900//api/uda/put/?i=1&k=0";
            }
            if (String.Equals(k, Convert.ToString(1)))
            {
                url = "https://www.sagosoft.it/_API_/cpim/luda/www/luda_20200901_0900//api/uda/put/?i=1&k=1";

            }
            if (String.Equals(k, Convert.ToString(2)))
            {
                url = "https://www.sagosoft.it/_API_/cpim/luda/www/luda_20200901_0900//api/uda/put/?i=1&k=2";
            }
            if (String.Equals(k, Convert.ToString(3)))
            {
                url = "https://www.sagosoft.it/_API_/cpim/luda/www/luda_20200901_0900//api/uda/put/?i=1&k=3";

            }
            if (String.Equals(k, Convert.ToString(4)))
            {
                url = "https://www.sagosoft.it/_API_/cpim/luda/www/luda_20200901_0900//api/uda/put/?i=1&k=4";
            }
            if (String.Equals(k, Convert.ToString(5)))
            {
                url = "https://www.sagosoft.it/_API_/cpim/luda/www/luda_20200901_0900//api/uda/put/?i=1&k=5";
            }
            if (String.Equals(k, Convert.ToString(6)))
            {
                url = "https://www.sagosoft.it/_API_/cpim/luda/www/luda_20200901_0900//api/uda/put/?i=1&k=6";
            }
            if (String.Equals(k, Convert.ToString(7)))
            {
                url = "https://www.sagosoft.it/_API_/cpim/luda/www/luda_20200901_0900//api/uda/put/?i=1&k=7";
            }

            concatenate = string.Concat("i=", UDA_index1);
            url = url.Replace("i=1", concatenate);
            return url;
            */
        }
        
        // cosa vuol dire Rexcp1???? dai dei nomi sensati alle cose.
        public async static Task<string> Rexcp1(string url)
        {
            WebRequest server = HttpWebRequest.Create(url);
            var response = server.GetResponse();
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                var result = await reader.ReadToEndAsync();
                return result;
            }
        }


        // non ho capito cosa fa questa callback. qui ci devi mettere SOLO la parte di get, periodico.
        // che riceve lo status e, solo se è nuovo, lo passa alla form che ha instanziato questa classe.
        // si era detta, ma continui a non ascoltare, che ti saresti tenuto qui in memoria l'ultimo status 

        // i put li metti altrove. questa classe deve esporre il metodo putStatus che setta lo status dell'uda 
        // e ritorna eventualmente un feedback se è riuscita a mandare l'informazione.
        public async void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            string url, get_url;
            //int leng_g;
            get_url = "https://www.sagosoft.it/_API_/cpim/luda/www/luda_20200901_0900//api/uda/get/?i="+ UDA_index1;
            //leng_g = get_url.Length;
            try
            {
                //concatenate_g = string.Concat("i=", UDA_index1);
                //get_url = get_url.Replace("i=1", concatenate_g);
                string t = await Rexcp1(get_url);
                main.Show_String(t, 1);              
                char1 = Convert.ToString(t[88]);    // t[88] cos'è!?!?!?!? se t cambia lunghezza non funziona piu. 
                                                    // t è la versione string di una struttura dati di tipo JSON. 
                                                    // tu devi fare il parsing di tale json e prendere il campo giusto.
                if (main.contatore == 0)
                {
                    main.L1.Insert(main.contatore, char1);
                    url = getPutUrl(char1);
                    string t1 = await Rexcp1(url);
                    main.Show_String(t1, 2);
                    main.Status_Changed(char1, 2);
                }
                else
                {

                    main.L1.Insert(main.contatore, char1);
                    if (main.L1[main.contatore] == main.L1[main.contatore - 1])
                    {
                        main.contatore = 0;
                    }
                    else
                    {
                        url = getPutUrl(char1);
                        string t1 = await Rexcp1(url);
                        main.Show_String(t1, 2);
                        main.Status_Changed(char1, 2);
                        main.contatore = 1;
                    }
                }
                main.contatore++;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(Convert.ToString(ex));
                throw new ApplicationException("Error", ex);
                aTimer.Stop();
            }

        }

        public int putStatus(int status)
        {
            return 0;
        }

    }
}
