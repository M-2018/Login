using LoginAPI.Data;
using LoginAPI.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LoginAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private readonly LoginDbContext loginDbContext;

        public LoginController(LoginDbContext loginDbContext)
        {
            this.loginDbContext = loginDbContext;
        }

        //// POST para verificar el inicio de sesión
        //[HttpPost]
        //[Route("login")]
        //public async Task<IActionResult> Login([FromBody] UserLoginDTO userLoginDTO)
        //{
        //    var login = await loginDbContext.Logins.FirstOrDefaultAsync(x => x.UserName == userLoginDTO.Username && x.Password == userLoginDTO.Password);

        //    if (login != null)
        //    {
        //        return Ok("Login successful");
        //    }

        //    return BadRequest("Invalid username or password");
        //}

        [HttpGet]
        [Route("login")]
        public async Task<IActionResult> Login(string username, string password)
        {
            var login = await loginDbContext.Logins.FirstOrDefaultAsync(x => x.UserName == username && x.Password == password);

            if (login != null)
            {
                return Ok(new { message = "Login successful" });
            }

            return BadRequest(new { message = "Invalid username or password" });
        }




        //Get all logins
        [HttpGet]
        public async Task<IActionResult> GetAllLogins()
        {
            var logins = await loginDbContext.Logins.ToListAsync();
            return Ok(logins);
        }

        //Get single login
        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetLogin")]
        public async Task<IActionResult> GetLogin([FromRoute] Guid id)
        {
            var login = await loginDbContext.Logins.FirstOrDefaultAsync(x => x.Id == id);
            if(login != null)
            {
                return Ok(login);
            }
            return NotFound("Login not found");
        }

        //Add login
        [HttpPost]
        public async Task<IActionResult> AddLogin([FromBody] Login login)
        {
            login.Id = Guid.NewGuid();
            await loginDbContext.Logins.AddAsync(login);
            await loginDbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetLogin), new {id = login.Id}, login);
        }

        //Updating a login
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateLogin([FromRoute] Guid id, [FromBody] Login login)
        {
            //var existingCard = await cardsDbContext.Cards.FirstOrDefaultAsync(x => x.Id == id);

            //var existingLogin = await LoginDbContext.Logins.FirstOrDefaultAsync(x => x.Id == id);
            var existingLog = await loginDbContext.Logins.FirstOrDefaultAsync(x => x.Id == id);

            if (existingLog != null)
            {
                existingLog.UserName = login.UserName;
                existingLog.Password = login.Password;
                existingLog.FullName = login.FullName;                
                await loginDbContext.SaveChangesAsync();
                return Ok(existingLog);
            }

            return NotFound("Login not found");

        }

        //Delete a login
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteCard([FromRoute] Guid id)
        {
            var existingLog = await loginDbContext.Logins.FirstOrDefaultAsync(x => x.Id == id);
            if (existingLog != null)
            {
                loginDbContext.Remove(existingLog);
                await loginDbContext.SaveChangesAsync();
                return Ok(existingLog);
            }

            return NotFound("Login not found");
        }


    }
}
