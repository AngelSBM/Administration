using Administration.LogicLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Administration.LogicLayer.Abstractions
{
    public interface IClientService
    {
        public IEnumerable<ClientDTO> GetClients();
        public ClientDTO CreateClient(NewClientDTO client);
        public ClientDTO UpdateClient(ClientUpdateDTO updatedClient);
        public bool DeleteClient(int id);
        public bool DeleteAddress(int id);
    }
}
