using Campaign.Business.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campaign.Business.Repositories
{
    public class LocationService
    {
        private readonly CampaignEntities _db;
        public LocationService()
        {
            _db = new CampaignEntities();
        }

        public IQueryable<Country> GetCountries()
        {
            return _db.Countries.AsQueryable();
        }

        public IQueryable<State> GetStates()
        {
            return _db.States.AsQueryable();
        }

        public IQueryable<Lga> GetLgas()
        {
            return _db.Lgas.AsQueryable();
        }

        public IQueryable<State> GetStates(int countryID)
        {
            return _db.States.Where(x => x.CountryID == countryID).AsQueryable();
        }

        public IQueryable<Lga> GetLgas(int stateID)
        {
            return _db.Lgas.Where(x => x.StateID == stateID).AsQueryable();
        }
       
    }
}
