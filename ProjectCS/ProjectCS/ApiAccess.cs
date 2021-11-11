using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json.Linq;

namespace ProjectCS
{
    public class ApiAccess
    {
        public const string FreeBaseUrl = "https://free.currconv.com/api/v7/";
        
        public static List<CurrencyType> GetAllCurrencies()
        {
            string url;
            var apikey = "c39426996576d2d7d28b";
            url = FreeBaseUrl + "currencies" + "?apiKey=" + apikey;

            var jsonString = GetResponse(url);

            var data = JObject.Parse(jsonString)["results"].ToArray();
            return data.Select(item => item.First.ToObject<CurrencyType>()).ToList();
        }
        private static string GetResponse(string url)
        {
            string jsonString;

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (var response = (HttpWebResponse)request.GetResponse())
            using (var stream = response.GetResponseStream())
            using (var reader = new StreamReader(stream))
            {
                jsonString = reader.ReadToEnd();
            }

            return jsonString;
        }
        
        public static double ExchangeRate(string from, string to)
        {
            var apikey = "c39426996576d2d7d28b";
            string url;
            url = FreeBaseUrl + "convert?q=" + from + "_" + to + "&compact=y&apiKey=" + apikey;

            var jsonString = GetResponse(url);
            
            return JObject.Parse(jsonString).First.First["val"].ToObject<double>();
        }
        
    }
}