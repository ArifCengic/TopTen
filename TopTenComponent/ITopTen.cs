using System;
using System.Collections.Generic;

namespace TopTen
{
	/// <summary>
	/// Gets Most Frequent words from list of Web sites
	/// </summary>
	public interface ITopTen
	{
		/// <summary>
		/// Take list of webSite URI & download HTML
		/// if Web site is not available put "NO_HTML"
		/// <summary>
		/// <param name="webSites"> List of webSites URI to download from</param>

		List<String> getHtmlFromUrls(List<Uri> webSites);
		//TODO Sinisa
		//TODO Test case Senad

		List<String> removeHTMLs(List<string> htmls);

			// Uzeti listu htmls remove all tags
			// if "NO_HTML" return empty string
			// Uzeti text pretvoriti ga u listu rijeci
			// Kreirati metod koji ce Izbrisati rijeci krace od 3 slova
			// Kreirati metod koji ce izbaciti rijeci koje sadrze broj
	 	List<List<string>> getWordListFromTexts(List<string> texts);
		//TODO Senad
		//TestCase Denis

		Dictionary<String, int> getWordCounts(List<string> rijeci);
			//TODO Denis
			//TODO TestCases DZenita

		Dictionary<String, int> makeTop10s(List<Dictionary<String, int>> top10Liste);
			//TODO Dzenita, weighted average
			//TODO TesttCase Adin

	}
}
