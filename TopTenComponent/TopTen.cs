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
        public int minValidWordLenght = 3;
        public List<string> blackList = new List<string>(new string[]{"a", "o", "i", "u", "ako", "zbog",
        "ja", "mi", "moj", "naš", "ovaj", "ovakav", "ko", "koji", "neko", "nekakav", "niko", "ničiji",
            "svako","od", "do", "k", "uz",  "ili", "jer", "ali", "ili", "te", });
        public char[] delimiters = new char[] { ' ', ',', '.', ':', '\t', '\\', '=', '/', '\r', '\n', '{', '}', '[', ']' };
        public bool NamesOnly = false;
        public int limitWordsPerWebSite = 20;       

        public Func<string, bool> HtmlDownloaded;

        public delegate bool HtmlRemoved(string msg);
        public event HtmlRemoved HtmlRemovedEvent;

        public Func<string, bool> WordListCreated;
        public Func<string, bool> WordCountsCalculated;
        public Func<string, long, bool> WordCountsCalculated2;
        public delegate bool TopTenMade(string msg);
        public event TopTenMade TopTenMadeEvent;

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
            foreach (String s in htmls)
            {
                String a = Regex.Replace(s, HTML_TAG_PATTERN, string.Empty);
                withoutHTMLs.Add(a);
            }

            if (HtmlRemovedEvent != null) HtmlRemovedEvent(String.Format("All HTML tags removed. {0} script tags removed", 7));
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
            foreach (String text in texts)
            {
                List<string> words = new List<string>(
                           text.Split(delimiters,
                           StringSplitOptions.RemoveEmptyEntries));


                //TODO put back in
                words.RemoveAll(x => x.Length < minValidWordLenght);
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


            if (WordListCreated != null) WordListCreated("Word List Created");
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
            var watch = System.Diagnostics.Stopwatch.StartNew();

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

                if (blackList != null && blackList.Exists(elem => elem == s))
                {
                    continue;
                }

                if ( string.IsNullOrEmpty(s) )
                {
                    continue;
                }

                if (NamesOnly && !s.startsWithUppercase())
                {
                    continue;
                }

                int currentCount = 0;
				//  TryGetValue is more performant than ContainsKey followed by item access
				results.TryGetValue(s, out currentCount);
				currentCount++;
				results[s] = currentCount;
			}

            //TODO Denis
            //TODO TestCases DZenita
            // return new Dictionary<String, int> { { "Danas", 2 }, { "dan", 1 } };
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            if (WordCountsCalculated != null) WordCountsCalculated(String.Format("Word Counts were calculated in {0} milliseconds.", elapsedMs));
            if (WordCountsCalculated2 != null) WordCountsCalculated2("Word Counts were calculated.", elapsedMs);
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

           Dictionary<String, int> resultNew = new Dictionary<String, int>();

           foreach (KeyValuePair<String, int> pair in result.OrderByDescending(key => key.Value))
            {
                resultNew.Add(pair.Key, pair.Value);
            }

            if (TopTenMadeEvent != null) TopTenMadeEvent("Top Ten Lists created!");
            return resultNew;
           


        }
    } 
}