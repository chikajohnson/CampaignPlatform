using Campaign.Business.EF;
using Campaign.Business.Repositories;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Campaign.API.Controllers
{
    [RoutePrefix("api/parties")]
    public class PartiesController : BaseController
    {
        private readonly PartyService _service;
        private UtilityService _utilityService;
        public PartiesController()
        {
            _service = new PartyService();
            _utilityService = new UtilityService();
        }

        [Route("")]
        public IHttpActionResult Get()
        {
            var parties = _service.GetAll().Select(x => new {
                ID = x.ID,
                Name = x.Name,
                Acronym = x.Acronym,
                ImageUrl = x.ImageUrl,
                //LgaID = x.LgaID,
                //LgaName = x.LgaName,
                //Town = x.Town,
                Address = x.Address,
                website = x.Website,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                CountryID = x.CountryID,
                CountryName = x.CountryName,
                //StateID = x.StateID,
                //StateName = x.StateName,
            }).ToList();

            if (parties == null)
            {
                Log.Information($"An error occured while retrieving parties {BadRequest()}");
                return BadRequest("An error occured while retrieving parties. ");
            }
            return Ok(parties);
        }
        
        [Route("{id}")]
        public IHttpActionResult Get(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            
            var item = _service.GetById(id);
            if (item == null)
            {
                return NotFound();
            }
            var party = new Party
            {
                ID = item.ID,
                Name = item.Name,
                Acronym = item.Acronym,
                ImageUrl = item.ImageUrl,
                //LgaID = item.LgaID,
                //LgaName = item.LgaName,
                //Town = item.Town,
                Address = item.Address,
                Website = item.Website,
                Email = item.Email,
                PhoneNumber = item.PhoneNumber,
                CountryID = item.CountryID,
                CountryName = item.CountryName,
               // StateID = item.StateID,
                //StateName = item.StateName,
            }; 
            if (party != null)
            {
                return Ok(party);
            }
            Log.Information($"Error occured while creating party {BadRequest()}");
            return BadRequest("Error occured while retreiving party.");
        }

        [Route("")]
        public IHttpActionResult Post(Party party)
        {
            if (party == null)
            {
                return BadRequest("An error occured while trying to create party.");
            }
            if (_service.CheckNameExists(party.Name))
            {
                return BadRequest($"Party name '{party.Name}' already exist");
            }

            if (_service.CheckNameExists(party.Name) || _service.CheckNameExists(party.Acronym))
            {
                return BadRequest($"Party acronym '{party.Acronym}' already exist");
            }


            party.ID = _utilityService.generateGuid();

            party = _service.Create(party);
            if (party != null)
            {
                return Ok(party);
            }
            Log.Information($"Error occured while creating party - {BadRequest()}");
            return BadRequest("Error occured while creating party.");
        }

        [Route("")]
        public IHttpActionResult Put(Party model)
        {
            if (model == null)
            {
                return BadRequest("An error occured while trying to update party.");
            }

            if (_service.GetById(model.ID) == null)
            {
                return NotFound();
            }

            var party = new Party
            {
                Name = model.Name,
                Acronym = model.Acronym,
                ImageUrl = model.ImageUrl,
                //LgaID = model.LgaID,
                //LgaName = model.LgaName,
                //Town = model.Town,
                Address = model.Address,
                Website = model.Website,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                CountryID = model.CountryID,
                CountryName = model.CountryName,
               // StateID = model.StateID,
                //StateName = model.StateName,
            };
            party = _service.Update(model);
            if (party != null)
            {
                return Ok(party);
            }
            Log.Information($"Error occured while updating  party {BadRequest()}");
            return BadRequest("Error occured while updating party");
        }

        [Route("{id}")]
        public IHttpActionResult Delete(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                Log.Information($"An error occured,  party is invalid {BadRequest()}");
                return BadRequest("An error occured,  party  invalid");
            }
            var party = _service.GetById(id);
            if (party == null)
            {
                return NotFound();
            }           

            party = _service.Delete(id);
            return Ok(party);
        }
    }
}