using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SearchfightCore;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections;
namespace GoogleSearch
{
    public class GoogleSearch: ISearch
    {
        private Hashtable _Parameters;




        #region ISearch Members

        public void Initialitation(Hashtable parameters)
        {
            this._Parameters = parameters;
        }

        public string SearchTitle()
        {
            return "Google";
        }

      
        private Uri GetURI(string input)
        {

            string urlweb = this._Parameters["MainUrl"].ToString();
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat(urlweb, System.Web.HttpUtility.UrlEncode(input), System.Web.HttpUtility.UrlEncode(this._Parameters["cx"].ToString()), System.Web.HttpUtility.UrlEncode(this._Parameters["Appkey"].ToString())); // requires ref to System.Web.dll

            return new System.Uri(sb.ToString());

        }
        public long ExecuteSearch(string input)
        {
            Uri url = GetURI(input);

            WebRequest request = HttpWebRequest.Create(url);

            WebResponse response = request.GetResponse();


            Stream raw = response.GetResponseStream();

            StreamReader s = new StreamReader(raw);
            string value = string.Empty;
            string key = string.Empty;
            long result = 0;
            string json = s.ReadToEnd();


            Regex reg = new Regex("\"totalResults\": \"(\\d+)\"");
            MatchCollection mc = reg.Matches(json);
            foreach (Match m in mc)
            {
                key = m.Groups[0].Value;
                value = m.Groups[1].Value;
                if (long.TryParse(value, out result)) break;
            }

            return string.IsNullOrEmpty(value) ? 0 : long.Parse(value);//list.Count;

        }


        #endregion

        
    }
}
