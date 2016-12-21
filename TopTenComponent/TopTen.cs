using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using TopTen;
using System.Text.RegularExpressions;

namespace TopTenComponent
{
	public class TopTen : ITopTen
	{
        
        //all HTML markers are constrained with <>
        const string HTML_TAG_PATTERN = "<.*?>";
       public List<string> blackList;

        public List<String> getHtmlFromUrls(List<Uri> webSites)
		{
			const string noHTML = "NO_HTML";

			List<String> htmlList = new List<String>();

			foreach (Uri webURI in webSites)
			{
				WebClient wc = new WebClient();
				string html = wc.DownloadString(webURI);
				htmlList.Add(html);
			}
			return htmlList;
		}

		public List<String> removeHTMLs(List<string> htmls)
		{

			//TODO Sinisa
			//TEst case Senad
			// Uzeti listu htmls remove all tags
			// if "NO_HTML" return empty string
            List<String> withoutHTMLs = new List<String> ();
            foreach(String s in htmls)
            {
                   String a = Regex.Replace(s, HTML_TAG_PATTERN, string.Empty);
                   withoutHTMLs.Add(a);
            }
           
			return withoutHTMLs;
		}

		public List<List<string>> getWordListFromTexts(List<string> texts)
		{
                      
                //TODO Senad
                //TestCase Denis
                // Uzeti text pretvoriti ga u listu rijeci
                // Kreirati metod koji ce Izbrisati rijeci krace od 3 slova
                // Kreirati metod koji ce izbaciti rijeci koje sadrze broj

            List<List<String>> rezultat = new List<List<string>>();
            foreach(String text in texts)
            {
                List<string> words = new List<string>(
                           text.Split(new char[] { ' ', ',', '.', ':', '\t' },
                           StringSplitOptions.RemoveEmptyEntries));

                /*
                 * foreach (String word in words)
                {
                    if (word.Length < 3)
                    {
                        words.Remove(word);
                    }
                }
                */
                words.RemoveAll(x => x.Length < 3);

                rezultat.Add(words);
            }



            return rezultat;
		}



		public bool test_getWordListFromTexts()
		{
			var testData = new List<String> { "Danas", "je", "lijep", "dan" };
			var methodData = getWordListFromTexts(new List<string> { "Danas je lijep dan" })[0];

			return testData.SequenceEqual(methodData);

			/*
			 *  Manual comparing
			int wordCount = testData.Count;
			if (wordCount != methodData.Count)
			{
				return false;
			}
			for (int i = 0; i < wordCount; i++)
			{
				if ( testData[i] != methodData[i])
				{
					return false;
				}
			}
			return true; ;
			*/

		}

		public Dictionary<string, int> getWordCounts(List<string> rijeci)
		{
			/*
			 * v1 code
			Dictionary<string, int> results = new Dictionary<string, int>();
			foreach (string s in rijeci)
			{
				if (results.ContainsKey(s))
				{
					results[s] += 1;
				}
				else
				{
					results[s] = 1;
				}            
			}
			*/

			// should be case-insensitive now
			var results = new Dictionary<string, int>(StringComparer.CurrentCultureIgnoreCase);
			foreach (string s in rijeci)
			{

				if (blackList.Exists(elem => elem == s))
					continue;
				int currentCount = 0;
				//  TryGetValue is more performant than ContainsKey followed by item access
				results.TryGetValue(s, out currentCount);
				currentCount++;
				results[s] = currentCount;
			}

			//TODO Denis
			//TODO TestCases DZenita
			// return new Dictionary<String, int> { { "Danas", 2 }, { "dan", 1 } };
			return results;
		}
		public Dictionary<String, int> makeTop10s(List<Dictionary<String, int>> top10Liste)
		{
			// TODO Dzenita, weighted average
			// TODO TEsttCase Adin
			return top10Liste[0];
		}
    } 
}