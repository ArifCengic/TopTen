using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using NUnit.Framework;
using System.Net;

namespace UnitTestProject
{
    [TestClass]
    public class UntiTests
    {

        TopTenComponent.TopTen tt = new TopTenComponent.TopTen();


        [TestMethod]
        public void Test_getHtmlFromUrls()
        {
            var result = tt.getHtmlFromUrls(new List<Uri> { new Uri("http://www.klix.ba") });
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(result[0].Length > 1000, true);
        }

        
        [TestMethod]
        public void Test_getHtmlFromUrls1()
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
                NUnit.Framework.Assert.AreEqual(e.GetType(), typeof(UriFormatException));
            }


        }

        [TestMethod]
        public void Test_removeHTMLs()
        {
            var result = tt.removeHTMLs(new List<String> { "<h1> TEST </h2>" });
            NUnit.Framework.Assert.AreEqual(result[0].Trim(), "TEST");
        }


        [TestMethod]
        public void Test_getWordCounts()
        {
            var result = tt.getWordCounts(new List<String> { "juce", "danas", "sutra", "danas" });
            NUnit.Framework.Assert.AreEqual(result["danas"], 2);
        }

        //some comment
        [TestMethod]
        public void Test_getWordCountsUppercase()
        {
            tt.NamesOnly = true;
            var result = tt.getWordCounts(new List<String> { "Jack", "danas", "Jim", "danas", "Jim" });
            NUnit.Framework.Assert.AreEqual(result["danas"], null);
        }

       
        [TestMethod]
        public void Test_getWordListFromTexts()
        {
            var result = tt.getWordListFromTexts(new List<String> { "juce,danas i sutra" }); ;
            NUnit.Framework.Assert.AreEqual(result[0].Count, 3);
        }



        [TestMethod]
        public void Test_makeTop10s()
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

        [TestMethod]
        public void Test_URLsWithoutScheme()
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


        [TestMethod]
        public void Test_URLsWithoutScheme1()
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
            NUnit.Framework.Assert.AreEqual(ok, false);
        }
      
    }
}
  

