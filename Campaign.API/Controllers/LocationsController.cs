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
    [RoutePrefix("api/locations")]
    public class LocationsController : BaseController
    {
        private readonly LocationService _service;
        private UtilityService _utilityService;
        public LocationsController()
        {
            _service = new LocationService();
            _utilityService = new UtilityService();
        }

        [Route("countries")]
        public IHttpActionResult GetCountries()
        {
            var countries = _service.GetCountries().Select(x => new {
                ID = x.ID,
                Name = x.Name,
                Code = x.Code
            }).ToList();

            if (countries == null)
            {
                Log.Information($"An error occured while retrieving countries {BadRequest()}");
                return BadRequest("An error occured while retrieving countries. ");
            }
            return Ok(countries);
        }

        [Route("states")]
        public IHttpActionResult GetStates()
        {
            var states = _service.GetStates().Select(x => new {
                ID = x.ID,
                Name = x.Name,
                Code = x.Code
            }).ToList();

            if (states == null)
            {
                Log.Information($"An error occured while retrieving states {BadRequest()}");
                return BadRequest("An error occured while retrieving states. ");
            }
            return Ok(states);
        }

        [Route("lgas")]
        public IHttpActionResult GetLgas()
        {
            var lgas = _service.GetLgas().Select(x => new {
                ID = x.ID,
                Name = x.Name
            }).ToList();

            if (lgas == null)
            {
                Log.Information($"An error occured while retrieving lgas {BadRequest()}");
                return BadRequest("An error occured while retrieving lgas. ");
            }
            return Ok(lgas);
        }

        [Route("states/{id}")]
        public IHttpActionResult GetStates(int id)
        {
            if (String.IsNullOrEmpty(id.ToString()))
            {
                return NotFound();
            }

            var states = _service.GetStates(id).Select(x => new {
                ID = x.ID,
                Name = x.Name,
                Code = x.Code
            }).ToList();

            if (states == null)
            {
                Log.Information($"An error occured while retrieving states {BadRequest()}");
                return BadRequest("An error occured while retrieving states. ");
            }
            return Ok(states);
        }

        [Route("lgas/{stateId}")]
        public IHttpActionResult GetLgas(int stateId)
        {
            if (String.IsNullOrEmpty(stateId.ToString()))
            {
                return NotFound();
            }

            var lgas = _service.GetLgas(stateId).Select(x => new {
                ID = x.ID,
                Name = x.Name,
                StateName = x.StateName,
                StateID = x.StateName
            }).ToList();

            if (lgas == null)
            {
                Log.Information($"An error occured while retrieving lgas {BadRequest()}");
                return BadRequest("An error occured while retrieving lgas. ");
            }
            return Ok(lgas);
        }       
    }
}