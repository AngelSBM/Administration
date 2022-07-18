using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Administration.LogicLayer.DTOs
{
    public class ClientUpdateDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }          
        public IEnumerable<AddressDTO> UpdatedAddreses { get; set; }
        public IEnumerable<NewAddressDTO> NewAddresess { get; set; }

    }
}
