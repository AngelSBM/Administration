using Administration.LogicLayer.Abstractions;
using Administration.LogicLayer.DTOs;
using Auth.ClientLayer.Helpers.Exceptions;
using Auth.ClientLayer.Helpers.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Administration.ClientLayer.Controllers
{

    [ApiController]
    [Route("Client")]
    public class ClientController : ControllerBase
    {
        private IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            this._clientService = clientService;
        }

        [HttpPost("GetAll")]
        public IActionResult NewClient()
        {
            try
            {
                var resp = _clientService.GetClients();

                return ApiResponse.OK(resp);
            }
            catch (Exception e)
            {
                var resp = ApiResponse.CreateErrorObject(e.Message);
                return StatusCode(500, resp);
            }
        }


        [HttpPost("New")]
        public IActionResult NewClient(NewClientDTO newClient)
        {
            try
            {

                var resp = _clientService.CreateClient(newClient);


                return ApiResponse.OK(resp);
            }
            catch (Exception e)
            {
                var resp = ApiResponse.CreateErrorObject(e.Message);
                return StatusCode(500, resp);
            }
        }

        [HttpDelete("Delete")]
        public IActionResult DeleteClient(int id)
        {
            try
            {
                _clientService.DeleteClient(id);

                return ApiResponse.OK(new { message = "Client deleted!" });
            }
            catch (Exception e)
            {
                var resp = ApiResponse.CreateErrorObject(e.Message);
                return StatusCode(500, resp);
            }
        }

        [HttpPut("Update")]
        public IActionResult UpdateClient(ClientUpdateDTO updatedClient)
        {
            try
            {
                var resp = _clientService.UpdateClient(updatedClient);
                return ApiResponse.OK(resp);
            }
            catch(NotFoundException e)
            {
                var resp = ApiResponse.CreateErrorObject(e.Message);
                return ApiResponse.NotFound(resp);
            }
            catch (Exception e)
            {
                var resp = ApiResponse.CreateErrorObject(e.Message);
                return StatusCode(500, resp);
            }
        }

        [HttpDelete("DeleteAddress")]
        public IActionResult DeleteAddress(int addressId)
        {
            try
            {
                _clientService.DeleteAddress(addressId);

                return ApiResponse.OK(new { message = "Client deleted!" });
            }
            catch (Exception e)
            {
                var resp = ApiResponse.CreateErrorObject(e.Message);
                return StatusCode(500, resp);
            }
        }

    }
}
