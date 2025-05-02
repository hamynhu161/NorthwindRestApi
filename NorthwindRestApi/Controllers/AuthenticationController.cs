using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using NorthwindRestApi.Services.Interfaces;
using NorthwindRestApi.Models;

namespace NorthwindRestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        public IAuthenticateService _authenticateService;

        public AuthenticationController(IAuthenticateService authenticateService)
        {
            _authenticateService = authenticateService;
        }

        //Tähän tulee Front Endin kirjautumisyritys
        [HttpPost]
        public ActionResult Post([FromBody] Credentials tunnukset)
        {
            var loggerUser = _authenticateService.Authenticate(tunnukset.Username, tunnukset.Password);

            if(loggerUser == null)
            {
                return BadRequest(new { message = "Käyttäjätunus tai salasana on vihreellinen." });
            }
            return Ok(loggerUser);      //Palautus Font Endiin (sis. vain LoggerUser luokan mukaiset kentät)
        }
    }
}
