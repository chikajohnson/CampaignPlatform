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
    [RoutePrefix("api/newscomments")]
    public class NewsCommentsController : BaseController
    {
        private readonly NewsCommentService _service;
        private UtilityService _utilService;
        public NewsCommentsController()
        {
            _service = new NewsCommentService();
            _utilService = new UtilityService();
        }

        [Route("")]
        public IHttpActionResult Get()
        {
            var newsComments = _service.GetAll()
                .Select(x => new {
                    ID = x.ID,
                    Comment = x.Comment,
                    CreatedAt = x.CreatedAt,
                    PostedBy = x.postedBy,
                    ParentID = x.ParentID,
                    IsParent = x.IsParent
                })
                .OrderByDescending(x => x.CreatedAt).ToList();

            if (newsComments == null)
            {
                Log.Information($"An error occured while retrieving news items {BadRequest()}");
                return BadRequest("An error occured while retrieving news items");
            }
            return Ok(newsComments);
        }
        
        [Route("{id}")]
        public IHttpActionResult Get(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            if (_service.Exists(id) == false)
            {
                return NotFound();
            }
            var commentDTO = new List<CommentModel>();
            var newsComment = _service.GetById(id);
            if (newsComment.IsParent == true)
            {
               var replies = _service.GetReplies(id).OrderByDescending(x => x.CreatedAt).ToList();
                foreach (var item in replies)
                {
                    var com = new CommentModel
                    {
                        ID = item.ID,
                        IsParent = item.IsParent,
                        ParentID = item.ParentID,
                        Comment = item.Comment,
                        CreatedAt = item.CreatedAt,
                        postedBy = item.postedBy,
                        NewsID = item.NewsID
                    };
                    commentDTO.Add(com);
                }
            }
            var comment = new CommentModel
            {
                ID = newsComment.ID,
                IsParent = newsComment.IsParent,
                Comment = newsComment.Comment,
                CreatedAt = newsComment.CreatedAt,
                postedBy = newsComment.postedBy,
                NewsID = newsComment.NewsID,
                ParentID = newsComment.ParentID,
                Replies = commentDTO
            };
            
            if (comment != null)
            {
                return Ok(comment);
            }
            Log.Information($"Error occured while creating news {BadRequest()}");
            return BadRequest("Error occured while retreiving news item");
        }

        [Route("")]
        public IHttpActionResult Post(NewsComment model)
        {
            if (model == null)
            {
                return BadRequest("An error occured while trying to create news item.");
            }

            model.ID = _utilService.generateGuid();
            model.CreatedAt = DateTime.Now;
            model.IsParent = true;
            model.postedBy = UserRecord.Id;
            model.ParentID = null;

            var newsComment = _service.Create(model);
            if (newsComment != null)
            {
                return Ok(newsComment);
            }
            Log.Information($"Error occured while creating news comment {BadRequest()}");
            return BadRequest("Error occured while creating news comment");
        }

        [Route("")]
        public IHttpActionResult Put(NewsComment model)
        {
            if (model == null)
            {
                return BadRequest("An error occured while trying to update news item.");
            }

            var comment = _service.Update(model);
            if (comment != null)
            {
                return Ok(comment);
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

            var item = _service.Delete(id);
            return Ok(item);
        }

        [Route("reply/{newsId}/{commentId}")]
        public IHttpActionResult PostReply(string newsId, string commentId, NewsComment comment)
        {
            if (String.IsNullOrEmpty(newsId) == true || String.IsNullOrEmpty(commentId) || comment == null)
            {
                Log.Information($"An error occured, news is invalid {BadRequest()}");
                return BadRequest("An error occured, news is invalid");
            }

            _service.Reply(newsId, commentId, comment);
            return Ok("news updated Successfully");
        }
    }
}