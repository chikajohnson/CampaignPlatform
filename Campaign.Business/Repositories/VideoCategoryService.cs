using Campaign.Business.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campaign.Business.Repositories
{
    public class VideoCategoryService
    {
        private readonly CampaignEntities _db;
        private UtilityService _utilityService;
        public VideoCategoryService()
        {
            _db = new CampaignEntities();
            _utilityService = new UtilityService();
        }

        public IQueryable<VideoCategory> GetAll()
        {
            return _db.VideoCategories.AsQueryable();
        }
        public VideoCategory GetById(string Id)
        {
            return _db.VideoCategories.SingleOrDefault(x => x.ID == Id);
        }

        public VideoCategory Create(VideoCategory model)
        {
            if (model == null)
            {
                return null;
            }
            var cat = _db.VideoCategories.Add(model);
            _db.SaveChanges();

            return cat;
        }
        public VideoCategory Update(VideoCategory model)
        {
            _db.VideoCategories.AddOrUpdate(model);
            _db.SaveChanges();

            return model;
        }

        public VideoCategory Delete(string id)
        {
            if (id == null)
            {
                return null;
            }
            var category = GetById(id);
            _db.VideoCategories.Remove(category);
            _db.SaveChanges();

            return category;
        }

        public bool Exists(string name)
        {
            var categoryExist = _db.VideoCategories.Any(x => x.Name.ToLower().Trim() == name.ToLower().Trim());
            if (categoryExist)
            {
                return true;
            }
            return false;

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

