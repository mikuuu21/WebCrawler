using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler.DbModels
{
    class Restaurants
    {
        [Key]
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string  City { get; set; }

        public virtual ICollection<Ingredients> Ingredients { get; set; }

    }
}
