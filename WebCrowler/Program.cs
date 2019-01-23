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
using HtmlAgilityPack;
using System.IO;
using System.Drawing;


namespace WebCrawler
{
    class Program
    {
        static void Main(string[] args)
        {
            //https://stackoverflow.com/questions/26958829/how-do-i-use-the-new-httpclient-from-windows-web-http-to-download-an-image pobranie obrazka za pomoc¹ HttpClient

            Crawler webCrawler = new Crawler();
            webCrawler.StartWebCrawling();

            //IngredientsStore.GetBasicIngredients();

            Console.ReadLine();

        }

       
        private static async void GetImages( HttpClient client)
        {


            Bitmap bitmapImage;

            try
            {

                string uri = "static/menu/kuchnia_Strona_1.jpg";
                using (var response = await client.GetAsync(uri))
                {
                    response.EnsureSuccessStatusCode();

                    using (Stream inputStream = await response.Content.ReadAsStreamAsync()) 
                    {
                        bitmapImage = new Bitmap(inputStream);
                    }
                }


            }
            catch (Exception)
            {

                throw;
            }
        }


    }
}

