using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Business.Abstract;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        IUserService _userService;
        IAuthService _authService;

        public UserController(IUserService userService, IAuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        [HttpPost("Premium/DM")]
        [Authorize]
        public ActionResult BecomePremiumDM()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if(identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                string userEmail = claims.First(x => x.Type == ClaimTypes.Email).Value;
                var user = _userService.GetByMail(userEmail).Data;
                var result = _userService.BecomeDungeonMasterPremium(user);
                if (!result.Success)
                {
                    return BadRequest(result.Message);
                }

                var createdToken = _authService.CreateAccessToken(user);
                if (!createdToken.Success)
                {
                    return BadRequest(createdToken.Message);
                }

                return Ok(createdToken);
            }

            return BadRequest();
        }

        [HttpPost("Premium/Player")]
        [Authorize]
        public ActionResult BecomePremiumPlayer()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                string userEmail = claims.First(x => x.Type == ClaimTypes.Email).Value;
                var result = _userService.BecomePlayerPremium(_userService.GetByMail(userEmail).Data);
                if (!result.Success)
                {
                    return BadRequest(result.Message);
                }

                var createdToken = _authService.CreateAccessToken(user);
                if (!createdToken.Success)
                {
                    return BadRequest(createdToken.Message);
                }

                return Ok(createdToken);
            }

            return BadRequest();
        }
    }
}