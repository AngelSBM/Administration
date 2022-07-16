using Auth.ClientLayer.Helpers.Utilities;
using Auth.LogicLayer.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Auth.ClientLayer.Controllers
{
    [ApiController]
    [Route("Company")]
    public class CompanyController
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            this._companyService = companyService;
        }


        [HttpGet("GetCompanies")]
        [Authorize]
        public IActionResult GetAllUsers()
        {

            try
            {
                var companies = _companyService.GetCompanies();
                return ApiResponse.OK(companies);
            }
            catch (Exception e)
            {


                throw;
            }

        }

    }
}
