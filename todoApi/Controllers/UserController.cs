using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using todoApi.Models;

namespace todoApi.Controllers {

    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase {

        private readonly UserService _userService;

        public UserController(UserService userService) {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> UserLogin(LoginDTO user) {
            try
            {
                var result = await _userService.UserLogin(user);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Ocorreu um erro ao processar a solicitação." });
            }
        }

        [HttpGet("getall")]
        public async Task<ActionResult> GetUsers() {
            IEnumerable<UserModel> users = await _userService.GetUsers();
            return Ok(users);
        }

        [HttpPost("register")]
        public async Task<IActionResult> UserRegister(UserDTO user) {
            var userCreated = await _userService.UserRegister(user);

            if (userCreated == null) {
                return BadRequest("Não foi possível criar o usuário!");
            }

            return Ok(userCreated);
        }
    }
}