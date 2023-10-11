using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DP
{
    internal class RecipeModel
    {
        public string id { get; set; }
        public string title { get; set; }
        public string difficulty { get; set; }
        public string portion { get; set; }
        public string time { get; set; }
        public string description { get; set; }
        public List<string> ingredients { get; set; }
        public List<Dictionary<string, string>> method { get; set; }
        public string image { get; set; }
    }
}
