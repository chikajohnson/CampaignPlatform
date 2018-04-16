using Campaign.Business.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campaign.Business.Repositories
{
    public class NewsCommentService
    {
        private readonly CampaignEntities _db;
        private UtilityService _utilityService;
        public NewsCommentService()
        {
            _db = new CampaignEntities();
            _utilityService = new UtilityService();
        }

        public IQueryable<NewsComment> GetAll()
        {
            return _db.NewsComments.AsQueryable();
        }
        
        public NewsComment GetById(string Id)
        {
            return _db.NewsComments.SingleOrDefault(x => x.ID == Id);
        }

        public NewsComment Create(NewsComment model)
        {
            if (model == null)
            {
                return null;
            }
            
            var comment = _db.NewsComments.Add(model);
            _db.SaveChanges();

            return comment;            
        }
        public NewsComment Update(NewsComment model)
        {
            _db.NewsComments.AddOrUpdate(model);
            _db.SaveChanges();

            return model;
        }

        public NewsComment Delete(string id)
        {
            if (id == null)
            {
                return null;
            }

            var comment = GetById(id);
            _db.NewsComments.Remove(comment);
            _db.SaveChanges();
            return comment;
        }

        public IQueryable<NewsComment> GetReplies(string commentId)
        {
            return _db.NewsComments.Where(x => x.ParentID == commentId && x.IsParent == false).AsQueryable();
        }

        public NewsComment Reply(string newsId, string parentCommentId, NewsComment comment)
        {
            if (String.IsNullOrEmpty(newsId) == true || String.IsNullOrEmpty(parentCommentId) == true || comment == null)
            {
                return null;
            }

            comment.NewsID = newsId;
            comment.ParentID = parentCommentId;
            comment.IsParent = false;
            comment.ID = _utilityService.generateGuid();
            comment.CreatedAt = DateTime.Now;

            var reply = _db.NewsComments.Add(comment);
            _db.SaveChanges();
            return reply;
        }

        public bool Exists(string id)
        {
            var exists = _db.NewsComments
                .Where(x => x.ID == id)
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
