﻿using Administration.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.DataAccessLayer.Entities
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public byte[]? Salt { get; set; }
        public string Password { get; set; }
        public Guid PublicId { get; set; }

        #region relationships
        public ICollection<Client> Clients { get; set; }
        #endregion

    }
}
