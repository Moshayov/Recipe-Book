﻿using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    internal class RecipeAdapter
    {
        public string GetRecipes(string id)
        {
            string API_Key = "81f90ca374msh1bb6ff6b8a1b9d4p158205jsn605ad534db9e";

            var client = new RestClient("https://the-vegan-recipes-db.p.rapidapi.com/45");
            var request = new RestRequest(new Uri("https://the-vegan-recipes-db.p.rapidapi.com/" + id), Method.Get);
            request.AddHeader("X-RapidAPI-Key", API_Key);
            request.AddHeader("X-RapidAPI-Host", "the-vegan-recipes-db.p.rapidapi.com");
            RestResponse response = client.Execute(request);
            return response.Content;
        }
    }
}
