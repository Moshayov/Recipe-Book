using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;


namespace DAL
{
    public class ImaggaAdapter
    {
        //return from the imagga the values of the image it get
        public string GetImageInformation(string ImageUrl)
        {
            string apiKey = "acc_4858251230dcb88";
            string apiSecret = "c2f19534f3e9c73697368ef9cfe0cae1";


            string basicAuthValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(String.Format("{0}:{1}", apiKey, apiSecret)));
            var client = new RestClient("https://api.imagga.com/v2/tags");

            var request = new RestRequest(new Uri("https://api.imagga.com/v2/tags"), Method.Get);
            request.AddParameter("image_url", ImageUrl);
            request.AddHeader("Authorization", String.Format("Basic {0}", basicAuthValue));

            RestResponse response = client.Execute(request);

            return response.Content;

        }
    }
}
