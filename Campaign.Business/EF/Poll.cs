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
    
    public partial class Poll
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Poll()
        {
            this.PollParticipations = new HashSet<PollParticipation>();
        }
    
        public string ID { get; set; }
        public string OpinionQuestion { get; set; }
        public string Category { get; set; }
        public string OpinionAnswerOptionA { get; set; }
        public string OpinionAnswerOptionB { get; set; }
        public string OpinionAnswerOptionC { get; set; }
        public string OpinionAnswerOptionD { get; set; }
        public string OpinionAnswerOptionE { get; set; }
        public int OpinionAnswerOptionACount { get; set; }
        public int OpinionAnswerOptionBCount { get; set; }
        public int OpinionAnswerOptionCCount { get; set; }
        public int OpinionAnswerOptionDCount { get; set; }
        public int OpinionAnswerOptionECount { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public Nullable<System.DateTime> PublishedAt { get; set; }
        public Nullable<bool> IsPublished { get; set; }
        public string PublishedBy { get; set; }
        public string Title { get; set; }
        public Nullable<int> NumberOfAnswerOptions { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PollParticipation> PollParticipations { get; set; }
    }
}
