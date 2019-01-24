using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebCrawler.DbModels;

namespace WebCrowler.DbModels
{
    class IngredientsBasis
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public static List<string> CreateConditionsQueries()
        {

            string firstQuery = null;
            string secondQuery = null;
            string thirdQuery = null;
            int lenght;
            List<string> result;
            Regex regex = new Regex("(\\s+(OR)\\s*)$");
            StringBuilder query = new StringBuilder();
            


            using (APPContext context = new APPContext())
            {

                IList<IngredientsBasis> ingredients = context.IngredientsBasis.ToList();

                for (int i = 0; i < 163; i++)
                {
                    query.Append($"FORMSOF(INFLECTIONAL, \"{ingredients[i].Name}\") OR ");
                }

                firstQuery = regex.Replace(query.ToString(), "");
                lenght = query.Length;
                query.Remove(0, lenght);

                for (int i = 163; i < 326; i++)
                {
                    query.Append($"FORMSOF(INFLECTIONAL, \"{ingredients[i].Name}\") OR ");
                }

                secondQuery = regex.Replace(query.ToString(), "");
                lenght = query.Length;
                query.Remove(0, lenght);


                for (int i = 326; i < ingredients.Count(); i++)
                {
                    query.Append($"FORMSOF(INFLECTIONAL, \"{ingredients[i].Name}\") OR ");
                }

                thirdQuery = regex.Replace(query.ToString(),"");

                return result = new List<string> { firstQuery, secondQuery, thirdQuery };
            }

        }

        public static string CreateQuery()
        {

            Regex regex = new Regex("(\\s+(OR)\\s*)$");
            StringBuilder query = new StringBuilder();



            using (APPContext context = new APPContext())
            {

                IList<IngredientsBasis> ingredients = context.IngredientsBasis.ToList();

                for (int i = 0; i < ingredients.Count(); i++)
                {
                    query.Append($"FORMSOF(INFLECTIONAL, \"{ingredients[i].Name}\") OR ");
                }

                return regex.Replace(query.ToString(), "");
            }

        }
    }
    
}
