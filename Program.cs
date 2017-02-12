using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using HtmlAgilityPack;

namespace DvachImages
{
    class Program
    {
        public static List<string> imgLinks;
        public static WebClient _webClient;

        public static void GetImages(HtmlDocument htmlDoc)
        {
            Console.WriteLine(htmlDoc.DocumentNode.SelectNodes("//*[@class='image']").Count());
            int i = 0;
            
            foreach (HtmlNode node in htmlDoc.DocumentNode.SelectNodes("//*[@class='image']"))
            {
                string format;
                string link = node.SelectSingleNode("div/a").Attributes["href"].Value;
                Console.WriteLine("--------" + link);

                using (WebResponse response = WebRequest.Create(String.Format("https://2ch.hk{0}", link)).GetResponse())
                {
                    format = response.Headers.GetValues("Content-Type")[0].Split(new char[] { '/' }).Last();
                    Console.WriteLine("\n------- " + format + " --------\n");
                }

                _webClient.DownloadFile("https://2ch.hk" + link, "Image " + i + "." + format);
                i++;
            }
        }



        static void Main()
        {
            imgLinks = new List<string>();
            string remoteURI = "https://2ch.hk/v/res/1765234.html";
            HtmlDocument doc = new HtmlDocument();
            _webClient = new WebClient();
            _webClient.Encoding = System.Text.Encoding.GetEncoding("utf-8");

            Console.WriteLine("...............Request.............\n");
            doc.LoadHtml(_webClient.DownloadString(remoteURI));
            GetImages(doc);
            Console.WriteLine("----------------Done---------------");

            Console.ReadLine();
        }
    }
}
