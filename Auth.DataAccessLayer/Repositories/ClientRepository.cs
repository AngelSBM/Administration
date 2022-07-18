using Administration.DataAccessLayer.Abstractions.Repos;
using Administration.DataAccessLayer.Entities;
using Auth.DataAccessLayer;
using Auth.DataAccessLayer.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Administration.DataAccessLayer.Repositories
{
    public class ClientRepository : Repository<Client>, IClientRepository
    {
        private readonly ApplicationContext _context;

        public ClientRepository(ApplicationContext context) : base(context)
        {
            this._context = context;
        }

        public IEnumerable<Client> GetClientsDetails(int companyId)
        {
           return _context.Set<Client>()
                .Where(client => client.CompanyId == companyId)
                .Include(c => c.Addresses);
        }
    }
}
