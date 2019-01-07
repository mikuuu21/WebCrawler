using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebCrawler.DbModels;

namespace WebCrowler.DbModels
{
    class Keys
    {
        [Key]
        public int Keyid { get; set; }
        public string ApiKey { get; set; }
        public string SearchKey { get; set; }


        public static Keys GetKeys()
        {
            using (APPContext context = new APPContext())
            {
                return context.Key.FirstOrDefault();
            }
            
        }


    }

}
