using RestSharp;
using System;

namespace DAL
{
    public class HebCalAdapter
    {
        /*
         * we call the hebrew calander API with the date of today and a week from today
         * the func returns a json with the holidays in the comming week  
         */
        public string GetComingWeek(string startDate,string endDate)
        {

            string Url = $"https://www.hebcal.com/hebcal?v=1&cfg=json&maj=on&min=on&end={endDate}&start={startDate}";

            var client = new RestClient(Url);

            var request = new RestRequest(new Uri(Url), Method.Get);

            RestResponse response = client.Execute(request);

            return response.Content;
        }
    }
}
