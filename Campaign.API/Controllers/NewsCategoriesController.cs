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
    [Authorize]
    [RoutePrefix("api/newscategories")]
    public class NewsCategoriesController : BaseController
    {
        private readonly NewsCategoryService _service;
        private UtilityService _utilityService;

        public NewsCategoriesController()
        {
            _service = new NewsCategoryService();
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
                Log.Information($"An error occured while retrieving news categories {BadRequest()}");
                return BadRequest("An error occured while retrieving news categories");
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
            Log.Information($"Error occured while creating category {BadRequest()}");
            return BadRequest("Error occured while retreiving category.");
        }

        [Route("")]
        public IHttpActionResult Post(NewsCategory model)
        {
            if (model == null)
            {
                return BadRequest("An error occured while trying to create news item.");
            }

            if (_service.Exists(model.Name))
            {
                return BadRequest("News Categoy " + "'" + model.Name + "'" + " already exists");
            }

            if (String.IsNullOrEmpty(model.Name))
            {
                return BadRequest("Newscategory name is required.");

            }

            model.ID = _utilityService.generateGuid();
            var category = _service.Create(model);
            if (category != null)
            {
                return Ok(category);
            }
            Log.Information($"Error occured while creating category - {BadRequest()}");
            return BadRequest("Error occured while creating category.");
        }

        [Route("")]
        public IHttpActionResult Put(NewsCategory model)
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
            Log.Information($"Error occured while updating news category {BadRequest()}");
            return BadRequest("Error occured while updating news category");
        }

        [Route("{id}")]
        public IHttpActionResult Delete(string id)
        {
            if (String.IsNullOrEmpty(id) == true)
            {
                Log.Information($"An error occured, news category is invalid {BadRequest()}");
                return BadRequest("An error occured, news category  invalid");
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