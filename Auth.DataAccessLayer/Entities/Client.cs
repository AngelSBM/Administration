using Auth.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Administration.DataAccessLayer.Entities
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public Guid PublicId { get; set; }
        public int CompanyId { get; set; }

        #region Relationships
        public Company Company { get; set; }
        public ICollection<Address> Addresses { get; set; }
        #endregion


    }
}
