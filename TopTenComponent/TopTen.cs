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
        const int minlen = 3;
        public List<string> blackList = new List<string>(new string[]{"a", "o", "i", "u", "ako", "zbog",
        "ja", "mi", "moj", "naš", "ovaj", "ovakav", "ko", "koji", "neko", "nekakav", "niko", "ničiji",
            "svako","od", "do", "k", "uz",  "ili", "jer", "ali", "ili", "te", });
        public char[] delimiters = new char[] { ' ', ',', '.', ':', '\t', '\\', '=', '/', '\r', '\n', '{', '}', '[', ']' };
        public bool NamesOnly = false;

        public Func<string, bool> HtmlDownloaded;

        public delegate bool HtmlRemoved(string msg);
        public event HtmlRemoved HtmlRemovedEvent;

        public List<String> getHtmlFromUrls(List<Uri> webSites)
        {
            const string noHTML = "NO_HTML";

            List<String> htmlList = new List<String>();

            foreach (Uri webURI in webSites)
            {
                try
                {
                    WebClient wc = new WebClient();
                    string html = wc.DownloadString(webURI);
                    if(String.IsNullOrEmpty(html)) htmlList.Add(noHTML);
                    else htmlList.Add(html);
                }
                catch (Exception e)
                {
                    throw new System.UriFormatException("InvalidUriFormat :" + e.Message);
                }
            }
            if (HtmlDownloaded != null) HtmlDownloaded("Downloaded some web sites");
            return htmlList;
        }

        public List<String> removeHTMLs(List<string> htmls)
        {
            // Uzeti listu htmls remove all tags
            // if "NO_HTML" return empty string
            List<String> withoutHTMLs = new List<String>();
            int count = 0;
            foreach (String s in htmls)
            {
                String a = Regex.Replace(s, HTML_TAG_PATTERN, string.Empty);
                withoutHTMLs.Add(a);
                if (HtmlRemovedEvent != null) HtmlRemovedEvent(String.Format("All HTML tags removed for {0} web site ", ++count));
            }

            return withoutHTMLs;
        }

        public List<List<string>> getWordListFromTexts(List<string> texts)
        {
            List<List<String>> rezultat = new List<List<string>>();
            foreach (String text in texts)
            {
                List<string> words = new List<string>(
                           text.Split(delimiters,
                           StringSplitOptions.RemoveEmptyEntries));

                //TO DO Add StartsWithBlackList List<string>
                words.RemoveAll(x => x.Length < minlen);
                words.RemoveAll(x => x.StartsWith("google"));
                words.RemoveAll(x => x.StartsWith("var"));
                words.RemoveAll(x => x.StartsWith("this"));
                words.RemoveAll(x => x.StartsWith("func"));
                words.RemoveAll(x => x.StartsWith("min"));
                words.RemoveAll(x => x.StartsWith("win"));
                words.RemoveAll(x => x.StartsWith("exp"));
                words.RemoveAll(x => x.StartsWith("doc"));
                words.RemoveAll(x => x.StartsWith("perf"));
                rezultat.Add(words);
            }

            return rezultat;
		}


		public Dictionary<string, int> getWordCounts(List<string> rijeci)
		{
			
			// should be case-insensitive now
			var results = new Dictionary<string, int>(StringComparer.CurrentCultureIgnoreCase);
			foreach (string s in rijeci)
			{

				if (blackList != null && blackList.Exists(elem => elem == s))
					continue;
				int currentCount = 0;
				//  TryGetValue is more performant than ContainsKey followed by item access
				results.TryGetValue(s, out currentCount);
				currentCount++;
				results[s] = currentCount;
			}

			return results;
		}
		public Dictionary<String, int> makeTop10s(List<Dictionary<String, int>> top10Liste)
		{

            var result = new Dictionary<String, int>();

            foreach (var dictionary in top10Liste)
            {
                foreach (var item in dictionary)
                {
                    if (result.ContainsKey(item.Key)) result[item.Key] += item.Value;
                    else result.Add(item.Key, item.Value);
                }
            }
           
           //Sort Dictionary
           Dictionary<String, int> resultNew = new Dictionary<String, int>();

           foreach (KeyValuePair<String, int> pair in result.OrderByDescending(key => key.Value))
            {
                resultNew.Add(pair.Key, pair.Value);
            }

            return resultNew;
        }
    } 
}