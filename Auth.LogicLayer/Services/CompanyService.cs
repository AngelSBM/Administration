﻿using Auth.DataAccessLayer;
using Auth.DataAccessLayer.Abstractions.Repos;
using Auth.DataAccessLayer.Entities;
using Auth.LogicLayer.Abstractions;
using Auth.LogicLayer.DTOs;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using Auth.DataAccessLayer.Abstractions;

namespace Auth.LogicLayer.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper mapper;
        private readonly IConfiguration _configuration;

        public CompanyService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IConfiguration configuration)
        {
            this._unitOfWork = unitOfWork;
            this.mapper = mapper;
            this._configuration = configuration;
        }

        public IEnumerable<CompanyDTO> GetCompanies()
        {
            var companiesDB = _unitOfWork.companyRepo.GetAll();

            var users = mapper.Map<IEnumerable<CompanyDTO>>(companiesDB);
            return users;
        }

        //public IEnumerable<UserDetailDTO> GetUsersDetail()
        //{
        //    var usersDB = _unitOfWork.userRepo.GetUsersWithRoles().ToList();

        //    var resp = mapper.Map<IEnumerable<Company>, IEnumerable<UserDetailDTO>>(usersDB);

        //    return resp;
        //    //var users = mapper.Map<IEnumerable<UserDTO>>(usersDB);
        //    //return users;
        //}


    }
}
