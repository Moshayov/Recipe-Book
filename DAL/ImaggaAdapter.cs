﻿using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;


namespace DAL
{
    public class ImaggaAdapter
    {
            public string GetImageInformation(byte[] imageData)
            {
                string apiKey = "acc_aec6fdf1f78ec11";
                string apiSecret = "884bbc9a6483d9faa8988d821c671fbb";

                string basicAuthValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{apiKey}:{apiSecret}"));
                var client = new RestClient("https://api.imagga.com/v2/tags");

                var request = new RestRequest("https://api.imagga.com/v2/tags", Method.Post);

                // Set the image data as a byte array
                request.AddParameter("image", imageData, ParameterType.RequestBody);

                request.AddHeader("Authorization", $"Basic {basicAuthValue}");

                RestResponse response = client.Execute(request);

                return response.Content;
            
        }

    }
}
