using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Administration.LogicLayer.DTOs
{
    public class ClientDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int CompanyId { get; set; }

        public IEnumerable<AddressDTO> Addresses { get; set; }
    }
}
