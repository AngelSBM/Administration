using Administration.DataAccessLayer.Entities;
using Administration.LogicLayer.DTOs;
using Auth.DataAccessLayer.Entities;
using Auth.LogicLayer.DTOs;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.LogicLayer.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Company, CompanyDTO>().ReverseMap();

            CreateMap<Client, NewClientDTO>().ReverseMap();
            CreateMap<Client, ClientDTO>()
                .ForMember(x => x.Addresses, options => options.MapFrom(MapClientAddress));

            //CreateMap<Company, UserDetailDTO>()
            //    .ForMember(x => x.Roles, options => options.MapFrom(MapUserRole)).ReverseMap();
        }

        private List<AddressDTO> MapClientAddress(Client client, ClientDTO clientDTO)
        {
            var addresses = new List<AddressDTO>();
            if(client.Addresses == null) { return addresses; }
            foreach(var address in client.Addresses)
            {
                addresses.Add(new AddressDTO
                {
                    Id = address.Id,
                    Address = address.AddressName
                });
            }

            return addresses;

        }

        //private List<RoleDTO> MapUserRole(Company user, UserDetailDTO userDetailDTO)
        //{

        //    var roles = new List<RoleDTO>();
        //    if(user.UsersRoles == null) {  return roles; }
        //    foreach (var userRole in user.UsersRoles)
        //    {
        //        roles.Add(new RoleDTO
        //        {
        //            Id = userRole.Role.PublicId,
        //            Name = userRole.Role.Name,
        //            Description = userRole.Role.Description,
        //        });
        //    }

        //    return roles;

        //}

    }
}
