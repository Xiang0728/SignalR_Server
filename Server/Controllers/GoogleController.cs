using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Server.Controllers.BroadCastController;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GoogleController : Controller
    {
        [HttpGet]
        public string Start()
        {
            return "Server Start";
        }

        [AllowAnonymous]
        [HttpGet("UserInfo")]
        public async Task<IActionResult> GetUserInfo(string? Token)
        {

            var url = "https://oauth2.googleapis.com/tokeninfo?id_token=" + Token;
            using (var client = new HttpClient())
            {
                var res = await client.GetAsync(url);
                string result = await res.Content.ReadAsStringAsync();

                var obj = System.Text.Json.JsonSerializer.Deserialize<Users.GoogleUser>(result);

                var jwt = new JWT();
                var APIToken = jwt.EncryptionJWT(obj.given_name, obj.email);
                return Ok(APIToken);

            }

        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("GetUsers")]
        public IActionResult Test()
        {
          
            return Ok(UserHandler.ConnectedIds);


        }



    }
}
