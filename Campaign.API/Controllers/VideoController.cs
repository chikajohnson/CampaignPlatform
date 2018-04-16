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
    [RoutePrefix("api/videos")]
    public class VideoController : BaseController
    {
        private readonly VideoService _service;
        private readonly UtilityService _utilService;
        public VideoController()
        {
            _service = new VideoService();
            _utilService = new UtilityService();
        }

        [Route("")]
        public IHttpActionResult Get()
        {
            var videos = _service.GetAll().Select(x => new
            {
                ID = x.ID,
                Title = x.Title,
                Description = x.Description,
                Url = x.Url,
                LgaID = x.LgaID,
                LgaName = x.LgaName,
                Town = x.Town,
                //PublishedBy = x.PublishedBy,
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
            }).ToList();

            if (videos == null)
            {
                Log.Information($"An error occured while retrieving videos {BadRequest()}");
                return BadRequest("An error occured while retrieving videos");
            }
            return Ok(videos);
        }

        [Route("{id}")]
        public IHttpActionResult Get(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return BadRequest("invalid video item");
            }
            var video = _service.GetById(id);
            if (video == null)
            {
                return NotFound();
            }
            var videoItem = new Video
            {
                ID = video.ID,
                Title = video.Title,
                Description = video.Description,
                Url = video.Url,
                LgaID = video.LgaID,
                LgaName = video.LgaName,
                Town = video.Town,
                PublishedBy = video.PublishedBy,
                PublishedAt = video.PublishedAt,
                CategoryName = video.CategoryName,
                CountryID = video.CountryID,
                CountryName = video.CountryName,
                StateName = video.StateName,
                StateID = video.StateID,
                CreatedAt = video.CreatedAt,
                CreatedBy = video.CreatedBy,
                IsPublished = video.IsPublished,
                Shared = video.Shared
            };

            if (videoItem != null)
            {
                return Ok(videoItem);
            }
            Log.Information($"Error occured while creating video {BadRequest()}");
            return BadRequest("Error occured while retreiving video item");
        }


        [Route("category/{id}")]
        public IHttpActionResult GetCategory(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return BadRequest("invalid video item");
            }
            var videos = _service.GetByCategory(id)
                .Select(x => new
                {
                    ID = x.ID,
                    Title = x.Title,
                    Description = x.Description,
                    Url = x.Url,
                    LgaID = x.LgaID,
                    LgaName = x.LgaName,
                    Town = x.Town,
                   // PublishedBy = x.PublishedBy,
                    PublishedAt = x.PublishedAt,
                    CategoryID = x.CategoryID,
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
            if (videos == null)
            {
                return NotFound();
            }

            if (videos != null)
            {
                return Ok(videos);
            }
            Log.Information($"Error occured while retreiving video {BadRequest()}");
            return BadRequest("Error occured while retreiving videos");
        }

        [Route("")]
        public IHttpActionResult Post(Video model)
        {
            if (model == null)
            {
                return BadRequest("An error occured while trying to create video item.");
            }
            model.CreatedBy = UserRecord.Id;
            model.CreatedAt = DateTime.Now;
            model.ID = _utilService.generateGuid();
            model.PublishedBy = UserRecord.Id;
            model.PublishedAt = DateTime.Now;
            var video = _service.Create(model);
            if (video != null)
            {
                return Ok(video);
            }
            Log.Information($"Error occured while creating video item {BadRequest()}");
            return BadRequest("Error occured while creating video item");
        }

        [Route("")]
        public IHttpActionResult Put(VideoModel model)
        {
            if (model == null)
            {
                return BadRequest("An error occured while trying to update video item.");
            }

            if (_service.GetById(model.ID) == null)
            {
                return BadRequest("video item is invalid");
            }

            var video = new Video
            {
                ID = model.ID,
                Title = model.Title,
                Description = model.Description,
                Url = model.Url,
                LgaID = model.LgaID,
                LgaName = model.LgaName,
                Town = model.Town,
                Type = model.Type,
                //PublishedBy = model.PublishedBy,
                PublishedAt = model.PublishedAt,
                CategoryID = model.CategoryID,
                Likes = model.Likes,
                Dislikes = model.Dislikes,
                CategoryName = model.CategoryName,
                CountryID = model.CountryID,
                CountryName = model.CountryName,
                StateName = model.StateName,
                StateID = model.StateID,
                CreatedAt = model.CreatedAt,
                CreatedBy = model.CreatedBy,
                IsPublished = model.IsPublished,
                Shared = model.Shared
            };
            video = _service.Update(video);
            if (video != null)
            {
                return Ok(video);
            }
            Log.Information($"Error occured while updating video {BadRequest()}");
            return BadRequest("Error occured while updating video");
        }

        [Route("{id}")]
        public IHttpActionResult Delete(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                Log.Information($"An error occured, video is invalid {BadRequest()}");
                return BadRequest("An error occured, video is invalid");
            }
            if (_service.Exists(id) == false)
            {
                return NotFound();
            }

            var video = _service.Delete(id);
            return Ok(video);
        }

        [Route("publish/{id}")]
        public IHttpActionResult PutPublish(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                Log.Information($"An error occured, video is invalid {BadRequest()}");
                return BadRequest("An error occured, video is invalid");
            }

            if (_service.Exists(id) == false)
            {
                return NotFound();
            }

            _service.Publish(id);
            return Ok("video item published Successfully");
        }

        [Route("unpublish/{id}")]
        public IHttpActionResult PutUnPublish(string id)
        {

            if (String.IsNullOrEmpty(id))
            {
                Log.Information($"An error occured, video is invalid {BadRequest()}");
                return BadRequest("An error occured, video is invalid");
            }

            if (_service.Exists(id) == false)
            {
                return NotFound();
            }
            _service.UnPublish(id);
            return Ok("video item unpublished Successfully");
        }
    }
}