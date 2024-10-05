using LoginService.Models;
using Microsoft.AspNetCore.Mvc;

namespace LoginService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        private static readonly List<string> Users = new()
        {
            "John Doe", "Jane Smith", "Alex Johnson"
        };

        public LoginController(ILogger<LoginController> logger)
        {
            _logger = logger;
        }

        // GET: api/users
        [HttpGet]
        public ActionResult<IEnumerable<string>> GetUsers()
        {
            return Ok(Users);
        }

        // GET: api/users/2
        [HttpGet("{id}")]
        public ActionResult<string> GetUser(int id)
        {
            if (id < 0 || id >= Users.Count)
                return NotFound("User not found");

            return Ok(Users[id]);
        }

        // POST: api/users
        [HttpPost]
        public ActionResult AddUser([FromBody] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("Name cannot be empty");

            Users.Add(name);
            return Ok("User added successfully");
        }

        // DELETE: api/users/2
        [HttpDelete("{id}")]
        public ActionResult DeleteUser(int id)
        {
            if (id < 0 || id >= Users.Count)
                return NotFound("User not found");

            Users.RemoveAt(id);
            return Ok("User deleted successfully");
        }
    }
}
