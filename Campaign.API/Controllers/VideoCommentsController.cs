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
    [RoutePrefix("api/videocomments")]
    public class VideoCommentsController : BaseController
    {
        private readonly VideoCommentService _service;
        private UtilityService _utilService;

        public VideoCommentsController()
        {
            _service = new VideoCommentService();
            _utilService = new UtilityService();
        }

        [Route("")]
        public IHttpActionResult Get()
        {
            var videoComments = _service.GetAll()
                .Select(x => new {
                    ID = x.ID,
                    Comment = x.Comment,
                    CreatedAt = x.CreatedAt,
                    PostedBy = x.postedBy,
                    ParentID = x.ParentID,
                    IsParent = x.IsParent
                })
                .OrderByDescending(x => x.CreatedAt).ToList();

            if (videoComments == null)
            {
                Log.Information($"An error occured while retrieving video items {BadRequest()}");
                return BadRequest("An error occured while retrieving video items");
            }
            return Ok(videoComments);
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
            var videoComment = _service.GetById(id);
            if (videoComment.IsParent == true)
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
                        VideoID = item.VideoID
                    };
                    commentDTO.Add(com);
                }
            }
            var comment = new CommentModel
            {
                ID = videoComment.ID,
                IsParent = videoComment.IsParent,
                Comment = videoComment.Comment,
                CreatedAt = videoComment.CreatedAt,
                postedBy = videoComment.postedBy,
                VideoID = videoComment.VideoID,
                ParentID = videoComment.ParentID,
                Replies = commentDTO
            };

            if (comment != null)
            {
                return Ok(comment);
            }
            Log.Information($"Error occured while creating video {BadRequest()}");
            return BadRequest("Error occured while retreiving video item");
        }

        [Route("")]
        public IHttpActionResult Post(VideoComment model)
        {
            if (model == null)
            {
                return BadRequest("An error occured while trying to create video item.");
            }

            model.ID = _utilService.generateGuid();
            model.CreatedAt = DateTime.Now;
            model.IsParent = true;
            model.postedBy = UserRecord.Id;
            model.ParentID = null;

            var videoComment = _service.Create(model);
            if (videoComment != null)
            {
                return Ok(videoComment);
            }
            Log.Information($"Error occured while creating video comment {BadRequest()}");
            return BadRequest("Error occured while creating video comment");
        }

        [Route("")]
        public IHttpActionResult Put(VideoComment model)
        {
            if (model == null)
            {
                return BadRequest("An error occured while trying to update video item.");
            }

            var comment = _service.Update(model);
            if (comment != null)
            {
                return Ok(comment);
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

            var item = _service.Delete(id);
            return Ok(item);
        }

        [Route("reply/{videoId}/{commentId}")]
        public IHttpActionResult PostReply(string videoId, string commentId, VideoComment comment)
        {
            if (String.IsNullOrEmpty(videoId) == true || String.IsNullOrEmpty(commentId) || comment == null)
            {
                Log.Information($"An error occured, video is invalid {BadRequest()}");
                return BadRequest("An error occured, video is invalid");
            }

            _service.Reply(videoId, commentId, comment);
            return Ok("video updated Successfully");
        }
    }
}