using System;
using System.Collections.Generic;

namespace TopTenApp
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			Console.WriteLine("Application  Started");

			TopTenComponent.TopTen tt = new TopTenComponent.TopTen();

            var res1 = tt.getHtmlFromUrls(
                new List<Uri> {
                    new Uri("http://klix.ba"),
                    new Uri("http://oslobodjenje.ba") });
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
