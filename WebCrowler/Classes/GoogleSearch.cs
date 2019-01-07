using Google.Apis.Customsearch.v1;
using Google.Apis.Customsearch.v1.Data;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebCrawler.DbModels;
using WebCrowler.DbModels;

namespace WebCrowler.Classes
{
    class GoogleSearch
    {

        public string GoogleApiKey { get;}
        public string GoogleSearchId {get;}

        public GoogleSearch()
        {
            Keys keys = Keys.GetKeys();
            GoogleApiKey = keys.ApiKey;
            GoogleSearchId = keys.SearchKey;
        }

        public void SearchForSites(string query)
        {
            using (APPContext context = new APPContext())
            {
                GoogleResults result;
                List<GoogleResults> resultList = new List<GoogleResults>();
                CustomsearchService customsearchService = GoogleSearchAuthorize(GoogleApiKey);

                var listRequest = customsearchService.Cse.List(query);
                listRequest.Cx = GoogleSearchId;
                IList<Result> paging = new List<Result>();
                var count = 0;

                while (paging != null)
                {
                    Console.WriteLine($"Page {count}");
                    listRequest.Start = 10 * count + 1;

                    paging = listRequest.Execute().Items;

                    if (paging != null)
                    {
                        foreach (var item in paging)
                        {
                            result = new GoogleResults();
                            result.pageTitle = item.Title;
                            result.link = item.Link;
                            result.createDate = DateTime.Now.Date;

                            resultList.Add(result);

                        }
                        
                        count++;
                    }
                }
                
                List<GoogleResults> distinctResults = resultList.GroupBy(x => x.link).Select(x => x.First()).ToList();
                context.GoogleResult.AddRange(distinctResults);
                context.SaveChanges();

            }
            Console.WriteLine("Done.");
            Console.ReadLine();

        }

        private static CustomsearchService GoogleSearchAuthorize(string apiKey)
        {


            var customSearchService = new CustomsearchService(new BaseClientService.Initializer { ApiKey = apiKey });

            return customSearchService;
        }

    }
}
