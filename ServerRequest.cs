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
using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Tulpep.NotificationWindow;
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
        public string save_status;
        ArrayList L = new ArrayList();
        public ServerRequest(Form1 form, string f)
        {
            main = form;
            UDA_index1 = f;
            counter = 0;
            aTimer = new System.Timers.Timer(10000);
            aTimer.Elapsed += new ElapsedEventHandler(Get_Status_UDA);
            aTimer.Interval = 1000;
            aTimer.Enabled = true;

        }
        //Alberto: sta funzione ritorna l'url del put?? allora chiamala cosi
        public string getPutUrl(string k)
        {
            int ik = Int32.Parse(k);
            if (ik >= 0 && ik < 8)
                return "https://www.sagosoft.it/_API_/cpim/luda/www/luda_20200901_0900//api/uda/put/?i="+ UDA_index1 + "&k=" + ik.ToString();
            else
                return "";
        }
        
        //Alberto: cosa vuol dire Recxmp1???? dai dei nomi sensati alle cose.
        // Walter: Questo modulo serve per l'interrogazione con il server. Ho cambiato il suo nome in "Server_Request"
        public async static Task<string> Server_Request(string url)
        {
            WebRequest server = HttpWebRequest.Create(url);
            var response = server.GetResponse();
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                var result = await reader.ReadToEndAsync();
                return result;
            }
        }


        // Alberto: non ho capito cosa fa questa callback. qui ci devi mettere SOLO la parte di get, periodico.
        // che riceve lo status e, solo se è nuovo, lo passa alla form che ha instanziato questa classe.
        // si era detta, ma continui a non ascoltare, che ti saresti tenuto qui in memoria l'ultimo status 

        //Walter: La callback OneTimeEdvent (adesso Get_Status_UDA) serviva inizialmente per Get e Put. Ho cambiato adesso. 
        // Questa funzione viene invocata dal timer e serve per ottenere (get) lo stato della UDA selezionata.
        // Dalla applicazione implementata da Sax, vado a cambiare lo stato della mia UDA tramite il comando PUT.
        // L'UDA viene interrogata ogni secondo e solo se lo stato cambia viene mandato il messaggio di PUT al server. Esempio:
        // L'UDA è al tempo 0 nello stato IDLE, quindi lo comunica al server (manda tramite il PUT il suo stato).
        // Per un pò resta così finchè, dalla applicazione di Sax, mando il messaggio START (k=1). La funzione fa un match 
        // con l'ultimo stato salvato. Quindi al tempo T(actual)-1, siccome lo stato attuale e quello salvato non corrispondono, 
        //manda un messaggio al server con che riceverà quindi la stringa relativa a STARTED, visibile sia dall'exe che ho fatto sia dalla applicazione
        // di Sax. Non so se mi sono spiegato bene, o se intendevi queste modifiche nel commento precedente, magari ho capito
        // male io. Nel caso sentiamoci. 

        
        public async void Get_Status_UDA(object source, ElapsedEventArgs e)
        {
            string url, get_url,char_status,char_status1;
            get_url = "https://www.sagosoft.it/_API_/cpim/luda/www/luda_20200901_0900//api/uda/get/?i="+ UDA_index1;
            try
            {
                string json_string = await Server_Request(get_url);
                main.Show_String(json_string, 1);
                JObject json_parsed = JObject.Parse(json_string);
                char_status = (string)json_parsed["status"];    //Alberto: t[88] cos'è!?!?!?!? se t cambia lunghezza non funziona piu. 
                                                                // t è la versione string di una struttura dati di tipo JSON. 
                                                                // tu devi fare il parsing di tale json e prendere il campo giusto.
                                                                // Walter: Ho fatto il parsing così da selezionare il campo giusto.

                // La variabile main contatore server per salvare lo stato "attuale" per poi confrontarlo di volta in volta.
                // Se il contatore è a 0, vuol dire che l'applicazione è appena partita quindi l'UDA si trova già in un suo 
                // status, dato dalla applicazione di Sax. Il contatore diventa quindi 1, e si passa all'else, all'interno 
                // del quale, si confronta lo stato attuale con quello precedente, ovvero quello salvato in memoria. 
                if (main.contatore == 0)
                {
                    save_status = char_status; 
                    putStatus_Server(char_status);
                    main.contatore++;
                }
                else
                {
                    char_status1 = (string)json_parsed["status"];
                    if (!string.Equals(char_status1, save_status))
                    {
                        main.contatore = 0;
                        putStatus_Server(char_status1);
                        MessageBox.Show("Stato Cambiato!!", "Caption",MessageBoxButtons.OK);
                    }
                }
            }
            catch (Exception ex)
            {
               // MessageBox.Show(Convert.ToString(ex));
                throw new ApplicationException("Error", ex);
                aTimer.Stop();
            }

        }

        //Alberto: Questa classe deve esporre il metodo putStatus che setta lo status dell'uda 
        // e ritorna eventualmente un feedback se è riuscita a mandare l'informazione.

        //Walter: Il metodo putStatus_Server, serve affinchè l'UDA possa mandare al server il messaggio 
        //del suo stato, quindi il feedback. Lo status dell'UDA, puoi soltato settarlo dall'applicazione di Sax se non vuoi che l'exe
        // abbia alcun pulsante di interazione con l'utente.
        //Quindi, se l'UDA ha per esemprio ricevuto dal server, tramite la stringa PUT il comando "START" (k=1), rimanderà al server la notifica "STARTED"
        // tramite il put. Il cambio nello stato del server lo si può vedere sia dalla textbox in basso a destra della Form1
        // sia dall'applicazione di Sax
        public async void putStatus_Server(string status)
        {
            string url_put_server;
            url_put_server = getPutUrl(status);
            string server_status = await Server_Request(url_put_server);
            main.Show_String(server_status, 2);
            main.Status_Changed(status, 2);
        }

    }
}
