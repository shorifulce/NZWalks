using Microsoft.AspNetCore.Mvc;
using Microsoft.JSInterop;
using NZWalks.api.Repositories;

namespace NZWalks.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly ITokenHandlerRepository tokenHandlerRepository;

        public AuthController(IUserRepository userRepository,ITokenHandlerRepository tokenHandlerRepository) 
        {
            this.userRepository = userRepository;
            this.tokenHandlerRepository = tokenHandlerRepository;
        }

        [HttpPost]
        [Route("login")]
        // so the routing url will be Auth/login
        public async Task<IActionResult> Login(Models.DTO.LoginRequest loginRequest)
        {

            // validate the incomming request , here we have to validate the username and password must not be empty
            // Here we use fluient validators in LoginRequestValidator under Validators Folder
            // username and password
            var user = await userRepository.AuthenticateAsync(loginRequest.Username, loginRequest.Password);

            if (user !=null) 
            {
                // Generate aa JWT Toekn

                var token=await tokenHandlerRepository.CreateTokenAsync(user);
                return Ok(token);
            }

            return BadRequest("Username and Password is Incorrect");
        }
    }
}
