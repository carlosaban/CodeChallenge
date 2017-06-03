using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SearchfightCore;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections;

using System.Net.Http.Headers;

using System.Net.Http;
using System.Web;
using System.Web.Script.Serialization;
using System.Threading.Tasks;

namespace MSNSearch
{
    public class MSNSearch :ISearch
    {


        private Hashtable _Parameters = new Hashtable();




        #region ISearch Members

        public void Initialitation(Hashtable parameters)
        {
            this._Parameters = parameters;
        }

        public string SearchTitle()
        {
            return "MSN Search";
        }

        public long ExecuteSearch(string input)
        {
            
            return Task.Run(()=>MakeRequest(input)).Result;

        }

        async Task<long> MakeRequest(string input)
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            // Request headers
            //client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "341c1460842d45858fb53114888223c5");
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", this._Parameters["Ocp-Apim-Subscription-Key"].ToString());

            // Request parameters
            queryString["q"] = System.Web.HttpUtility.UrlEncode(input);
            var uri = this._Parameters["MainUrl"].ToString() + queryString;

            string json = await client.GetStringAsync(uri);

            string value = string.Empty;
            string key = string.Empty;
            long result = 0;
            

            Regex reg = new Regex("\"totalEstimatedMatches\": (\\d+)");
            MatchCollection mc = reg.Matches(json);
            foreach (Match m in mc)
            {
                key = m.Groups[0].Value;
                value = m.Groups[1].Value;
                if (long.TryParse(value, out result)) break;
            }

            return  string.IsNullOrEmpty(value) ? 0 : long.Parse(value);//list.Count;
            
        }

        #endregion

    }
}
