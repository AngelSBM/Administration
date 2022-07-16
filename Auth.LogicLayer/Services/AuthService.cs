
using Auth.DataAccessLayer;
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
using System.Net;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using Auth.DataAccessLayer.Abstractions;
using Auth.ClientLayer.Helpers.Exceptions;
using Store.LogicLayer.Helpers.Exceptions;
using Administration.LogicLayer.DTOs;

namespace Auth.LogicLayer.Services
{
    public class AuthService : IAuthService
    {

        IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;  
            this._configuration = configuration;
            this._httpContextAccessor = httpContextAccessor;
        }


        public CompanyDTO RegisterCompany(CompanyRegisterDTO newCompany)
        {

            validateNewUser(newCompany);

            Company companyDB = new Company();
            companyDB.Name = newCompany.Name;
            companyDB.Email = newCompany.Email;
            companyDB.Salt = generateSalt();
            companyDB.Password = hashPassword(newCompany.Password, companyDB.Salt);
            companyDB.PublicId = Guid.NewGuid();

            _unitOfWork.companyRepo.Add(companyDB);
            _unitOfWork.Complete();

            return _mapper.Map<Company, CompanyDTO>(companyDB);
        }

        public CompanyCrendentialsDTO Login(CompanyLoginDTO companyUser)
        {
            Company userDB = _unitOfWork.companyRepo.Find(u => u.Email == companyUser.Email);
            if (userDB == null)
            {
                throw new NotFoundException("Company not found");
            }

            var hashLoginPassword = hashPassword(companyUser.Password, userDB.Salt);

            if (!(hashLoginPassword == userDB.Password))
            {
                throw new BadRequestException("Incorrect password");
            }


            string accessToken = createToken(userDB);
            string refreshToken = createSession(userDB);

            _unitOfWork.Complete();

            return new CompanyCrendentialsDTO
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };

        }


        public bool Logout()
        {
            var refreshToken = _httpContextAccessor.HttpContext.Request.Cookies["refreshToken"];

            var session = _unitOfWork.sessionRepo.Find(session => session.Token.ToString() == refreshToken);

            //foreach (var session in collection)
            //{

            //}

            return true;
        }


        public CompanyCrendentialsDTO RefreshSession()
        {
            var refreshToken = _httpContextAccessor.HttpContext.Request.Cookies["refreshToken"];

            var session = _unitOfWork.sessionRepo.Find(session => session.Token.ToString() == refreshToken);

            if(session == null || (DateTime.Now > session.ExpiresAt))
            {
                throw new NotAuthorizedException("Refresh token not valid");
            }

            var newSessionCredentials = new CompanyCrendentialsDTO();
            var userDB = _unitOfWork.companyRepo.GetById(session.CompanyId);

            newSessionCredentials.AccessToken = createToken(userDB);
            newSessionCredentials.RefreshToken = createSession(userDB);

            _unitOfWork.sessionRepo.Remove(session);
            _unitOfWork.Complete();

            return newSessionCredentials;
        }

        /// <summary>
        /// Create a new session in the database
        /// </summary>
        /// <param name="user"></param>
        /// <returns>refresh-token of the session</returns>
        private string createSession(Company company)
        {
            try
            {
                var session = new Session()
                {
                    Token = Guid.NewGuid(),
                    CompanyId = company.Id,
                    CreatedAt = DateTime.Now,
                    ExpiresAt = DateTime.Now.AddDays(7),
                };

                _unitOfWork.sessionRepo.Add(session);            

                return session.Token.ToString();

            }
            catch (Exception)
            {

                throw;
            }

        }

        private void validateNewUser(CompanyRegisterDTO newCompany)
        {

            bool userExists = _unitOfWork.companyRepo.Exists(user => user.Email == newCompany.Email);
            if (userExists)
            {
                throw new BadRequestException("User already exists!");
            }

        }


        private string createToken(Company user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim("UID", user.Id.ToString())
            };

            string secretKey = _configuration.GetSection("AppSettings:Token").Value;

            var key = new SymmetricSecurityKey
                (System.Text.Encoding.UTF8.GetBytes(secretKey));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);


            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: creds);

            string jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private byte[] generateSalt()
        {
            byte[] salt = new byte[128 / 8];
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetNonZeroBytes(salt);
            }

            return salt;
        }

        private string hashPassword(string password, byte[] salt)
        {
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));

            return hashed;
        }


    }
}
