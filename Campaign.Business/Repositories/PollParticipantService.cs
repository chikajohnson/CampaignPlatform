using Campaign.Business.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campaign.Business.Repositories
{
    public class PollParticipantService
    {
        private readonly CampaignEntities _db;
        public PollParticipantService()
        {
            _db = new CampaignEntities();
        }

        public PollParticipation Participate(PollParticipation participation)
        {
                var part = _db.PollParticipations.Add(participation);
                _db.SaveChanges();
                return part;
            
        }

    }
}
