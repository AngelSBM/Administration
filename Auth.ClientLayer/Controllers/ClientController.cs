using Administration.LogicLayer.Abstractions;
using Administration.LogicLayer.DTOs;
using Auth.ClientLayer.Helpers.Utilities;
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

        [HttpPost("New")]
        public IActionResult NewClient(ClientDTO newClient)
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

    }
}
