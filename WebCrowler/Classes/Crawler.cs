using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebCrawler.DbModels;
using WebCrowler.DbModels;

namespace WebCrowler.Classes
{
    class Crawler
    {
        //public HttpClient httpClient { get; set; }
        public List<GoogleResults> CGoogleResults { get; set; }

        public Crawler()
        {
           // var httpClient = new HttpClient();
            CGoogleResults = GoogleResults.GetResults(DateTime.Now);
        }


        public async void StartWebCrawling()
        {
            try
            {
                var httpClient = new HttpClient();
                using (APPContext context = new APPContext())
                {

                    foreach (var googleResult in CGoogleResults)
                    {
                        string url = "https://www.street.com.pl/"; //googleResult.link;

                        string menuLink = null;

                        var html = await httpClient.GetStringAsync(url);

                        string result = html.ToString();

                        var htmlDocument = new HtmlDocument();

                        htmlDocument.LoadHtml(html);
                        var nodes = htmlDocument.DocumentNode.SelectNodes(".//a[normalize-space(text())] | .//a//span[normalize-space(text())] | .//a//p[normalize-space(text())]").
                             Where(x => (String.Compare(x.InnerHtml, "menu", StringComparison.OrdinalIgnoreCase) == 0) && x.Attributes.Contains("href")).GroupBy(x => x.Attributes).FirstOrDefault().ToList();

                        if (nodes != null && nodes.Any(x => x.Attributes["href"].Value.Contains("https") || x.Attributes["href"].Value.Contains("http")))
                        {
                            foreach (var node in nodes)
                            {

                                menuLink = node.Attributes["href"].Value;


                                var menuHtml = await httpClient.GetStringAsync(menuLink);

                                htmlDocument.LoadHtml(menuHtml);
                                var ingredientsNodes = htmlDocument.DocumentNode.SelectNodes("//span[normalize-space(text())] | //h[normalize-space(text())] | //em[normalize-space(text())] | //p[normalize-space(text())]");

                                foreach (var ingNode in ingredientsNodes)
                                {
                                    //var nodeText = RemoveBetween(ingNode.InnerHtml, '<', '>');
                                    Console.WriteLine(ingNode.InnerHtml);
                                }

                            }
                        }
                        else
                        {
                            var fullHtmlNodes = htmlDocument.DocumentNode.SelectNodes("//span[normalize-space(text())] | //h3[normalize-space(text())] | //h2[normalize-space(text())] | //h4[normalize-space(text())] | //em[normalize-space(text())] | //p[normalize-space(text())] | //img");

                            //var imgNode = htmlDocument.DocumentNode.SelectNodes(".//img");

                            //foreach (var item in imgNode)
                            //{
                            //    var src = item.Attributes["src"].Value;
                            //}

                            foreach (var ingNode in fullHtmlNodes)
                            {
                                //var nodeText = RemoveBetween(ingNode.InnerHtml, '<', '>');
                                Console.WriteLine(ingNode.InnerHtml);
                            }
                        }
                    }

                   

                }
            }
            catch (Exception ex)
            {

                throw;
            }

         





        }

        string RemoveBetween(string s, char begin, char end)
        {
            Regex regex = new Regex(string.Format("\\{0}.*?\\{1}", begin, end));
            return regex.Replace(s, string.Empty);
        }



    }
}
