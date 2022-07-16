using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Administration.DataAccessLayer.Entities
{
    public class Address
    {
        public  int Id { get; set; }
        public string AddressName { get; set; }
        public int ClientId { get; set; }

        #region relationships
        public Client Client { get; set; }
        #endregion
    }
}
