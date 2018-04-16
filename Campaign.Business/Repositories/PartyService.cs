using Campaign.Business.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campaign.Business.Repositories
{
    public class PartyService
    {
        private readonly CampaignEntities _db;
        private UtilityService _utilityService;
        public PartyService()
        {
            _db = new CampaignEntities();
            _utilityService = new UtilityService();
        }

        public IQueryable<Party> GetAll()
        {
            return _db.Parties.AsQueryable();
        }

       
        public Party GetById(string Id)
        {
            return _db.Parties.SingleOrDefault(x => x.ID == Id);
        }

        public Party Create(Party model)
        {
            if (model == null)
            {
                return null;
            }
            model.ID = Guid.NewGuid().ToString();
            //model.CreatedAt = DateTime.Now;

            var news = _db.Parties.Add(model);
            _db.SaveChanges();
            return news;            
        }
        public Party Update(Party party)
        {
            _db.Parties.AddOrUpdate(party);
            _db.SaveChanges();

            return party;
        }

        public Party Delete(string id)
        {
            if (id == null)
            {
                return null;
            }
            var party = GetById(id);
            _db.Parties.Remove(party);
            _db.SaveChanges();

            return party;
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
            var exists = _db.Parties
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

        public bool CheckNameExists(string name)
        {
            var exists = _db.Parties
                .Where(x => x.Name == name || x.Acronym == name)
                .SingleOrDefault();

            if (exists == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
