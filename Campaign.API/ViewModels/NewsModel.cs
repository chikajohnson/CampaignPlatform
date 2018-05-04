using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Campaign.API.ViewModels
{
    public class NewsModel
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string CategoryID { get; set; }
        public string CategoryName { get; set; }
        public bool IsPublished { get; set; }
        public int Likes { get; set; }
        public string Dislikes { get; set; }
        public int Shared { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string PublishedBy { get; set; }
        public DateTime PublishedAt { get; set; }
        public string ImageUrl { get; set; }
        public string ImageSource { get; set; }
        public int CountryID { get; set; }
        public string CountryName { get; set; }
        public int StateID { get; set; }
        public string StateName { get; set; }
        public int? LgaID { get; set; }
        public string LgaName { get; set; }
        public string Town { get; set; }
    }
}