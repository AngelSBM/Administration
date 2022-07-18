using Auth.LogicLayer.Abstractions;
using Auth.LogicLayer.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Auth.ClientLayer.Helpers.Exceptions;
using Auth.ClientLayer.Helpers.Utilities;
using Store.LogicLayer.Helpers.Exceptions;
using Administration.LogicLayer.DTOs;

namespace Auth.ClientLayer.Controllers
{

    [ApiController]
    [Route("Auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;        
        public AuthController(IAuthService _authService) 
        {
            this._authService = _authService;
        }

        
        [HttpPost("Register")]
        public IActionResult Register(CompanyRegisterDTO newCompany)
        {
            try
            {
                var  createdUser = _authService.RegisterCompany(newCompany);
                return ApiResponse.OK(createdUser);

            }
            catch (BadRequestException e)
            {
                var resp = ApiResponse.CreateErrorObject(e.Message);
                return ApiResponse.BadRequest(resp); 
            }   
            catch (Exception e)
            {
                var resp = ApiResponse.CreateErrorObject(e.Message);
                return StatusCode(500, resp);
            }                        
        }

        [HttpPost("Login")]
        public IActionResult Login(CompanyLoginDTO companyUser)
        {         
            try
            {
                CompanyCrendentialsDTO tokens = _authService.Login(companyUser);

                //var cookieOptions = new CookieOptions { HttpOnly = true };
                //Response.Cookies.Append("refreshToken", tokens.RefreshToken, cookieOptions);


                return ApiResponse.OK(new
                {
                    Message = "You are logged!",
                    AccessToken = tokens.AccessToken,
                    RefreshToken = tokens.RefreshToken
                });
            }
            catch (BadRequestException e)
            {
                var resp = ApiResponse.CreateErrorObject(e.Message);
                return ApiResponse.BadRequest(resp);
            }
            catch (NotFoundException e)
            {
                var resp = ApiResponse.CreateErrorObject(e.Message);
                return ApiResponse.NotFound(resp);
            }
            catch(Exception e)
            {
                var resp = ApiResponse.CreateErrorObject(e.Message);
                return StatusCode(500, resp);
            }
        }

        [HttpGet("refreshToken")]        
        public IActionResult RefreshToken(string refreshToken)
        {

            try
            {
                CompanyCrendentialsDTO newTokens = _authService.RefreshSession(refreshToken);

                //var cookieOptions = new CookieOptions { HttpOnly = true };
                //Response.Cookies.Append("refreshToken", newTokens.RefreshToken, cookieOptions);

                return ApiResponse.OK(new
                {
                    Message = "Token refreshed",
                    AccessToken = newTokens.AccessToken,
                    RefreshToken = newTokens.RefreshToken
                }); 
            }
            catch (NotAuthorizedException e)
            {
                var resp = ApiResponse.CreateErrorObject(e.Message);
                return ApiResponse.Unauthorized(resp);
            }
            catch (Exception e)
            {
                var resp = ApiResponse.CreateErrorObject(e.Message);
                return StatusCode(500, resp);
            }

        }


        //TODO: FINISH LOGOUT IMPLEMENTATION
        //[HttpPost]
        //public IActionResult Logout()
        //{
        //    try
        //    {



        //    }
        //    catch (Exception e)
        //    {
        //        var resp = ApiResponse.CreateErrorObject(e.Message);
        //        return StatusCode(500, resp);
        //    }
        //}


    }
}
