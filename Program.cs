using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
namespace DinoParse
{
    class Program
    {
        private static void Main()
        {
            Console.WriteLine("Enter the name of a Wikipedia page to find related links:");
            string userSearch = Console.ReadLine();
            string searchLink = "https://en.wikipedia.org/w/api.php?action=parse&format=json&prop=links&redirect=1&page=" + userSearch;
            string path = @"DinoParse.csv";
            dynamic link = RetrieveJson(searchLink);
            string linkString = link.ToString();
            File.WriteAllText(path, linkString);
            Console.WriteLine(linkString);
        }

        public static dynamic RetrieveJson(string input)
        {
            using WebClient wc = new WebClient();
            string json = wc.DownloadString(input);
            JObject dinoList = JObject.Parse(json);
            JArray links = (JArray)dinoList["parse"]["links"];
            JArray parsedLinks = new JArray();
            foreach (JObject link in links)
            {
                string linkString = link["ns"].ToString();
                if (linkString == "0")
                {
                    parsedLinks.Add(link["*"]);
                }
            }
            return parsedLinks;
        }
    }
}
