using Auth.DataAccessLayer.Entities;
using Auth.LogicLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.LogicLayer.Abstractions
{
    public interface ICompanyService
    {
        public IEnumerable<CompanyDTO> GetCompanies();
    }
}
