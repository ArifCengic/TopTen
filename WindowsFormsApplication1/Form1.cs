using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Net;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBoxWords.Clear();

            //string[] args = new string[] { "www.klix.ba", "www.radiosarajevo.ba" };
            string[] args = textBoxWeb.Text.Split('\n');
            
            //Console.WriteLine("Application  Started");
            //textBoxWords.AppendText("Application  Started");
            try
            {
                //create Urls and validate them
                var listUri = new List<Uri>();
                for (int i = 0; i < args.Length; i++)
                {
                    try
                    {
                        string url = args[i];
                        if (!url.StartsWith("http://", StringComparison.OrdinalIgnoreCase)) url = "http://" + url;
                        Uri uri;

                        if (Uri.TryCreate(url, UriKind.Absolute, out uri) || Uri.TryCreate("http://" + url, UriKind.Absolute, out uri))
                        {
                            listUri.Add(uri);
                        }
                    }
                    catch (UriFormatException ex)
                    {
                       // Console.WriteLine("Error {1} web site uri {0} has wrong format. {2}", args[i], i, e.Message);
                        textBoxWords.AppendText("Error "+i.ToString()+" web site uri: "+args[i]+ " has wrong format."+ ex.Message+"\n");
                    }
                }

                if (listUri.Count == 0) throw new Exception("No valid web urls. Check your command line params" + "\n");
               
                TopTenComponent.TopTen tt = new TopTenComponent.TopTen();

                //Subscribe to events 
                listBox1.Items.Clear();
                tt.HtmlDownloadedEvent += Tt_HtmlRemovedEvent;
                tt.HtmlRemovedEvent += Tt_HtmlRemovedEvent;
               // tt.HtmlRemovedEvent2 += Tt_HtmlRemovedEvent2;
               //tt.TopTenMadeEvent2 += Tt_HtmlRemovedEvent2;
                tt.WordCountCalculatedEvent += Tt_HtmlRemovedEvent;
                tt.TopTenMadeEvent += Tt_HtmlRemovedEvent;


                var res1 = tt.getHtmlFromUrls(listUri);
                var res2 = tt.removeHTMLs(res1);
                var res3 = tt.getWordListFromTexts(res2);
                var res4 = new List<Dictionary<String, int>>();
                foreach (var l in res3)
                {
                    res4.Add(tt.getWordCounts(l));
                }
                var res5 = tt.makeTop10s(res4);
                int counter = 0;
                foreach (var item in res5)
                {
                    counter++;
                    //Console.WriteLine("Word {0} appears {1} times", item.Key, item.Value);
                    textBoxWords.AppendText(counter.ToString()+". Word " + item.Key+ " appears " + item.Value.ToString() + " times\n");
                    if(counter==10) break;
                }
            }
            catch (NullReferenceException ex)
            {
                //Console.WriteLine(e.Message + " Error ");
                textBoxWords.AppendText(ex.Message + " Error ");
            }

            catch (WebException ex)
            {
                // No internet connection
                //Console.WriteLine(e.Message + " Error ");
                textBoxWords.AppendText(ex.Message + " Error ");
                

            }
            catch (Exception ex)
            {
                //Some "Generic" error
                //Console.WriteLine(e.Message + " Error ");
                textBoxWords.AppendText(ex.Message + " Error ");
            }
            finally
            {
                //Finally is executed ALWAYS
                //Greaat place to do clean up work
                //Console.WriteLine("Application Closing");
               // textBoxWords.AppendText("\n"+"Application Closing"+"\n");
            }


            
        }

        private bool Tt_HtmlRemovedEvent(string msg)
        {
            listBox1.Items.Add(msg);
            return true;
        }

        private bool Tt_HtmlRemovedEvent2(string msg, long val)
        {
            listBox1.Items.Add(string.Format("{0} - {1} ms", msg, val));
            return true;
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.ShowDialog();
        }
    }
}
