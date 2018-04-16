using Campaign.Business.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
