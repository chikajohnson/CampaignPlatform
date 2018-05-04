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
    [RoutePrefix("api/news")]
    public class NewsController : BaseController
    {
        private readonly NewsService _service;
        private UtilityService _utilityService;
        public NewsController()
        {
            _service = new NewsService();
            _utilityService = new UtilityService();
        }

        [Route("")]
        public IHttpActionResult Get()
        {
            var newsArticles = _service.GetAll().Select(x => new
            {
                ID = x.ID,
                Title = x.Title,
                Body = x.Body,
                ImageUrl = x.ImageUrl,
                LgaID = x.LgaID,
                LgaName = x.LgaName,
                Town = x.Town,
                PublishedBy =x.PublishedBy,
                PublishedAt = x.PublishedAt,
                CategoryName = x.CategoryName,
                CountryID = x.CountryID,
                CountryName = x.CountryName,
                StateName = x.StateName,
                StateID = x.StateID,
                CreatedAt = x.CreatedAt,
                CreatedBy = x.CreatedBy,
                IsPublished = x.IsPublished,
                SharedBy = x.Shared
            }).ToList();

            if (newsArticles == null)
            {
                Log.Information($"An error occured while retrieving news items {BadRequest()}");
                return BadRequest("An error occured while retrieving news items");
            }
            return Ok(newsArticles);
        }
        
        [Route("{id}")]
        public IHttpActionResult Get(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return BadRequest("invalid news item");
            }
            var newsItem = _service.GetSingleViewById(id);
            if (newsItem == null)
            {
                return NotFound();
            }
            var news = new NewsView
            {
                ID = newsItem.ID,
                Title = newsItem.Title,
                Body = newsItem.Body,
                ImageUrl = newsItem.ImageUrl,
                ImageSource = newsItem.ImageSource,
                LgaID = newsItem.LgaID,
                LgaName = newsItem.LgaName,
                Town = newsItem.Town,
                PublishedBy = newsItem.PublishedBy,
                PublishedAt = newsItem.PublishedAt,
                CategoryID = newsItem.CategoryID,
                CategoryName = newsItem.CategoryName,
                CountryID = newsItem.CountryID,
                CountryName = newsItem.CountryName,
                StateName = newsItem.StateName,
                StateID = newsItem.StateID,
                CreatedAt = newsItem.CreatedAt,
                CreatedBy = newsItem.CreatedBy,
                Dislikes = newsItem.Dislikes,
                Likes = newsItem.Likes,
                IsPublished = newsItem.IsPublished,
                Shared = newsItem.Shared
            };

            if (news != null)
            {
                return Ok(news);
            }
            Log.Information($"Error occured while creating news {BadRequest()}");
            return BadRequest("Error occured while retreiving news item");
        }


        [Route("category/{id}")]
        public IHttpActionResult GetCategory(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return BadRequest("invalid news item");
            }
            var newsItems = _service.GetByCategory(id)
                .Select(x => new
                {
                    ID = x.ID,
                    Title = x.Title,
                    Body = x.Body,
                    ImageUrl = x.ImageUrl,
                    ImageSource = x.ImageSource,
                    LgaID = x.LgaID,
                    LgaName = x,
                    Town = x.Town,
                    PublishedBy = x.PublishedBy,
                    PublishedAt = x.PublishedAt,
                    CategoryName = x.CategoryName,
                    CountryID = x.CountryID,
                    CountryName = x.CountryName,
                    StateName = x.StateName,
                    StateID = x.StateID,
                    CreatedAt = x.CreatedAt,
                    CreatedBy = x.CreatedBy,
                    IsPublished = x.IsPublished,
                    Shared = x.Shared
                }).ToList(); ;
            if (newsItems == null)
            {
                return NotFound();
            }
           
            if (newsItems != null)
            {
                return Ok(newsItems);
            }
            Log.Information($"Error occured while retreiving news {BadRequest()}");
            return BadRequest("Error occured while retreiving news items");
        }

        [Route("")]
        public IHttpActionResult Post(News model)
        {
            if (model == null)
            {
                return BadRequest("An error occured while trying to create news item.");
            }
            model.CreatedBy = UserRecord.Id;
            model.CreatedAt = DateTime.Now;
            model.ID = _utilityService.generateGuid();
            model.PublishedBy = UserRecord.Id;
            model.PublishedAt = DateTime.Now;
            model.IsPublished = false;
            model.Shared = 0;
            model.Likes = 0;
            model.Dislikes = "ok";
            var newsItem = _service.Create(model);
            if (newsItem != null)
            {
                return Ok(newsItem);
            }
            Log.Information($"Error occured while creating news item {BadRequest()}");
            return BadRequest("Error occured while creating news item");
        }

        [Route("")]
        public IHttpActionResult Put(NewsModel model)
        {
            if (model == null)
            {
                return BadRequest("An error occured while trying to update news item.");
            }

            if (_service.GetById(model.ID) == null)
            {
                return BadRequest("news item is invalid");
            }

            var news = new News
            {
                ID = model.ID,
                Title = model.Title,
                Body = model.Body,
                ImageUrl = model.ImageUrl,
                ImageSource = model.ImageSource,
                LgaID = model.LgaID,
                Town = model.Town,
                PublishedBy = model.PublishedBy,
                PublishedAt = model.PublishedAt,
                CategoryID = model.CategoryID,
                Likes = model.Likes,
                Dislikes = model.Dislikes,
                CountryID = model.CountryID,
                StateID = model.StateID,
                CreatedAt = model.CreatedAt,
                CreatedBy = model.CreatedBy,
                IsPublished = model.IsPublished,
                Shared = model.Shared
            };
            news = _service.Update(news);
            if (news != null)
            {
                return Ok(news);
            }
            Log.Information($"Error occured while updating news {BadRequest()}");
            return BadRequest("Error occured while updating news");
        }

        [Route("{id}")]
        public IHttpActionResult Delete(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                Log.Information($"An error occured, news is invalid {BadRequest()}");
                return BadRequest("An error occured, news is invalid");
            }
            if (_service.Exists(id) == false)
            {
                return NotFound();
            }

            _service.Delete(id);
            return Ok("news deleted Successfully");
        }

        [Route("publish/{id}")]
        public IHttpActionResult PutPublish(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                Log.Information($"An error occured, news is invalid {BadRequest()}");
                return BadRequest("An error occured, news is invalid");
            }

            if (_service.Exists(id) == false)
            {
                return NotFound();
            }           

            _service.Publish(id);
            return Ok("news item published Successfully");
        }

        [Route("unpublish/{id}")]
        public IHttpActionResult PutUnPublish(string id)
        {
            
            if (String.IsNullOrEmpty(id))
            {
                Log.Information($"An error occured, news is invalid {BadRequest()}");
                return BadRequest("An error occured, news is invalid");
            }

            if (_service.Exists(id) == false)
            {
                return NotFound();
            }
            _service.UnPublish(id);
            return Ok("news item unpublished Successfully");
        }
    }
}