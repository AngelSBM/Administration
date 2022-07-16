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
        public ClientDTO CreateClient(ClientDTO client);
    }
}
