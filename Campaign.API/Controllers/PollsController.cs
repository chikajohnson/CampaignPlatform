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
    [RoutePrefix("api/polls")]
    public class PollsController : BaseController
    {
        private readonly PollService _service;
        private readonly PollParticipantService _pollParticipantService;
        private readonly UtilityService _utilService;
        public PollsController()
        {
            _service = new PollService();
            _pollParticipantService = new PollParticipantService();
            _utilService = new UtilityService();
        }

        [Route("")]
        public IHttpActionResult Get()
        {
            var polls = _service.GetAll().Select(x => new {
                ID = x.ID,
                OpinionQuestion = x.OpinionQuestion,
                Category = x.Category,
                Title = x.Title,
                NumberOfAnswerOptions = x.NumberOfAnswerOptions,
                OpinionAnswerOptionA = x.OpinionAnswerOptionA,
                OpinionAnswerOptionB = x.OpinionAnswerOptionB,
                OpinionAnswerOptionC = x.OpinionAnswerOptionC,
                OpinionAnswerOptionD = x.OpinionAnswerOptionD,
                OpinionAnswerOptionE = x.OpinionAnswerOptionE,
                OpinionAnswerOptionACount = x.OpinionAnswerOptionACount,
                OpinionAnswerOptionBCount = x.OpinionAnswerOptionBCount,
                OpinionAnswerOptionCCount = x.OpinionAnswerOptionCCount,
                OpinionAnswerOptionDCount = x.OpinionAnswerOptionDCount,
                OpinionAnswerOptionECount = x.OpinionAnswerOptionECount,
                StartDate = x.StartDate,
                EndDate = x.StartDate,
                CreatedAt = x.CreatedAt,
                CreatedBy = x.CreatedBy,
                IsPublished = x.IsPublished,
                //PublishedBy = x.PublishedBy,
                PublishedAt = x.PublishedAt

            }).ToList();

            if (polls == null)
            {
                Log.Information($"An error occured while retrieving polls {BadRequest()}");
                return BadRequest("An error occured while retrieving polls. ");
            }
            return Ok(polls);
        }
        
        [Route("{id}")]
        public IHttpActionResult Get(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return NotFound();
            }           
            var poll = _service.GetById(id);
            if (poll == null)
            {
                return NotFound();
            }

            var pollItem = new Poll
            {
                ID = poll.ID,
                OpinionQuestion = poll.OpinionQuestion,
                Category = poll.Category,
                Title = poll.Title,
                NumberOfAnswerOptions = poll.NumberOfAnswerOptions,
                OpinionAnswerOptionA = poll.OpinionAnswerOptionA,
                OpinionAnswerOptionB = poll.OpinionAnswerOptionB,
                OpinionAnswerOptionC = poll.OpinionAnswerOptionC,
                OpinionAnswerOptionD = poll.OpinionAnswerOptionD,
                OpinionAnswerOptionE = poll.OpinionAnswerOptionE,
                OpinionAnswerOptionACount = poll.OpinionAnswerOptionACount,
                OpinionAnswerOptionBCount = poll.OpinionAnswerOptionBCount,
                OpinionAnswerOptionCCount = poll.OpinionAnswerOptionCCount,
                OpinionAnswerOptionDCount = poll.OpinionAnswerOptionDCount,
                OpinionAnswerOptionECount = poll.OpinionAnswerOptionECount,
                StartDate = poll.StartDate,
                EndDate = poll.StartDate,
                CreatedAt = poll.CreatedAt,
                CreatedBy = poll.CreatedBy,
                IsPublished = poll.IsPublished,
                //PublishedBy = poll.PublishedBy,
                PublishedAt = poll.PublishedAt
            };

            if (pollItem != null)
            {
                return Ok(pollItem);
            }
            Log.Information($"Error occured while retreiving poll {BadRequest()}");
            return BadRequest("Error occured while retreiving poll.");
        }

        [Route("")]
        public IHttpActionResult Post(Poll model)
        {
            if (model == null)
            {
                return BadRequest("An error occured while trying to create poll.");
            }
            model.CreatedBy = UserRecord.Id;
            model.ID = _utilService.generateGuid();
            model.PublishedBy = UserRecord.Id;
            model.IsPublished = false;
            model.CreatedAt = DateTime.Now;
            model.PublishedAt = null;
            model.PublishedBy = null;
            var poll = _service.Create(model);
            if (poll != null)
            {
                return Ok(poll);
            }
            Log.Information($"Error occured while creating poll - {BadRequest()}");
            return BadRequest("Error occured while creating poll.");
        }

        [Route("participate")]
        public IHttpActionResult PostParticipate(PollParticipation model)
        {
            if (model == null)
            {
                return BadRequest("An error occured while trying to create poll.");
            }
           
            if (_service.GetById(model.ID) == null)
            {
                return BadRequest("Poll is invalid");
            }
            model.UserId = UserRecord.Id;
            model.ID = _utilService.generateGuid();
            model.ParticipationDate = DateTime.Now;
            var participation = _pollParticipantService.Participate(model);
            if (participation != null)
            {
                return Ok(participation);
            }
            Log.Information($"Error occured while participating in poll - {BadRequest()}");
            return BadRequest("Error occured while participating in poll.");
        }

        [Route("")]
        public IHttpActionResult Put(Poll poll)
        {
            if (poll == null)
            {
                return BadRequest("An error occured while trying to update poll.");
            }
            
            if (_service.GetById(poll.ID) == null)
            {
                return BadRequest("poll is invalid");
            }

            var pollItem = new Poll
            {
                ID = poll.ID,
                OpinionQuestion = poll.OpinionQuestion,
                Category = poll.Category,
                Title = poll.Title,
                NumberOfAnswerOptions = poll.NumberOfAnswerOptions,
                OpinionAnswerOptionA = poll.OpinionAnswerOptionA,
                OpinionAnswerOptionB = poll.OpinionAnswerOptionB,
                OpinionAnswerOptionC = poll.OpinionAnswerOptionC,
                OpinionAnswerOptionD = poll.OpinionAnswerOptionD,
                OpinionAnswerOptionE = poll.OpinionAnswerOptionE,
                OpinionAnswerOptionACount = poll.OpinionAnswerOptionACount,
                OpinionAnswerOptionBCount = poll.OpinionAnswerOptionBCount,
                OpinionAnswerOptionCCount = poll.OpinionAnswerOptionCCount,
                OpinionAnswerOptionDCount = poll.OpinionAnswerOptionDCount,
                OpinionAnswerOptionECount = poll.OpinionAnswerOptionECount,
                StartDate = poll.StartDate,
                EndDate = poll.StartDate,
                CreatedAt = poll.CreatedAt,
                CreatedBy = poll.CreatedBy,
                IsPublished = poll.IsPublished,
                //PublishedBy = poll.PublishedBy,
                PublishedAt = poll.PublishedAt
            };

            pollItem = _service.Update(pollItem);
            if (poll != null)
            {
                return Ok(pollItem);
            }

            Log.Information($"Error occured while updating  poll {BadRequest()}");
            return BadRequest("Error occured while updating poll");
        }

        [Route("publish/{pollId}")]
        public IHttpActionResult PutPublish(string pollId)
        {
            if (pollId == null)
            {
                return BadRequest("An error occured while trying to publish poll.");
            }

            if (_service.GetById(pollId) == null)
            {
                return BadRequest("poll is invalid");
            }

            var poll = _service.GetById(pollId);
            poll.IsPublished = true;

            poll = _service.Update(poll);
            if (poll != null)
            {
                return Ok(poll);
            }

            Log.Information($"Error occured while updating  poll {BadRequest()}");
            return BadRequest("Error occured while updating poll");
        }

        [Route("{id}")]
        public IHttpActionResult Delete(string id)
        {
            if (String.IsNullOrEmpty(id) == true)
            {
                Log.Information($"An error occured,  poll is invalid {BadRequest()}");
                return BadRequest("An error occured,  poll  invalid");
            }
            
            if (_service.Exists(id) == false)
            {
                return NotFound();
            }

            var poll =_service.Delete(id);
            return Ok(poll);
        }
    }
}