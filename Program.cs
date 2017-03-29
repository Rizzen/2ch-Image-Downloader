using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.ComponentModel;
using System.IO;
using System.Threading;
using HtmlAgilityPack;

namespace DvachImages
{
    class Program
    {

        //TODO POOL OF THREADS (Threading.ThreadPool)

        public static List<String> GetImageLinks(HtmlDocument htmlDoc)
        {
            var imageLinks = new List<String>();
            foreach (HtmlNode node in htmlDoc.DocumentNode.SelectNodes("//*[@class='image']"))
            {
                string link = node.SelectSingleNode("div/a").Attributes["href"].Value;
                imageLinks.Add($"http://2ch.hk{link}");
                
            }
            
            return imageLinks;
        }

        public static async void DownloadImageAsync(string link)
        {
            string format = Path.GetExtension(link);
            var wc = new WebClient();

            wc.DownloadFileCompleted += DownloadFileCompleted;
            Console.WriteLine(link);
            await wc.DownloadFileTaskAsync(link, $"Image {DateTimeOffset.Now.ToUnixTimeMilliseconds()+wc.GetHashCode()}{format}");
            
        }

        public static void GetAllImages(HtmlDocument htmlDoc)
        {
            List<string> imgLinks = new List<string>(); ;
            imgLinks = GetImageLinks(htmlDoc);
            foreach (string link in imgLinks)
            {
                DownloadImageAsync(link);
            }
            Console.WriteLine("...............Done.............\n");

        }

        static void Main()
        {
            const string REMOTE_URI = "https://2ch.hk/v/res/1908508.html";
            var doc = new HtmlDocument();
            var _webClient = new WebClient();

            Console.WriteLine("...............Request.............\n");
            Console.WriteLine($"Processors count: {Environment.ProcessorCount}");
           
            doc.LoadHtml(_webClient.DownloadString(REMOTE_URI));
            GetAllImages(doc);
            
            Console.ReadLine();
        }

        static void DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            Console.WriteLine($"Downloaded image in thread {Thread.CurrentThread.ManagedThreadId}");
        }
    }
}

