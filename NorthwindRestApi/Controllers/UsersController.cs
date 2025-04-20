using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NorthwindRestApi.Models;

namespace NorthwindRestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly NorthwindContext db;

        public UsersController(NorthwindContext context)
        {
            db =context;
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            var users = db.Users;

            foreach (var user in users)
            {
                user.Password = null;       //ensuring that no password data is exposed in API response
            }

            return Ok(users);
        }

        [HttpPost]
        public ActionResult PostCreateNew([FromBody] User u)
        {
            try
            {
                db.Users.Add(u);
                db.SaveChanges();
                return Ok("Lisättiin käyttäjä " + u.Username);
            }
            catch (Exception ex)
            {
                return BadRequest("lisääminen ei onnistunut. Tässä lisätietoa: " + ex);
            }
        }
    }

    
}
