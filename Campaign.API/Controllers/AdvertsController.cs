using Campaign.API.ViewModels;
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
    [RoutePrefix("api/adverts")]
    public class AdvertsController : BaseController
    {
        private readonly AdvertService _service;
        private UtilityService _utilityService;
        public AdvertsController()
        {
            _service = new AdvertService();
            _utilityService = new UtilityService();
        }

        [Route("")]
        public IHttpActionResult Get()
        {
            var adverts = _service.GetAll().Select(x => new {
                ID = x.ID,
                Description = x.Description,
                CreatedAt = x.CreatedAt,
                CreatedBy = x.CreatedBy,
                Url = x.Url,
                Title = x.Title,
                Type = x.Type,
                CountryID = x.CountryID,
                CountryName = x.CountryName,
                StateID = x.StateID,
                StateName = x.StateName,
                LgaID = x.LgaID,
                LgaName = x.LgaName,
                Town = x.Town
            }).ToList();

            if (adverts == null)
            {
                Log.Information($"An error occured while retrieving adverts {BadRequest()}");
                return BadRequest("An error occured while retrieving adverts. ");
            }
            return Ok(adverts);
        }

        [Route("{id}")]
        public IHttpActionResult Get(string id)
        {
            var advert = _service.GetById(id);
            if (advert == null)
            {
                return NotFound();

            }
            var advertModel = new AdvertModel
            {
                ID = advert.ID,
                Description = advert.Description,
                CreatedAt = advert.CreatedAt,
                CreatedBy = advert.CreatedBy,
                Url = advert.Url,
                Title = advert.Title,
                Type = advert.Type,
                CountryID = advert.CountryID,
                CountryName = advert.CountryName,
                StateID = advert.StateID,
                StateName = advert.StateName,
                LgaID = advert.LgaID,
                LgaName = advert.LgaName,
                Town = advert.Town
            };

            if (advertModel != null)
            {
                return Ok(advertModel);
            }
            Log.Information($"Error occured while creating advert {BadRequest()}");
            return BadRequest("Error occured while retreiving advert.");
        }

        [Route("")]
        public IHttpActionResult Post(Advert model)
        {
            if (model == null)
            {
                return BadRequest("An error occured while trying to create advert.");
            }

            model.ID = _utilityService.generateGuid();
            model.CreatedAt = DateTime.Now;
            model.CreatedBy = UserRecord.Id;
               
            var advert = _service.Create(model);
            if (advert != null)
            {
                return Ok(advert);
            }
            Log.Information($"Error occured while creating advert - {BadRequest()}");
            return BadRequest("Error occured while creating advert.");
        }

        [Route("")]
        public IHttpActionResult Put(AdvertModel model)
        {
            if (model == null)
            {
                return BadRequest("An error occured while trying to update advert.");
            }

            if (_service.GetById(model.ID) == null)
            {
                return NotFound();
            }

            var advert = new Advert
            {
                ID = model.ID,
                Title = model.Title,
                Description = model.Description,
                Url = model.Url,
                Type = model.Type,
                CreatedBy = model.CreatedBy,
                CreatedAt = DateTime.Now,
                CountryID = model.CountryID,
                CountryName = model.CountryName,
                StateID = model.StateID,
                StateName = model.StateName,
                LgaID = model.LgaID,
                LgaName = model.LgaName,
                Town = model.Town,
            };

            advert = _service.Update(advert);
            if (advert != null)
            {
                return Ok(advert);
            }
            Log.Information($"Error occured while updating  advert {BadRequest()}");
            return BadRequest("Error occured while updating advert");
        }

        [Route("{id}")]
        public IHttpActionResult Delete(string id)
        {
            if (String.IsNullOrEmpty(id) == true)
            {
                Log.Information($"An error occured,  advert is does not exist {NotFound()}");
                return NotFound();
            }

            var  advert = _service.Delete(id);
            return Ok(advert);
        }
    }
}