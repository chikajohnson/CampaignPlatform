//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Campaign.Business.EF
{
    using System;
    using System.Collections.Generic;
    
    public partial class News
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public News()
        {
            this.NewsComments = new HashSet<NewsComment>();
        }
    
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
        public System.DateTime CreatedAt { get; set; }
        public string PublishedBy { get; set; }
        public System.DateTime PublishedAt { get; set; }
        public string ImageUrl { get; set; }
        public int CountryID { get; set; }
        public string CountryName { get; set; }
        public int StateID { get; set; }
        public string StateName { get; set; }
        public Nullable<int> LgaID { get; set; }
        public string LgaName { get; set; }
        public string Town { get; set; }
    
        public virtual Country Country { get; set; }
        public virtual Lga Lga { get; set; }
        public virtual NewsCategory NewsCategory { get; set; }
        public virtual State State { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NewsComment> NewsComments { get; set; }
    }
}
