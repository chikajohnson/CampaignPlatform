using Campaign.API.ViewModels;
using Campaign.Business.EF;
using Campaign.Business.Repositories;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Campaign.API.Controllers
{
    [RoutePrefix("api/candidates")]
    public class CandidatesController :  BaseController
    {
        private readonly CandidateService _service;
        private UtilityService _utilityService;
        public CandidatesController()
        {
            _service = new CandidateService();
            _utilityService = new UtilityService();
        }

        [Route("")]
        public IHttpActionResult Get()
        {
            var candidates = _service.GetAll()
                .Select(x => new {
                    ID = x.ID,
                    Address = x.Address,
                    Age = x.Age,
                    DateOfBirth = x.DateOfBirth,
                    CountryName = x.CountryName,
                    StateName = x.StateName,
                    LgaName = x.LgaName,
                    Email = x.Email,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    OtherNames = x.OtherNames,
                    PartyName = x.PartyName,
                    ElectoralOffice = x.Position,
                    Constituency = x.PositionLocation,
                    Gender = x.Gender,
                    Town = x.Town,
                    ImageUrl = x.ImageUrl,
                    Remarks = x.Remarks,
                    PhoneNumber = x.PhoneNumber,
                    Qualifications = x.Qualifications
                })
                .ToList();
            if (candidates != null)
            {
                return Ok(candidates);
            }
            Log.Information($"Error occured while retreiving candidates {BadRequest()}");
            return BadRequest("Error occured while retreiving candidates");
        }

        [Route("displaydefault")]
        public IHttpActionResult GetDefaultCandidates()
        {
            var candidates = _service.GetDefaultCandidatesByElectoralOffice()
                .Select(x => new {
                    ID = x.ID,
                    Address = x.Address,
                    Age = x.Age,
                    DateOfBirth = x.DateOfBirth,
                    CountryID = x.Country.Name,
                    StateName = x.State.Name,
                    LgaName = x.LgaID,
                    Email = x.Email,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    OtherNames = x.OtherNames,
                    PartyName = x.PartyID,
                    Position = x.ElectoralOffice,
                    PositionLocation = x.Constituency,
                    Gender = x.Gender,
                    Town = x.Town,
                    ImageUrl = x.ImageUrl,
                    Remarks = x.Remarks,
                    PhoneNumber = x.PhoneNumber,
                    Qualifications = x.Qualifications
                })
                .ToList();
            if (candidates != null)
            {
                return Ok(candidates);
            }
            Log.Information($"Error occured while retreiving candidates {BadRequest()}");
            return BadRequest("Error occured while retreiving candidates");
        }


        [Route("{id}")]
        public IHttpActionResult Get(string id)
        {
            var candidate = _service.GetSingleViewById(id);
            var candidateModel = new CandidateView
            {
                ID = candidate.ID,
                Address = candidate.Address,
                Age = candidate.Age,
                DateOfBirth = candidate.DateOfBirth,
                CountryID = candidate.CountryID,
                CountryName = candidate.CountryName,
                StateID = candidate.StateID,
                StateName = candidate.StateName,
                LgaID = candidate.LgaID,
                LgaName = candidate.LgaName,
                Email = candidate.Email,
                FirstName = candidate.FirstName,
                LastName = candidate.LastName,
                OtherNames = candidate.OtherNames,
                PartyID = candidate.PartyID,
                PartyName = candidate.PartyName,
                Position = candidate.Position,
                PositionLocation = candidate.PositionLocation,
                Gender = candidate.Gender,
                Town = candidate.Town,
                ImageUrl = candidate.ImageUrl,
                ImageSource = candidate.ImageSource,
                Remarks = candidate.Remarks,
                PhoneNumber = candidate.PhoneNumber,
                Qualifications = candidate.Qualifications
            };

            if (candidateModel != null)
            {
                return Ok(candidateModel);
            }
            Log.Information($"Error occured while creating candidate {BadRequest()}");
            return BadRequest("Error occured while retreiving candidate");
        }

        [Route("")]
        public IHttpActionResult Post(Candidate model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("An error occured while trying to create candidate");
            }

            model.ID = _utilityService.generateGuid();           
            var candidate = _service.Create(model);
            if (candidate != null)
            {
                return Ok(candidate);
            }
            Log.Information($"Error occured while creating candidate {BadRequest()}");
            return BadRequest("Error occured while creating candidate.");
        }

        [Route("")]
        public IHttpActionResult Put(CandidateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("An error occured while trying to update candidate: model invalid");
            }

            if (model == null || _service.GetById(model.ID) == null)
            {
                return BadRequest("Candidate is invalid");
            }

            var candidate = new Candidate
            {
                ID = model.ID,
                Address = model.Address,
                Age = model.Age,
                DateOfBirth = model.DateOfBirth,
                CountryID = model.CountryID,
                StateID = model.StateID,
                LgaID = model.LgaID,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                OtherNames = model.OtherNames,
                PartyID = model.PartyID,
                ElectoralOffice = model.ElectoralOffice,
                Constituency = model.Constituency,
                Gender = model.Gender,
                Town = model.Town,
                ImageUrl = model.ImageUrl,
                ImageSource = model.ImageSource,
                Remarks = model.Remarks,
                PhoneNumber = model.PhoneNumber,
                Qualifications = model.Qualifications
            };

            candidate = _service.Update(candidate);
            if (candidate != null)
            {
                return Ok(candidate);
            }
            Log.Information($"Error occured while updating candidates {BadRequest()}");
            return BadRequest("Error occured while updating candidates");
        }

        [Route("{id}")]
        public IHttpActionResult Delete(string id)
        {
            if (String.IsNullOrEmpty(id) == true)
            {
                Log.Information($"An error occured, candidate is invalid {BadRequest()}");
                return BadRequest("An error occured, candidate is invalid");
            }

            _service.Delete(id);
             return Ok("Candidates updated Successfully");
          }


    }
}