using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using HtmlAgilityPack;

namespace _2chparse
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
                string link = node.SelectSingleNode("div/a").Attributes["href"].Value;
                Console.WriteLine("--------" + link);
                Console.WriteLine("Extention ---- " + link.Substring(link.Length - 4));
                _webClient.DownloadFile("https://2ch.pm" + link, "Image " + i + link.Substring(link.Length - 4));
                i++;
            }
        }



        static void Main()
        {
            imgLinks = new List<string>();
            string remoteURI = "https://2ch.pm/pr/res/890393.html";
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
