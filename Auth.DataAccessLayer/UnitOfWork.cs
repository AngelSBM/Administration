using Administration.DataAccessLayer.Entities;
using Auth.DataAccessLayer.Abstractions;
using Auth.DataAccessLayer.Abstractions.Repos;
using Auth.DataAccessLayer.Entities;
using Auth.DataAccessLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.DataAccessLayer
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationContext _globalContext;

        public UnitOfWork(
            ApplicationContext context,
            IRepository<Session> sessionRepository,
            IRepository<Company> companyRepository,
            IRepository<Client> clientRepository,
            IRepository<Address> addressRepository)
        {
            this._globalContext = context;

            sessionRepo = sessionRepository;
            companyRepo = companyRepository;
            clientRepo = clientRepository;
            addressRepo = addressRepository;
        }


        public IRepository<Session> sessionRepo { get; set; }
        public IRepository<Company> companyRepo { get; set; }
        public IRepository<Client> clientRepo { get; set; }
        public IRepository<Address> addressRepo { get; set; }



        public void BeginTransaction()
        {
            _globalContext.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            _globalContext.Database.CommitTransaction();
        }

        public void RollbackTransaction()
        {   
            _globalContext.Database.RollbackTransaction();
        }
      
        public void Complete()
        {
            _globalContext.SaveChanges();
        }

    }
}
