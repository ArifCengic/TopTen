using System;
using System.Collections.Generic;


namespace Test
{
	class MainClass
	{
		static void passed(string testName)
		{
			Console.WriteLine(testName + " OK ");
		}

		static void failed(string testName)
		{
			Console.WriteLine(testName + " FAILED ");

		}

		public static void Main(string[] args)
		{
			
			Console.WriteLine("Starting Test");

			string testName;
            
			{
                TopTenComponent.TopTen tt = new TopTenComponent.TopTen();
                tt.blackList = new List<string> { "sutra", "danas" };

                testName = "TEST getHtmlFromUrls";
				var result = tt.getHtmlFromUrls(new List<Uri> { new Uri("http://www.klix.ba") });
				if (result[0].Length > 1000) passed(testName);
				else failed(testName);
			}

			{
				testName = "TEST removeHTMLs";
				var result = tt.removeHTMLs(new List<String> { "<h1> TEST </h2>" });

				if (result[0].Trim() == "TEST") passed(testName);
				else failed(testName);
			}

			{
                TopTenComponent.TopTen tt = new TopTenComponent.TopTen();
                tt.blackList = new List<string> { "sutra", "danas" };

                testName = "TEST getWordCounts";
				var result = tt.getWordCounts(new List<String> { "juce", "danas", "sutra", "danas" } );

				if (result["danas"] == 2) passed(testName);
				else failed(testName);
			}

			
				{
                    TopTenComponent.TopTen tt = new TopTenComponent.TopTen();
                    tt.blackList = new List<string> { "sutra", "danas" };

                    testName = "TEST getWordListFromTexts";
					var result = tt.getWordListFromTexts(new List<String> { "juce,danas i sutra" }); ;

					if (result[0].Count == 4) passed(testName);
					else failed(testName);
				}


				{
                TopTenComponent.TopTen tt = new TopTenComponent.TopTen();
                tt.blackList = new List<string> { "sutra", "danas" };

                testName = "TEST getWordListFromTexts";
					Dictionary<string, int> d1 = new Dictionary<string, int>();
					d1.Add("danas", 2);
					d1.Add("sutra", 1);
					Dictionary<string, int> d2 = new Dictionary<string, int>() {
							{ "danas", 5 },
							{ "juce", 3 }
						};

					var result = tt.makeTop10s(new List<Dictionary<string, int>>
						{ d1, d2 });

					if (result["danas"] == 7) passed(testName);
					else failed(testName);
				}


			}


		}
	}

