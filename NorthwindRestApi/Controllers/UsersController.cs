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

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var user = db.Users.Find(id);
                if (user != null)
                {
                    db.Users.Remove(user);
                    db.SaveChanges();
                    return Ok("User " + user.Firstname + " poistettiin.");
                }
                return NotFound($"Käyttäjää id:llä {id} ei löytynyt.");
            }
            catch (Exception e)
            {
                return BadRequest("Tapahtui virhe. Lue lisää: " + e.InnerException);
            }
        }

        [HttpPut("{id}")]
        public ActionResult EditUser(int id, [FromBody] User user)
        {
            try
            {
                var käyttäjä = db.Users.Find(id);
                if (käyttäjä != null)
                {
                    käyttäjä.Firstname = user.Firstname;
                    käyttäjä.Lastname = user.Lastname;
                    käyttäjä.Email = user.Email;
                    käyttäjä.AccesslevelId = user.AccesslevelId;

                    db.SaveChanges();
                    return Ok("Muokattu käyttäjää " + user.Firstname);
                }
                return NotFound("Käyttäjä ei löytynyt id:llä " + id);
            }
            catch (Exception e)
            {
                return BadRequest("Tapahtui virhe. Lue lisää: " + e.InnerException);
            }
        }
    }

    
}
