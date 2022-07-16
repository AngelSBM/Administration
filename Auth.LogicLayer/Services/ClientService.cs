using Administration.DataAccessLayer.Entities;
using Administration.LogicLayer.Abstractions;
using Administration.LogicLayer.DTOs;
using Auth.DataAccessLayer.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Administration.LogicLayer.Services
{
    public class ClientService : IClientService
    {
        private IUnitOfWork _unitOfWork;

        public ClientService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public ClientDTO CreateClient(ClientDTO newClient)
        {

            var clientDB = new Client();
            clientDB.Name = newClient.Name;
            clientDB.Email = newClient.Email;
            clientDB.Phone = newClient.Phone;
            clientDB.CompanyId = newClient.CompanyId;

            try
            {

                _unitOfWork.BeginTransaction();

                _unitOfWork.clientRepo.Add(clientDB);
                _unitOfWork.Complete();

                foreach (var newClientAddress in newClient.Addresses)
                {
                    Address address = new Address
                    {
                        AddressName = newClientAddress.Address,
                        ClientId = clientDB.Id
                    };

                    _unitOfWork.addressRepo.Add(address);
                    _unitOfWork.Complete();
                }

                _unitOfWork.CommitTransaction();


                return newClient;
                

            }
            catch (Exception e)
            {
                _unitOfWork.RollbackTransaction();

                throw new Exception("Something went wrong creating the client, please consult IT department");
            }

        }
    }
}
