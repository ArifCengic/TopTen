using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Xml;
using System.Runtime.Serialization;
using System.Linq;

namespace TopTenApp
{
	public class MainClass
	{
        public static bool ShowMessage(string msg)
        {
            Console.WriteLine(msg);
            return true;
        }
        public static bool ShowMessageWithLong(string msg, long val)
        {
            Console.WriteLine(String.Format("Message: {0}, value {1}", msg, val));
            return true;
        }
        public static void Main(string[] args)
		{
			Console.WriteLine("Application  Started");
			//var logResults = LoadTopTen();
            try
            {
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
                    catch (UriFormatException e)
                    {
                        Console.WriteLine("Error {1} web site uri {0} has wrong format. {2}", args[i], i, e.Message);
                    }
                }

                if (listUri.Count == 0) throw new Exception("No valid web urls. Check your command line params");

                TopTenComponent.TopTen tt = new TopTenComponent.TopTen();
                tt.NamesOnly = false;
                tt.HtmlDownloaded = ShowMessage;
                tt.HtmlRemovedEvent += ShowMessage;
                tt.WordListCreated = ShowMessage;
                tt.WordCountsCalculated = ShowMessage;
                tt.WordCountsCalculated2 = ShowMessageWithLong;
                tt.TopTenMadeEvent += ShowMessage;
				tt.TopTenMadeEvent2 += ShowMessageWithLong;

                var res1 = tt.getHtmlFromUrls(listUri);
                var res2 = tt.removeHTMLs(res1);
                var res3 = tt.getWordListFromTexts(res2);
                var res4 = new List<Dictionary<String, int>>();
                foreach (var l in res3)
                {
                    res4.Add(tt.getWordCounts(l));
                }
                var res5 = tt.makeTop10s(res4);

                foreach (var item in res5)
                {
                    if (item.Value > 10) Console.WriteLine("Word {0} appears {1} times", item.Key, item.Value);
                }

				SaveTopTen(res5);
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e.Message + " Error ");
            }

            catch (WebException e)
            {
                // No internet connection
                Console.WriteLine(e.Message + " Error ");
                throw e;
              
            }
            catch (Exception e)
            {
                //Some "Generic" error
                Console.WriteLine(e.Message + " Error ");
            }
            finally
            {
                //Finally is executed ALWAYS
                //Greaat place to do clean up work
                Console.WriteLine("Application Closing");
                
            }


            Console.ReadLine();
		}

        /****
        private static bool ShowMessage(string msg)
        {
            throw new NotImplementedException();
        }
        ****/
        private static void SaveTopTen(Dictionary<string, int> histogram)
		{
			Dictionary<string, int> outputVals = new Dictionary<string, int>();
			int maxCount = 10;
			if (histogram.Count < maxCount)
			{
				maxCount = histogram.Count;
			}
			int count = 1;
			foreach (KeyValuePair<string, int> kvp in histogram)
			{
				outputVals.Add(kvp.Key, kvp.Value);
				count++;
				if (count > maxCount)
				{
					break;
				}
			}

            DateTime currentDT = DateTime.Now;
			string currentDTString = currentDT.ToString("yyyyMMdd_HHmmss") + ".xml";
			string histogramFileName = "TopTen" + currentDTString;
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.Indent = true;
			XmlWriter writer = XmlWriter.Create(histogramFileName, settings);
			DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<string, int>));
			serializer.WriteObject(writer, outputVals);
			writer.Close();
		}

		private static Dictionary<string, int> LoadTopTen()
		{
			string searchPattern = "TopTen*.xml";  // This would be for you to construct your prefix

			string path = Directory.GetCurrentDirectory();
			DirectoryInfo di = new DirectoryInfo(path);
			FileInfo[] files = di.GetFiles(searchPattern);
			Dictionary<string, int> inputVals;

			foreach (var file in files)
			{
				Console.WriteLine("Log data from " + file.Name);
				XmlReader reader = XmlReader.Create(file.FullName);
                DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<string, int>));
				inputVals = (Dictionary<string, int>)serializer.ReadObject(reader);

				foreach (KeyValuePair<string, int> kvp in inputVals)
				{
					Console.Write(kvp.Key + ":" + kvp.Value + " | ");
				}
				Console.WriteLine("");
                reader.Close();
			}
			Console.WriteLine("Those were historical data. Please press any key to continue the applicaiton");
			Console.ReadKey();

			return new Dictionary<string, int>();
		}
	}
}
