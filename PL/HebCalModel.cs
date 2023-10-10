using System;
using System.Collections.Generic;

namespace DP
{
    //This is the format of the HebCal API 
    public class Item
    {
        public string title { get; set; }
        public string date { get; set; }
        public string hdate { get; set; }
        public string category { get; set; }
        public string subcat { get; set; }
        public string hebrew { get; set; }
        public string link { get; set; }
        public string memo { get; set; }
        public Leyning leyning { get; set; }
        public bool? yomtov { get; set; }
    }

    public class Leyning
    {
        public string _1 { get; set; }
        public string _2 { get; set; }
        public string _3 { get; set; }
        public string _4 { get; set; }
        public string _5 { get; set; }
        public string _6 { get; set; }
        public string _7 { get; set; }
        public string torah { get; set; }
        public string haftarah { get; set; }
        public string maftir { get; set; }
        public Triennial triennial { get; set; }
    }

    public class Location
    {
        public string geo { get; set; }
    }

    public class Range
    {
        public string start { get; set; }
        public string end { get; set; }
    }

    public class Root
    {
        public string title { get; set; }
        public DateTime date { get; set; }
        public Location location { get; set; }
        public Range range { get; set; }
        public List<Item> items { get; set; }
    }

    public class Triennial
    {
        public string _1 { get; set; }
        public string _2 { get; set; }
        public string _3 { get; set; }
        public string _4 { get; set; }
        public string _5 { get; set; }
        public string _6 { get; set; }
        public string _7 { get; set; }
        public string maftir { get; set; }
    }
}
