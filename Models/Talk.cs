using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TgBotApi.Models
{
    public class Talk
    {
        public FulfillmentMessages fulfillmentMessages { get; set; }
    }

    public class FulfillmentMessages
    {
        public List<string> text { get; set; }
    }
}
