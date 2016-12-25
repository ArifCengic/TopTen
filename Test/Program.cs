using System;
using System.Collections.Generic;



namespace Test
{
	public class MainClass
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
			TopTenComponent.TopTen tt = new TopTenComponent.TopTen();
           

			Console.WriteLine("Starting Test");

			string testName;

			{
				testName = "TEST getHtmlFromUrls";
				var result = tt.getHtmlFromUrls(new List<Uri> { new Uri("http://www.klix.ba") });
				if (result[0].Length > 1000) passed(testName);
				else failed(testName);
			}

            {
                testName = "TEST getHtmlFromUrls 1";
        

               // Create new Test Case for getHtmlFromUrls when invalid Uri is passed 
               // when getHtmlFromUrls( new List {new Uri( " bad_uri "} ) is call with bad Url strings it should throw InvalidUriFormat exception
                try
                {
                    var result = tt.getHtmlFromUrls(new List<Uri> { new Uri("www.klix.ba") });
                    if (result[0].Length > 1000) passed(testName);
                    else failed(testName);
                  
                }
                catch (Exception e)
                {
                    
                    Console.WriteLine("Exception was thrown: " + e.Message);
                    failed(testName);

                }
               
               
            }
          


			{
				testName = "TEST removeHTMLs";
				var result = tt.removeHTMLs(new List<String> { "<h1> TEST </h2>" });

				if (result[0].Trim() == "TEST") passed(testName);
				else failed(testName);
			}

			{
				testName = "TEST getWordCounts";
				var result = tt.getWordCounts(new List<String> { "juce", "danas", "sutra", "danas" } );

				if (result["danas"] == 2) passed(testName);
				else failed(testName);
			}

			
				{
					testName = "TEST getWordListFromTexts";
					var result = tt.getWordListFromTexts(new List<String> { "juce,danas i sutra" }); ;
  					if (result[0].Count == 4) passed(testName);
					else failed(testName);
				}


				{
					testName = "TEST makeTop10s";
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

            // test Url without scheme
                {
                    testName = "TEST URLsWithoutScheme";
                    bool ok= false;
                            
                    try
                    {
                        string[] test_urls = new string[] { "www.klix.ba", "www.radiosarajevo.ba" };
                        TopTenApp.MainClass ttApp = new TopTenApp.MainClass();
                        TopTenApp.MainClass.Main(test_urls);
                        ok=true;
                 
                    }
                    catch(Exception e)
                    {
                        ok=false;
                    }
                    if (ok == true) passed(testName);
                    else failed(testName);
                }


                // test Url without scheme - fail
                {
                    testName = "TEST URLsWithoutScheme";
                    bool ok = false;
                    try
                    {
                        string[] test_urls = new string[] { "https://www.klix.ba", "ww.radiosarajevo.ba" };
                        TopTenApp.MainClass ttApp = new TopTenApp.MainClass();
                        TopTenApp.MainClass.Main(test_urls);
                        ok = true;
                    }
                    catch
                    {
                        ok = false;
                    }
                    if (ok == true) passed(testName);
                    else failed(testName);
                }
                Console.ReadLine();
		}
	}
}
