using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebCrawler.DbModels;
using WebCrowler.DbModels;

namespace WebCrowler.Classes
{
    class IngredientsStore
    {


        public static async void GetBasicIngredients()
        {
            try
            {
                using (APPContext context = new APPContext())
                {
                    List<IngredientsBasis> basicIngredientsList = new List<IngredientsBasis>();
                     await GetIngredientsFromFirsSite(basicIngredientsList);
                     await GetIngredientsFromSecondSite(basicIngredientsList);

                    var distinctResults = basicIngredientsList.GroupBy(x => x.Name).Select(x => x.First()).ToList();
                    context.IngredientsBasis.AddRange(distinctResults);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw;
            }


        }

        private static async Task GetIngredientsFromFirsSite(List<IngredientsBasis> basicIngredientsList)
        {
            string url = "https://gotujmy.pl/skladniki-kulinarne.html";
            var httpClient = new HttpClient();


            var html = await httpClient.GetStringAsync(url);
            string result = html.ToString();


            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            var nodes = htmlDocument.DocumentNode.SelectNodes("//div//a[normalize-space(text())]");

            for (int i = 41; i < nodes.Count() - 13; i++)
            {
                IngredientsBasis Ingredient = new IngredientsBasis();
                Ingredient.Name = nodes[i].InnerHtml;

                basicIngredientsList.Add(Ingredient);
            }


        }

        private  static async Task GetIngredientsFromSecondSite(List<IngredientsBasis> basicIngredientsList)
        {
            string url = "https://www.dorotakaminska.pl/indeks-skladnikow/";
            var httpClient = new HttpClient();

            var html = await httpClient.GetStringAsync(url);

            string result = html.ToString();


            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            var nodes = htmlDocument.DocumentNode.SelectNodes("//p//a[normalize-space(text())]");
            for (int i = 2; i < nodes.Count() - 4; i++)
            {
                if (nodes[i].InnerHtml.Contains("<img"))
                {
                    continue;
                }
                IngredientsBasis Ingredient = new IngredientsBasis();
                Ingredient.Name = nodes[i].InnerHtml;

                basicIngredientsList.Add(Ingredient);
            }
        }

    }
}
