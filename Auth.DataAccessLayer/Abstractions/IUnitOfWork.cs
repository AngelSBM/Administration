﻿using Administration.DataAccessLayer.Abstractions.Repos;
using Administration.DataAccessLayer.Entities;
using Auth.DataAccessLayer.Abstractions.Repos;
using Auth.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.DataAccessLayer.Abstractions
{
    public interface IUnitOfWork
    {
        public IRepository<Session> sessionRepo { get; }
        public IRepository<Company> companyRepo { get; }
        public IClientRepository clientRepo { get; }
        public IRepository<Address> addressRepo { get; }


        public void BeginTransaction();
        public void CommitTransaction();
        public void RollbackTransaction();
        public void Complete();
    }
}
