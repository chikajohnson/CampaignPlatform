using Campaign.Business.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Campaign.Business.Repositories
{
    public class PollService
    {
        private readonly CampaignEntities _db;
        private UtilityService _utilityService;
        public PollService()
        {
            _db = new CampaignEntities();
            _utilityService = new UtilityService();
        }

        public IQueryable<Poll> GetAll()
        {
            return _db.Polls.AsQueryable();
        }
        
        public Poll GetById(string Id)
        {
            return _db.Polls.SingleOrDefault(x => x.ID == Id);
        }

        public Poll Create(Poll model)
        {
            if (model == null)
            {
                return null;
            }
            
            var polls = _db.Polls.Add(model);
            _db.SaveChanges();
            return polls;            
        }
        public Poll Update(Poll poll)
        {
            _db.Polls.AddOrUpdate(poll);
            _db.SaveChanges();

            return poll;
        }

        public Poll Delete(string id)
        {
            if (id == null)
            {
                return null;
            }
            var poll = GetById(id);
            poll = _db.Polls.Remove(poll);
            _db.SaveChanges();

            return poll;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && _db != null)
            {
                _db.Dispose();
            }

            disposing = true;
        }

        public bool Exists(string id)
        {
            var exists = _db.Polls
                .Where(x => x.ID == id)
                .SingleOrDefault();

            if (exists == null )
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void UpdatePollOptionCount(string option)
        {
            if (option == null)
            {
                return;
            }
            //switch (option)
            //{
            //    case "OpinionOptionAnswerA":
            //        var count = _db.Polls.Where(x => x.)
            //    default:
            //}
        }

        private int UpdateValue(int x)
        {
            return x + 1;
        }

        public Poll GetPollToDisplay()
        {
            return _db.Polls.FirstOrDefault(x => x.IsPublished == true && x.EndDate > DateTime.Now);
        }

        public bool HasParticipated(PollParticipation model)
        {
            var participation = _db.PollParticipations
                .SingleOrDefault(x => x.UserId == model.UserId && x.PollId == model.PollId);
            if (participation == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public PollStat GetPollStat(string pollId)
        {
            var pollStat = new PollStat();
            var OpinionStatList = new List<Dictionary<int, OptionStat>>();
           

            var possiblePollAnswerCount = _db.Polls.Where(x => x.ID == pollId).SingleOrDefault().NumberOfAnswerOptions;

            string[] answerOptions = { "OpinionAnswerOptionA", "OpinionAnswerOptionB", "OpinionAnswerOptionC", "OpinionAnswerOptionD", "OpinionAnswerOptionE" };
            for (int i = 0; i < possiblePollAnswerCount; i++)
            {
                var oStatDictionary = new Dictionary<int, OptionStat>();
                var opinionStat = new OptionStat();

                opinionStat.OpinionAnswerText = GetSelectedPollAnswerOptionText(pollId,answerOptions[i]);
                opinionStat.OpinionParticipantCount = GetParticipantCountByAnswerOption(pollId, answerOptions[i]);
                opinionStat.OpinionParticipantPercent = GetOpinionParticipantPercent(pollId, answerOptions[i]);

                oStatDictionary.Add(i, opinionStat);
                OpinionStatList.Add(oStatDictionary);
            }


            pollStat.OptionStatistics = OpinionStatList;
            pollStat.TotalParticipationStat = GetTotalParticipant(pollId);

            return pollStat;
        }

        private int GetTotalParticipant(string pollId)
        {
            return _db.PollParticipations.Count(x  => x.PollId == pollId);
        }

        private string GetSelectedPollAnswerOptionText(string pollId, string pollAnswerOption)
        {
          var pollParticipation =  _db.PollParticipations
                .Where(x => x.PollId == pollId && 
                x.SelectedPollAnswerOption == pollAnswerOption).FirstOrDefault();
            return (pollParticipation == null) ? GetDefaultAnswerOptionText(pollId, pollAnswerOption) : pollParticipation.SelectedPollAnswerOptionText ;
        }

        private string GetDefaultAnswerOptionText(string pollId, string pollAnswerOption)
        {
            var pollAnswerOptions = _db.Polls.Where(x => x.ID == pollId).SingleOrDefault();
            var result = "";
            switch (pollAnswerOption)
            {
                case "OpinionAnswerOptionA":
                    result = pollAnswerOptions.OpinionAnswerOptionA;
                    break;
                case "OpinionAnswerOptionB":
                    result = pollAnswerOptions.OpinionAnswerOptionB;
                    break;
                case "OpinionAnswerOptionC":
                    result = pollAnswerOptions.OpinionAnswerOptionC;
                    break;
                case "OpinionAnswerOptionD":
                    result = pollAnswerOptions.OpinionAnswerOptionD;
                    break;
                case "OpinionAnswerOptionE":
                    result = pollAnswerOptions.OpinionAnswerOptionE;
                    break;
            }

            return result;
        }

        private decimal GetParticipantCountByAnswerOption(string pollId, string pollAnswerOption)
        {
            var pollParticipationCount =  _db.PollParticipations
                .Where(x => x.PollId == pollId &&
                x.SelectedPollAnswerOption == pollAnswerOption).Count();
            return pollParticipationCount;
        }

        private decimal GetTotalParticipantCount(string pollId)
        {
            var pollParticipationCount = _db.PollParticipations
                .Where(x => x.PollId == pollId).Count();
            return pollParticipationCount;
        }

        private decimal GetOpinionParticipantPercent(string pollId, string pollAnswerOption)
        {
            var totalParticipantByAnswerOption = GetParticipantCountByAnswerOption(pollId, pollAnswerOption);
            var totalParticipant = GetTotalParticipant(pollId);
            decimal percent = 0;
            if (totalParticipant != 0)
            {
                percent = (totalParticipantByAnswerOption / totalParticipant) * 100;
            }
           
            return percent;
        }
    }    

    public class PollStat
    {
        public List<Dictionary<int, OptionStat>> OptionStatistics { get; set; }
        public decimal TotalParticipationStat { get; set; }
    }
    public class OptionStat
    {
        public string OpinionAnswerText { get; set; }
        public decimal OpinionParticipantCount { get; set; }
        public decimal OpinionParticipantPercent { get; set; }

    }
}
