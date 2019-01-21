using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Net.Http;
using Google.Apis.Services;
using Google.Apis.Customsearch.v1.Data;
using Google.Apis.Customsearch.v1;
using WebCrowler.DbModels;
using WebCrowler.Classes;
using HtmlAgilityPack;
using System.IO;
using System.Drawing;


namespace WebCrawler
{
    class Program
    {
        static void Main(string[] args)
        {
            //https://stackoverflow.com/questions/26958829/how-do-i-use-the-new-httpclient-from-windows-web-http-to-download-an-image pobranie obrazka za pomoc¹ HttpClient
            StartWebCrawling();


            //string url = "";
            //WebClient web = new WebClient();
            //string site = web.DownloadString(url);

            //GoogleSearch search = new GoogleSearch();
            //Console.WriteLine("WprowadŸ zapytanie");
            //string query = Console.ReadLine();
            //if (!string.IsNullOrWhiteSpace(query))
            //{
            //    search.SearchForSites(query);
            //}
            Console.ReadLine();

        }

        private static async void StartWebCrawling()
        {
            string url = "http://www.momu.pl/menu.html";
            string menuLink = null;
            var httpClient = new HttpClient();

            var html = await httpClient.GetStringAsync(url);

            string result = html.ToString();


            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);
            var nodes = htmlDocument.DocumentNode.SelectNodes(".//a[normalize-space(text())] | .//a//span[normalize-space(text())] | .//a//p[normalize-space(text())]").
                 Where(x => (String.Compare(x.InnerHtml, "menu", StringComparison.OrdinalIgnoreCase) == 0) && x.Attributes.Contains("href")).ToList();

            if (nodes != null && nodes.Any(x => x.Attributes["href"].Value.Contains("https") || x.Attributes["href"].Value.Contains("http")))
            {
                // var link = nodes.Where(x => x.Attributes["href"].Value.ToUpper().Contains("menu".ToUpper()));
                foreach (var node in nodes)
                {

                    menuLink = node.Attributes["href"].Value;


                    var menuHtml = await httpClient.GetStringAsync(menuLink);

                    htmlDocument.LoadHtml(menuHtml);
                    var ingredientsNodes = htmlDocument.DocumentNode.SelectNodes("//span[normalize-space(text())] | //h[normalize-space(text())] | //em[normalize-space(text())] | //p[normalize-space(text())]");

                    foreach (var ingNode in ingredientsNodes)
                    {
                        Console.WriteLine(ingNode.InnerHtml);
                    }

                }
            }
            else
            {
                var fullHtmlNodes = htmlDocument.DocumentNode.SelectNodes("//span[normalize-space(text())] | //h[normalize-space(text())] | //em[normalize-space(text())] | //p[normalize-space(text())] | //img");

                var imgNode = htmlDocument.DocumentNode.SelectNodes(".//img");

                GetImages(httpClient);

                foreach (var item in imgNode)
                {
                    var src = item.Attributes["src"].Value;
                }

                foreach (var ingNode in fullHtmlNodes)
                {
                    Console.WriteLine(ingNode.InnerHtml);
                }
            }




        }

        private static async void GetImages( HttpClient client)
        {


            Bitmap bitmapImage;

            try
            {

                string uri = "static/menu/kuchnia_Strona_1.jpg";
                using (var response = await client.GetAsync(uri))
                {
                    response.EnsureSuccessStatusCode();

                    using (Stream inputStream = await response.Content.ReadAsStreamAsync()) 
                    {
                        bitmapImage = new Bitmap(inputStream);
                    }
                }


            }
            catch (Exception)
            {

                throw;
            }
        }


    }
}

