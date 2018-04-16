using Campaign.Business.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campaign.Business.Repositories
{
    public class CandidateService
    {
        private readonly CampaignEntities _db;
        public CandidateService()
        {
            _db = new CampaignEntities();
        }

        public IQueryable<CandidateView> GetAll()
        {
            return _db.CandidateViews.AsQueryable();
        }
        public Candidate GetById(string Id)
        {
            return _db.Candidates.SingleOrDefault(x => x.ID == Id);
        }

        public CandidateView GetSingleViewById(string Id)
        {
            return _db.CandidateViews.SingleOrDefault(x => x.ID == Id);
        }

        public Candidate Create(Candidate model)
        {
            if (model == null)
            {
                return null;
            }
            var candidate = _db.Candidates.Add(model);
            _db.SaveChanges();

            return candidate;            
        }
        public Candidate Update(Candidate model)
        {
            _db.Candidates.AddOrUpdate(model);
            _db.SaveChanges();

            return model;
        }

        public Candidate Delete(string id)
        {
            if (id == null)
            {
                return null;
            }
            var candidate = GetById(id);
            _db.Candidates.Remove(candidate);
            _db.SaveChanges();

            return candidate;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && _db != null)
            {
                _db.Dispose();
            }

            disposing = true;
        }
    }
}
