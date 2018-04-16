using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Campaign.API.ViewModels
{
    public class AdvertModel
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CountryID { get; set; }
        public string CountryName { get; set; }
        public int StateID { get; set; }
        public string StateName { get; set; }
        public int? LgaID { get; set; }
        public string LgaName { get; set; }
        public string Town { get; set; }

    }
}