using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Net.Http;
using Google.Apis.Services;
using Google.Apis.Customsearch.v1.Data;
using Google.Apis.Customsearch.v1;
using WebCrowler.DbModels;
using WebCrowler.Classes;

namespace WebCrawler
{
    class Program
    {
        static void Main(string[] args)
        {


            //string url = "";
            //WebClient web = new WebClient();
            //string site = web.DownloadString(url);

            GoogleSearch search = new GoogleSearch();
            Console.WriteLine("Wprowadü zapytanie");
            string query = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(query))
            {
                search.SearchForSites(query);
            }

            
            
        }
    }


}
