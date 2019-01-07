using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
