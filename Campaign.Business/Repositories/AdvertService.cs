using Campaign.Business.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campaign.Business.Repositories
{
    public class AdvertService
    {
        private readonly CampaignEntities _db;
        private UtilityService _utilityService;
        public AdvertService()
        {
            _db = new CampaignEntities();
            _utilityService = new UtilityService();
        }

        public IQueryable<Advert> GetAll()
        {
            return _db.Adverts.AsQueryable();
        }

       
        public Advert GetById(string Id)
        {
            return _db.Adverts.SingleOrDefault(x => x.ID == Id);
        }

        public Advert Create(Advert model)
        {
            if (model == null)
            {
                return null;
            }
            
            var news = _db.Adverts.Add(model);
            _db.SaveChanges();
            return news;            
        }
        public Advert Update(Advert model)
        {   
            _db.Adverts.AddOrUpdate(model);
            _db.SaveChanges();

            return model;
        }

        public Advert Delete(string id)
        {
            if (id == null)
            {
                return null;
            }
            var advert = GetById(id);
            advert = _db.Adverts.Remove(advert);
            _db.SaveChanges();

            return advert;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && _db != null)
            {
                _db.Dispose();
            }

            disposing = true;
        }

        public bool Exists(string title)
        {
            var exists = _db.Adverts
                .Where(x => x.Title == title)
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
    }
}
