using System;
using System.Collections.Generic;
namespace TopTenApp
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			Console.WriteLine("Application  Started");
            var listURI = new List<Uri>();

            for(int i=0; i< args.Length; i++)
            {
                if (!args[i].StartsWith("http://"))
                { args[i] = "http://" + args[i]; }

                Uri u = new Uri(args[i]);
                listURI.Add(u);
            }
 //pre processor
#if DEBUG
            Console.WriteLine("Application has " + args.Length + " web sites" );
#endif

            TopTenComponent.TopTen tt = new TopTenComponent.TopTen();

            var res1 = tt.getHtmlFromUrls(listURI);
            var res2 = tt.removeHTMLs(res1);
            var res3 = tt.getWordListFromTexts(res2);
            var res4 = new List<Dictionary<String, int>>();
            foreach (var l in res3)
            {
                res4.Add( tt.getWordCounts(l));
            }
            var res5 = tt.makeTop10s(res4);

            foreach (var item in res5)
            {
                if (item.Value > 10) Console.WriteLine("Word {0} appears {1} times", item.Key, item.Value);
            }
            Console.ReadLine();
		}
	}
}
