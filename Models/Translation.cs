using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TgBotApi.Models
{
    public class Translation
    {
        public Data data { get; set; }
    }
    //public class Pair
    //{
    //    public string s { get; set; }
    //    public string t { get; set; }
    //}

    public class Data
    {
        public string translation { get; set; }
        public string pronunciation { get; set; }
    }
}
