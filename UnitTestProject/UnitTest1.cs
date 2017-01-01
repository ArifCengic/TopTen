using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TopTenComponent;
using System.Collections.Generic;
//using NUnit.Framework;
using System.Net;

namespace Test
{
    namespace UnitTestProject
{
        [TestClass]
  
       // [TestFixture]
        public class UntiTests
        {

            TopTenComponent.TopTen tt = new TopTenComponent.TopTen();


            [TestMethod]
            void Test_getHtmlFromUrls()
            {
                var result = tt.getHtmlFromUrls(new List<Uri> { new Uri("http://www.klix.ba") });
                Assert.AreEqual(result[0].Length > 1000, true);
            }

            /****
            [Test]
            void Test_getHtmlFromUrls1()
            {
                // Create new Test Case for getHtmlFromUrls when invalid Uri is passed 
                // when getHtmlFromUrls( new List {new Uri( " bad_uri "} ) is call with bad Url strings it should throw InvalidUriFormat exception
                try
                {
                    var result = tt.getHtmlFromUrls(new List<Uri> { new Uri("www.klix.ba") });
                    //no exception was thrown

                }
                catch (Exception e)
                {

                    //Console.WriteLine("Exception was thrown: " + e.Message);
                    //exception was thrown - test is successfull
                    //TODO check Right Exception type is thrown
                    NUnit.Framework.Assert.AreEqual(e.GetType(), typeof(WebException));

                }


            }




            [Test]
            void Test_removeHTMLs()
            {
                var result = tt.removeHTMLs(new List<String> { "<h1> TEST </h2>" });
                NUnit.Framework.Assert.AreEqual(result[0].Trim(), "TEST");
            }


            [Test]
            void Test_getWordCounts()
            {
                var result = tt.getWordCounts(new List<String> { "juce", "danas", "sutra", "danas" });
                NUnit.Framework.Assert.AreEqual(result["danas"], 2);
            }



            [Test]
            void Test_getWordListFromTexts()
            {
                var result = tt.getWordListFromTexts(new List<String> { "juce,danas i sutra" }); ;
                NUnit.Framework.Assert.AreEqual(result[0].Count, 4);
            }



            [Test]
            void Test_makeTop10s()
            {
                Dictionary<string, int> d1 = new Dictionary<string, int>();
                d1.Add("danas", 2);
                d1.Add("sutra", 1);
                Dictionary<string, int> d2 = new Dictionary<string, int>() {
                            { "danas", 5 },
                            { "juce", 3 }
                        };

                var result = tt.makeTop10s(new List<Dictionary<string, int>>
                        { d1, d2 });

                NUnit.Framework.Assert.AreEqual(result["danas"], 7);
            }

            // test Url without scheme

            [Test]
            void Test_URLsWithoutScheme()
            {
                bool ok = false;

                try
                {
                    string[] test_urls = new string[] { "www.klix.ba", "www.radiosarajevo.ba" };
                   // TopTenApp.MainClass ttApp = new TopTenApp.MainClass();
                  //  TopTenApp.MainClass.Main(test_urls);
                    ok = true;

                }
                catch (Exception e)
                {
                    ok = false;
                }
                NUnit.Framework.Assert.AreEqual(ok, true);

            }


            [Test]
            void Test_URLsWithoutScheme1()
            {
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
                NUnit.Framework.Assert.AreEqual(ok, true);
            }
            ***/
        }
    }




}
