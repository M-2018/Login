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

        //Revisar si el usuario y password recibido del cliente coinciden con la info en base de datos
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            var user = await loginDbContext.Logins.FirstOrDefaultAsync(x => x.UserName == loginDto.Username && x.Password == loginDto.Password);

            if (user != null)
            {
                // Mapea el usuario a un objeto LoginDTO antes de devolverlo
                var userDto = new LoginDTO
                {
                    Username = user.UserName,
                    Password = user.Password,
                    FullName = user.FullName
                };
                return Ok(userDto); // Devuelve el usuario como JSON
            }

            return BadRequest(new { message = "Invalid username or password" });
        }
             
        //Crear usuario en base de datos teniendo en cuenta lo recibido del cliente
            [HttpPost]
            [Route("create")]
            public async Task<IActionResult> CreateUser([FromBody] LoginDTO loginDto)
            {
                try
                {
                    // Mapea el LoginDTO a una entidad Login
                    var newLogin = new Login
                    {
                        Id = Guid.NewGuid(),
                        UserName = loginDto.Username,
                        Password = loginDto.Password,
                        FullName = loginDto.FullName
                    };

                    // Agrega el nuevo usuario a la base de datos
                    loginDbContext.Logins.Add(newLogin);
                    await loginDbContext.SaveChangesAsync();

                    // Devuelve el nuevo usuario creado como respuesta
                    return Ok(new LoginDTO
                    {
                        Username = newLogin.UserName,
                        Password = newLogin.Password,
                        FullName = newLogin.FullName
                    });
                }
                catch (Exception ex)
                {
                    // En caso de error, devuelve una respuesta de error
                    return BadRequest(new { message = "Error al crear el usuario: " + ex.Message });
                }
            }



        ////Get all logins
        //[HttpGet]
        //public async Task<IActionResult> GetAllLogins()
        //{
        //    var logins = await loginDbContext.Logins.ToListAsync();
        //    return Ok(logins);
        //}

        ////Get single login
        //[HttpGet]
        //[Route("{id:guid}")]
        //[ActionName("GetLogin")]
        //public async Task<IActionResult> GetLogin([FromRoute] Guid id)
        //{
        //    var login = await loginDbContext.Logins.FirstOrDefaultAsync(x => x.Id == id);
        //    if(login != null)
        //    {
        //        return Ok(login);
        //    }
        //    return NotFound("Login not found");
        //}

        ////Add login
        //[HttpPost]
        //public async Task<IActionResult> AddLogin([FromBody] Login login)
        //{
        //    login.Id = Guid.NewGuid();
        //    await loginDbContext.Logins.AddAsync(login);
        //    await loginDbContext.SaveChangesAsync();
        //    return CreatedAtAction(nameof(GetLogin), new {id = login.Id}, login);
        //}

        ////Updating a login
        //[HttpPut]
        //[Route("{id:guid}")]
        //public async Task<IActionResult> UpdateLogin([FromRoute] Guid id, [FromBody] Login login)
        //{
        //    //var existingCard = await cardsDbContext.Cards.FirstOrDefaultAsync(x => x.Id == id);

        //    //var existingLogin = await LoginDbContext.Logins.FirstOrDefaultAsync(x => x.Id == id);
        //    var existingLog = await loginDbContext.Logins.FirstOrDefaultAsync(x => x.Id == id);

        //    if (existingLog != null)
        //    {
        //        existingLog.UserName = login.UserName;
        //        existingLog.Password = login.Password;
        //        existingLog.FullName = login.FullName;                
        //        await loginDbContext.SaveChangesAsync();
        //        return Ok(existingLog);
        //    }

        //    return NotFound("Login not found");

        //}

        ////Delete a login
        //[HttpDelete]
        //[Route("{id:guid}")]
        //public async Task<IActionResult> DeleteCard([FromRoute] Guid id)
        //{
        //    var existingLog = await loginDbContext.Logins.FirstOrDefaultAsync(x => x.Id == id);
        //    if (existingLog != null)
        //    {
        //        loginDbContext.Remove(existingLog);
        //        await loginDbContext.SaveChangesAsync();
        //        return Ok(existingLog);
        //    }

        //    return NotFound("Login not found");
        //}


    }
}
