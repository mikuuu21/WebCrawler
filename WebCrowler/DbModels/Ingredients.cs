using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebCrowler.DbModels;

namespace WebCrawler.DbModels
{
    class Ingredients
    {
        public Ingredients()
        {
            this.Restaurants = new HashSet<Restaurants>();
        }

        [Key]
        public int IngredientId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Restaurants> Restaurants { get; set; }

        public static List<Ingredients> ConvertToIngredientsList(List<Nodes> nodes)
        {
            List<Ingredients> ingredientsList = new List<Ingredients>();
            Ingredients ingredient = new Ingredients();
            foreach (var node in nodes)
            {
                ingredient.Name = node.node;
                ingredientsList.Add(ingredient);
            }

            return ingredientsList;
        }
    }
}
