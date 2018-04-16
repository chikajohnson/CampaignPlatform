using Campaign.Business.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campaign.Business.Repositories
{
    public class VideoCommentService
    {
        private readonly CampaignEntities _db;
        private UtilityService _utilityService;
        public VideoCommentService()
        {
            _db = new CampaignEntities();
            _utilityService = new UtilityService();
        }

        public IQueryable<VideoComment> GetAll()
        {
            return _db.VideoComments.AsQueryable();
        }
        
        public VideoComment GetById(string Id)
        {
            return _db.VideoComments.SingleOrDefault(x => x.ID == Id);
        }

        public VideoComment Create(VideoComment model)
        {
            if (model == null)
            {
                return null;
            }
            model.ID = _utilityService.generateGuid();            
            //model.CreatedAt = DateTime.Now;

            var comment = _db.VideoComments.Add(model);
            _db.SaveChanges();

            return comment;            
        }
        public VideoComment Update(VideoComment comment)
        {
            _db.VideoComments.AddOrUpdate(comment);
            _db.SaveChanges();

            return comment;
        }

        public VideoComment Delete(string id)
        {
            if (id == null)
            {
                return null;
            }
            var comment = GetById(id);
            _db.VideoComments.Remove(comment);
            _db.SaveChanges();

            return comment;
        }
        public IQueryable<VideoComment> GetReplies(string commentId)
        {
            return _db.VideoComments.Where(x => x.ParentID == commentId && x.IsParent == false).AsQueryable();
        }

        public VideoComment Reply(string videoId, string parentCommentId, VideoComment comment)
        {
            if (String.IsNullOrEmpty(videoId) == true || String.IsNullOrEmpty(parentCommentId) == true || comment == null)
            {
                return null;
            }

            comment.VideoID = videoId;
            comment.ParentID = parentCommentId;
            comment.IsParent = false;
            comment.ID = _utilityService.generateGuid();
            comment.CreatedAt = DateTime.Now;

            var reply = _db.VideoComments.Add(comment);
            _db.SaveChanges();
            return reply;
        }

        public bool Exists(string id)
        {
            var exists = _db.VideoComments
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
