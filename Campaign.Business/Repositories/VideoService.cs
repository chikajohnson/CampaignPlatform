using Campaign.Business.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campaign.Business.Repositories
{
    public class VideoService
    {
        private readonly CampaignEntities _db;
        private UtilityService _utilityService;
        public VideoService()
        {
            _db = new CampaignEntities();
            _utilityService = new UtilityService();
        }

        public IQueryable<Video> GetAll()
        {
            return _db.Videos.AsQueryable();
        }

        public IQueryable<Video> GetByCategory(string categoryId)
        {
            return _db.Videos.Where(v => v.CategoryID == categoryId).AsQueryable();
        }
        public Video GetById(string Id)
        {
            return _db.Videos.SingleOrDefault(x => x.ID == Id);
        }

        public Video Create(Video model)
        {
            if (model == null)
            {
                return null;
            }
           
            var video = _db.Videos.Add(model);
            _db.SaveChanges();

            return video;            
        }
        public Video Update(Video model)
        {
           _db.Videos.AddOrUpdate(model);
            _db.SaveChanges();

            return model;
        }

        public Video Delete(string id)
        {
            if (id == null)
            {
                return null;
            }
            var video = GetById(id);
            _db.Videos.Remove(video);
            _db.SaveChanges();

            return video;
        }

        public bool Publish(string newsId)
        {
            bool published;
            var video = GetById(newsId);
            if (video != null)
            {
                video.IsPublished = true;
                published = true;
                //news.PublishedAt = DateTime.Now;
                _db.SaveChanges();
            }
            else
            {
                published = false;
            }
            return published;
        }

        public bool UnPublish(string newsId)
        {
            bool unPublished;
            var video = GetById(newsId);
            if (video != null)
            {
                video.IsPublished = false;
                //news.PublishedAt = DateTime.Now;
                unPublished = false;
                _db.SaveChanges();
            }
            else
            {
                unPublished = false;
            }
            return unPublished;
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
            var exists = _db.Videos
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
    }
}
