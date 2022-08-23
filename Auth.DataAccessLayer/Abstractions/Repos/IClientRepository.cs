using Administration.DataAccessLayer.Entities;
using Auth.DataAccessLayer.Abstractions.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Administration.DataAccessLayer.Abstractions.Repos
{
    public interface IClientRepository : IRepository<Client>
    {
        public IEnumerable<Client> GetClientsDetails();
    }
}
