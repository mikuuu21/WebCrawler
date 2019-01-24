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
        public List<GoogleResults> CGoogleResults { get; }
        public string ConditionsQuery { get; }
        public string restauranNamePattern { get; } = @"\.([^\.]+)\.";
        //public string postalCodePatter { get;} = "^[0-9]{2}-[0-9]{3}$";
        //public string streetPattern { get; } = @"^(ul\.)[(a-z)]+\s(\d)*$";

        public Crawler()
        {
            // var httpClient = new HttpClient();
            CGoogleResults = GoogleResults.GetResults(DateTime.Now);
            ConditionsQuery = IngredientsBasis.CreateQuery();

        }


        public async void StartWebCrawling()
        {
            try
            {
                var httpClient = new HttpClient();
                using (APPContext context = new APPContext())
                {
                    List<Ingredients> ingredients = new List<Ingredients>();
                    List<Nodes> dBNodes = new List<Nodes>();
                    
                    string restaurantName = null;

                    foreach (var googleResult in CGoogleResults)
                    {

                        Restaurants restaurant = new Restaurants() { City = "Warszawa" };

                        string url = "https://www.street.com.pl/"; //googleResult.link;

                        restaurantName = Regex.Match(url, restauranNamePattern).ToString().Trim('.');
                        restaurant.RestaurantName = restaurantName;

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

                                AddNodesToDb(dBNodes, htmlDocument, restaurant);

                            }
                        }
                        else
                        {


                            AddNodesToDb(dBNodes, htmlDocument, restaurant);



                        }

                        context.Nodes.AddRange(dBNodes);
                        context.SaveChanges();

                        var matchedIngredients = Nodes.RunStoredProcedure(ConditionsQuery);

                        context.Database.ExecuteSqlCommand("TRUNCATE TABLE [Nodes]");
                        //context.Database.ExecuteSqlCommand("TRUNCATE TABLE [Ingredients]");
                        //context.Database.ExecuteSqlCommand("TRUNCATE TABLE [Restaurants]");
                        //context.Database.ExecuteSqlCommand("TRUNCATE TABLE [RestaurantsIngredients]");


                        restaurant.Ingredients = Ingredients.ConvertToIngredientsList(matchedIngredients);

                        context.Restaurant.Add(restaurant);
                        context.SaveChanges();



                    }



                }
            }
            catch (Exception ex)
            {

                throw;
            }






        }

        public void AddNodesToDb(List<Nodes> dBNodes, HtmlDocument htmlDocument, Restaurants restaurant)
        {

            var fullHtmlNodes = htmlDocument.DocumentNode.SelectNodes("//span[normalize-space(text())] | //h3[normalize-space(text())] | //h2[normalize-space(text())] | //h4[normalize-space(text())] | //em[normalize-space(text())] | //p[normalize-space(text())] | //img");
            var matchPostalCodeRegex = true;
            var matchAddressRegex = true;
            string postalCode = null;
            string address = null;

            foreach (var ingNode in fullHtmlNodes)
            {



                var cleanWords = Regex.Replace(ingNode.InnerHtml, @"\t|\n|\r", "");
                var splittedWords = Regex.Split(ingNode.InnerText, ",");
                var words = splittedWords
                    .Where(x => !x.Contains("&nbsp;") && !string.IsNullOrEmpty(x.Trim()))
                         .ToList();

                foreach (var item in words)
                {
                    if (matchAddressRegex == true)
                    {
                        address = Regex.Match(item, @"(ul\.)([A-z a-z])+\s(\d)*").ToString();
                        if (!string.IsNullOrWhiteSpace(address))
                        {
                            restaurant.Address = address;
                            matchAddressRegex = false;
                        }
                        
                    }
                    if (matchPostalCodeRegex == true)
                    {
                        postalCode = Regex.Match(item, "[0-9]{2}-[0-9]{3}").ToString();
                        if (!string.IsNullOrWhiteSpace(postalCode))
                        {
                            restaurant.PostalCode = postalCode;
                            matchPostalCodeRegex = false;
                        }
                    }
                    Nodes dBNode = new Nodes();
                    dBNode.node = item;
                    dBNodes.Add(dBNode);
                    Console.WriteLine(item);
                }
            }


        }
    }
}
