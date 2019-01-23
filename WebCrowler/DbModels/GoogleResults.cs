using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebCrawler.DbModels;

namespace WebCrowler.DbModels
{
    class GoogleResults
    {
        [Key]
        public int Id { get; set; }
        public string pageTitle { get; set; }
        public string link { get; set; }
        public DateTime createDate { get; set; }
        public DateTime? checkDate { get; set; }

        public static List<string> GetPagesUrl(DateTime date)
        {
            using (APPContext context = new APPContext())
            {
                return context.GoogleResult.Where(x => x.checkDate == null).Select(x => x.link).ToList(); 
            }
        }

        public static List<GoogleResults> GetResults(DateTime date)
        {
            using (APPContext context = new APPContext())
            {
                return context.GoogleResult.Where(x => x.checkDate == null).ToList();
            }
        }
    }
}
