using Campaign.Business.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campaign.Business.Repositories
{
    public class NewsService
    {
        private readonly CampaignEntities _db;
        private UtilityService _utilityService;
        public NewsService()
        {
            _db = new CampaignEntities();
            _utilityService = new UtilityService();
        }

        public IQueryable<NewsView> GetAll()
        {
            var news = _db.NewsViews.AsQueryable();
            return news;
        }

        public IQueryable<NewsView> GetByCategory(string categoryId)
        {
            var news = _db.NewsViews.Where(n => n.CategoryID == categoryId);
                return news.AsQueryable(); ;
        }
        public News GetById(string Id)
        {
            return _db.News.SingleOrDefault(x => x.ID == Id);
        }

        public NewsView GetSingleViewById(string Id)
        {
            return _db.NewsViews.SingleOrDefault(x => x.ID == Id);
        }

        public News Create(News model)
        {
            if (model == null)
            {
                return null;
            }
           
            var news = _db.News.Add(model);
            _db.SaveChanges();
            return news;            
        }
        public News Update(News model)
        {
            _db.News.AddOrUpdate(model);
            _db.SaveChanges();

            return model;
        }

        public News Delete(string id)
        {
            if (id == null)
            {
                return null;
            }
            var news = GetById(id);
            _db.News.Remove(news);
            _db.SaveChanges();

            return news;
        }

        public bool Publish(string newsId)
        {
            bool published;
            var news = GetById(newsId);
            if (news != null)
            {
                news.IsPublished = true;
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
            var news = GetById(newsId);
            if (news != null)
            {
                news.IsPublished = false;
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
            var exists = _db.News
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
