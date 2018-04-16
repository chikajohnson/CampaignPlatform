using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Campaign.API.ViewModels
{
    public class CandidateModel
    {
        [Required]
        public string ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ImageUrl { get; set; }
        public string Gender { get; set; }
        public string OtherNames { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? Age { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Qualifications { get; set; }
        public string PartyID { get; set; }
        public string PartyName { get; set; }
        public string Position { get; set; }
        public string PositionLocation { get; set; }
        public string Remarks { get; set; }
        public int CountryID { get; set; }
        public string CountryName { get; set; }
        public int StateID { get; set; }
        public string StateName { get; set; }
        public int? LgaID { get; set; }
        public string LgaName { get; set; }
        public string Town { get; set; }
    }
}