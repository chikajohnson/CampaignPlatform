using Campaign.Business.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Campaign.API.ViewModels
{
    public class CommentModel
    {
        public string ID { get; set; }
        public string NewsID { get; set; }
        public string VideoID { get; set; }
        public string ParentID { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public string postedBy { get; set; }
        public bool? IsParent { get; set; }
        public List<CommentModel> Replies { get; set; }
    }
}