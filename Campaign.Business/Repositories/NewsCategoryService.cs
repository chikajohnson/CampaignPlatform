using Campaign.Business.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campaign.Business.Repositories
{
    public class NewsCategoryService
    {
        private readonly CampaignEntities _db;
        public NewsCategoryService()
        {
            _db = new CampaignEntities();
        }

        public IQueryable<NewsCategory> GetAll()
        {
            return _db.NewsCategories.AsQueryable();
        }
        public NewsCategory GetById(string Id)
        {
            return _db.NewsCategories.SingleOrDefault(x => x.ID == Id);
        }

        public NewsCategory Create(NewsCategory model)
        {
            if (model == null)
            {
                return null;
            }
            var cat = _db.NewsCategories.Add(model);
            _db.SaveChanges();

            return cat;
        }
        public NewsCategory Update(NewsCategory model)
        {
            if (model == null)
            {
                return null;
            }
            _db.NewsCategories.AddOrUpdate(model);
            _db.SaveChanges();
            return model;
        }

        public NewsCategory Delete(string id)
        {
            if (id == null)
            {
                return null;
            }
            var category = GetById(id);
            _db.NewsCategories.Remove(category);
            _db.SaveChanges();

            return category;
        }

        public bool Exists(string name)
        {
            var categoryExist = _db.NewsCategories.Any(x => x.Name.ToLower().Trim() == name.ToLower().Trim());
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

