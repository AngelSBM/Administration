using Administration.DataAccessLayer.Abstractions.Repos;
using Administration.DataAccessLayer.Entities;
using Auth.DataAccessLayer.Abstractions;
using Auth.DataAccessLayer.Abstractions.Repos;
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
            IClientRepository clientRepository,
            IRepository<Address> addressRepository)
        {
            this._globalContext = context;

            clientRepo = clientRepository;
            addressRepo = addressRepository;
        }


        public IClientRepository clientRepo { get; set; }
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
