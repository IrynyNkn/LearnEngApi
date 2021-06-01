using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TgBotApi.Models
{
    public class ApiResponse
    {
        public string Word { get; set; }
        public List<string> Definition { get; set; }
        public List<string> Usage { get; set; }
        public List<string> Synonyms { get; set; }
        public List<string> Antonyms { get; set; }
        public string Audio { get; set; }
        public string Explanation { get; set; }
    }
}
