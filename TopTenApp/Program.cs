using System;
using System.Collections.Generic;
using System.Net;

namespace TopTenApp
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			Console.WriteLine("Application  Started");
            try
            {
                var listUri = new List<Uri>();

                for (int i = 0; i < args.Length; i++)
                {
                    try
                    {
                        Uri u = new Uri(args[i]);
                        listUri.Add(u);
                    }
                    catch (UriFormatException e)
                    {
                        Console.WriteLine("Error {1} web site uri {0} has wrong format. {2}", args[i], i, e.Message);
                    }
                }

                if (listUri.Count == 0) throw new Exception("No valid web urls. Check your command line params");

                TopTenComponent.TopTen tt = new TopTenComponent.TopTen();

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
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e.Message + " Error ");
            }

            catch (WebException e)
            {
                // No internet connection
                Console.WriteLine(e.Message + " Error ");
              
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
	}
}
