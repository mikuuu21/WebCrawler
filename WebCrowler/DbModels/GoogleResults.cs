using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
