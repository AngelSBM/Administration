using Administration.DataAccessLayer.Entities;
using Administration.LogicLayer.Abstractions;
using Administration.LogicLayer.DTOs;
using Auth.ClientLayer.Helpers.Exceptions;
using Auth.DataAccessLayer.Abstractions;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Administration.LogicLayer.Services
{
    public class ClientService : IClientService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ClientService(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }


        public IEnumerable<ClientDTO> GetClients(int companyId)
        {
            var clients = _unitOfWork.clientRepo.GetClientsDetails(companyId);

            List<ClientDTO> clientsResponse = new List<ClientDTO>();
            foreach (var client in clients)
            {
                clientsResponse.Add(_mapper.Map<Client, ClientDTO>(client));
            }

            return clientsResponse;

        }

        public ClientDTO CreateClient(NewClientDTO newClient)
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
                    clientDB.Addresses.Add(address);
                }

                _unitOfWork.CommitTransaction();


                return _mapper.Map<Client, ClientDTO>(clientDB);


            }
            catch (Exception e)
            {
                _unitOfWork.RollbackTransaction();

                throw new Exception("Something went wrong creating the client, please consult IT department");
            }

        }

        public ClientDTO UpdateClient(ClientUpdateDTO updatedClient)
        {

            try
            {
                var clientDB = _unitOfWork.clientRepo.Find(client => client.Id == updatedClient.Id);
                if (clientDB == null)
                {
                    throw new NotFoundException("User not found");
                }

                clientDB.Name = updatedClient.Name;
                clientDB.Phone = updatedClient.Phone;
                clientDB.Email = updatedClient.Email;

                _unitOfWork.BeginTransaction();

                if (updatedClient.UpdatedAddreses.Count() > 0)
                {
                    foreach (var updateAddress in updatedClient.UpdatedAddreses)
                    {
                        var addressDB = _unitOfWork.addressRepo.Find(address => address.Id == updateAddress.Id);
                        addressDB.AddressName = updateAddress.Address;

                        clientDB.Addresses.Add(addressDB);                    
                    }
                }

                if (updatedClient.NewAddresess.Count() > 0)
                {
                    foreach (var newAddress in updatedClient.NewAddresess)
                    {
                        var newAddressDB = new Address{ AddressName = newAddress.Address, ClientId = clientDB.Id };
                        _unitOfWork.addressRepo.Add(newAddressDB);

                        clientDB.Addresses.Add(newAddressDB);
                    }
                }

                _unitOfWork.Complete();
                _unitOfWork.CommitTransaction();

                return _mapper.Map<Client, ClientDTO>(clientDB);

            }
            catch (Exception e)
            {
                _unitOfWork.RollbackTransaction();
                throw;
            }

        }

        public bool DeleteClient(int id)
        {
            var clientDB = _unitOfWork.clientRepo.Find(client => client.Id == id);
            if (clientDB == null)
            {
                throw new NotFoundException("User not found");
            }



            try
            {
                _unitOfWork.BeginTransaction();

                IEnumerable<Address> clientAddreses = _unitOfWork.addressRepo.FindAll(address => address.ClientId == id);
                if(clientAddreses.Count() > 0)
                {
                    foreach (var address in clientDB.Addresses)
                    {
                        _unitOfWork.addressRepo.Remove(address);
                    }
                }

                _unitOfWork.clientRepo.Remove(clientDB);

                _unitOfWork.Complete();

                _unitOfWork.CommitTransaction();

                return true;


            }
            catch (Exception e)
            {
                _unitOfWork.RollbackTransaction();
                throw new Exception("Something went wrong creating the client, please consult IT department");
            }


   
        }


        public bool DeleteAddress(int id)
        {
            
            var addressDB = _unitOfWork.addressRepo.Find(address => address.Id == id);

            _unitOfWork.addressRepo.Remove(addressDB);

            _unitOfWork.Complete();

            return true;

        }
    }
}
