using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campaign.Business.Repositories
{
    public class UtilityService
    {
        public UtilityService()
        {

        }

        public string generateGuid()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
