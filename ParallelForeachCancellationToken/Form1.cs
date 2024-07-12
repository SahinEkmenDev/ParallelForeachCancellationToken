using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ParallelForeachCancellationToken
{
    public partial class Form1 : Form
    {
        
        public int counter { get; set; } = 0;
        CancellationTokenSource cts;    
        public Form1()
        {
            InitializeComponent();
            cts = new CancellationTokenSource();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            cts=new CancellationTokenSource();

            
            List<string> Urls = new List<string>()
            {
                "https://www.google.com",
                "https://www.youtube.com",
                "https://www.haberturk.com",
                "https://www.microsoft.com",
                "https://www.google.com",
                "https://www.youtube.com",
                "https://www.haberturk.com",
                "https://www.microsoft.com",
                "https://www.google.com",
                "https://www.youtube.com",
                "https://www.haberturk.com",
                "https://www.microsoft.com",
                "https://www.google.com",
                "https://www.youtube.com",
                "https://www.haberturk.com",
                "https://www.microsoft.com"

            };
            HttpClient client = new HttpClient();
            ParallelOptions Po = new ParallelOptions();
            Po.CancellationToken=cts.Token;


            //burası çok önemli  burda  biz  aslında paralel thread kullandığımız için 
            //paralelforeach  içi birçom threead kulanıyor  ama  aslında  dışı  sadece  tek bir  
            //threadla çalıştığı  için  burda  formda donmlara oluşuyor onun sebebi  tek bir  thread kullandığımız  için.

            Task.Run(() =>
            {
                try
                {
                    Parallel.ForEach<string>(Urls, Po, (Url) =>
                    {
                        string content = client.GetStringAsync(Url).Result;
                        string data = $"{Url}:{content.Length}";
                        cts.Token.ThrowIfCancellationRequested();

                        listBox1.Invoke((MethodInvoker)delegate { listBox1.Items.Add(data); });
                    });


                }
                catch (Exception ex )
                {

                    MessageBox.Show("işlem iptal edildi :" );
                }
                



            });
            
           
        }

        private void button4_Click(object sender, EventArgs e)
        {
            cts.Cancel();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button3.Text = counter++.ToString();
        }
    }
}
