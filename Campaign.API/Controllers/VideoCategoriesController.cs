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
    [RoutePrefix("api/videocategories")]
    public class VideoCategoriesController : BaseController
    {
        private readonly VideoCategoryService _service;
        private UtilityService _utilityService;
        public VideoCategoriesController()
        {
            _service = new VideoCategoryService();
            _utilityService = new UtilityService();
        }

        [Route("")]
        public IHttpActionResult Get()
        {
            var categories = _service.GetAll()
                .Select(x => new {
                    ID = x.ID,
                    Name = x.Name
                })
                .ToList();

            if (categories == null)
            {
                Log.Information($"An error occured while retrieving video categories {BadRequest()}");
                return BadRequest("An error occured while retrieving video categories");
            }
            return Ok(categories);
        }

        [Route("{id}")]
        public IHttpActionResult Get(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return BadRequest();
            }
            var category = _service.GetById(id);

            if (category == null)
            {
                return NotFound();
            }

            var cat = new NewsCategory
            {
                ID = category.ID,
                Name = category.Name
            };
            if (cat != null)
            {
                return Ok(cat);
            }
            Log.Information($"Error occured while retreiving category {BadRequest()}");
            return BadRequest("Error occured while retreiving category.");
        }

        [Route("")]
        public IHttpActionResult Post(VideoCategory model)
        {
            if (model == null)
            {
                return BadRequest("An error occured while trying to create video.");
            }

            if (_service.Exists(model.Name))
            {
                return BadRequest("Video Category " + "'" + model.Name + "'" + " already exists");
            }

            model.ID = _utilityService.generateGuid();
            var category = _service.Create(model);
            if (category != null)
            {
                return Ok(category);
            }
            Log.Information($"Error occured while creating video category - {BadRequest()}");
            return BadRequest("Error occured while creating video category.");
        }

        [Route("")]
        public IHttpActionResult Put(VideoCategory model)
        {
            if (model == null)
            {
                return BadRequest("An error occured while trying to update category.");
            }
            if (_service.GetById(model.ID) == null)
            {
                return BadRequest("The resource you are tring to update does not exist");
            }

            var category = _service.Update(model);
            if (category != null)
            {
                return Ok(category);
            }
            Log.Information($"Error occured while updating video category {BadRequest()}");
            return BadRequest("Error occured while updating video category");
        }

        [Route("{id}")]
        public IHttpActionResult Delete(string id)
        {
            if (String.IsNullOrEmpty(id) == true)
            {
                Log.Information($"An error occured, video category is invalid {BadRequest()}");
                return BadRequest("An error occured, video category  invalid");
            }
            if (_service.GetById(id) == null)
            {
                return NotFound();
            }

            var cat = _service.Delete(id);
            return Ok(cat);
        }
    }
}