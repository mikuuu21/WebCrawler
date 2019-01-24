using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebCrawler.DbModels;

namespace WebCrowler.DbModels
{
    class Nodes
    {
        [Key]
        public int Id { get; set; }
        public string node { get; set; }



        public static List<Nodes> RunStoredProcedure(string query)
        {
            using (var context = new APPContext())
            {
                var parameter = new SqlParameter("@serachTerm", query);

                var result = context.Database
                    .SqlQuery<Nodes>("findIngredients @serachTerm", parameter)
                    .ToList();

                return result;
            }
        }

    }


}
