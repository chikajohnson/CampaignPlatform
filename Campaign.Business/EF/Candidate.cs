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
    
    public partial class Candidate
    {
        public string ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ImageUrl { get; set; }
        public string ImageSource { get; set; }
        public string Gender { get; set; }
        public string OtherNames { get; set; }
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        public Nullable<int> Age { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Qualifications { get; set; }
        public string PartyID { get; set; }
        public string ElectoralOffice { get; set; }
        public string Constituency { get; set; }
        public string Remarks { get; set; }
        public int CountryID { get; set; }
        public int StateID { get; set; }
        public Nullable<int> LgaID { get; set; }
        public string Town { get; set; }
    
        public virtual Country Country { get; set; }
        public virtual Party Party { get; set; }
        public virtual State State { get; set; }
    }
}
