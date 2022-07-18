using Administration.LogicLayer.DTOs;
using Auth.LogicLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.LogicLayer.Abstractions
{
    public interface IAuthService
    {
        public CompanyDTO RegisterCompany(CompanyRegisterDTO newCompany);
        public CompanyCrendentialsDTO Login(CompanyLoginDTO company);
        public CompanyCrendentialsDTO RefreshSession(string refreshToken);
        public bool Logout();
    }
}
