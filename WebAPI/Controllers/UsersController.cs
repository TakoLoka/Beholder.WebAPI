using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Business.Abstract;
using Core.Dtos.UserDtos;
using Core.Entities.Models;
using Core.Utilities.Results;
using Core.Utilities.Security.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Services.Cache;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        IUserService _userService;
        IAuthService _authService;
        ICacheService _cacheService;

        public UsersController(IUserService userService, IAuthService authService, ICacheService cacheService)
        {
            _userService = userService;
            _authService = authService;
            _cacheService = cacheService;
        }

        #region Private Methods
        private IDataResult<AccessToken> RefreshToken(User user)
        {
            var accessToken = Request.Headers["Authorization"];
            var res = _authService.CreateAccessToken(user);
            if (res.Success)
            {
                _cacheService.SetKey("ExpiredToken" + Guid.NewGuid().ToString(), accessToken.ToString(), TimeSpan.FromDays(1));
            }

            return res;
        }
        #endregion

        #region Routes
        [HttpPost("Premium/DM")]
        [Authorize]
        public IActionResult BecomePremiumDM()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                string userEmail = claims.First(x => x.Type == ClaimTypes.Email).Value;
                var user = _userService.GetByMail(userEmail).Data;
                var result = _userService.BecomeDungeonMasterPremium(user);
                if (!result.Success)
                {
                    return BadRequest(result.Message);
                }

                var refreshedTokenResult = RefreshToken(user);

                if (!refreshedTokenResult.Success)
                {
                    return BadRequest(refreshedTokenResult.Message);
                }

                return Ok(refreshedTokenResult.Data);
            }

            return BadRequest();
        }

        [HttpGet("Premium/IsDM")]
        [Authorize]
        public IActionResult IsDungeonMaster()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                string userEmail = claims.First(x => x.Type == ClaimTypes.Email).Value;
                var user = _userService.GetByMail(userEmail).Data;
                var result = _userService.IsDungeonMasterPremium(user);

                return Ok(result.Success);
            }

            return BadRequest();
        }

        [HttpGet("Premium/IsPlayerPremium")]
        [Authorize]
        public IActionResult IsPlayerPremium()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                string userEmail = claims.First(x => x.Type == ClaimTypes.Email).Value;
                var user = _userService.GetByMail(userEmail).Data;
                var result = _userService.IsPlayerPremium(user);

                return Ok(result.Success);
            }

            return BadRequest();
        }

        [HttpGet("CurrentUser")]
        [Authorize]
        public IActionResult GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                string userEmail = claims.First(x => x.Type == ClaimTypes.Email).Value;
                var result = _userService.GetByMail(userEmail);
                if (!result.Success)
                {
                    return BadRequest(result.Message);
                }
                return Ok(new UserRepresentationDto(result.Data));
            }

            return BadRequest();
        }

        [HttpPost("Premium/Player")]
        [Authorize]
        public IActionResult BecomePremiumPlayer()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                string userEmail = claims.First(x => x.Type == ClaimTypes.Email).Value;
                var user = _userService.GetByMail(userEmail).Data;
                var result = _userService.BecomePlayerPremium(user);
                if (!result.Success)
                {
                    return BadRequest(result.Message);
                }

                var refreshedTokenResult = RefreshToken(user);

                if (!refreshedTokenResult.Success)
                {
                    return BadRequest(refreshedTokenResult.Message);
                }

                return Ok(refreshedTokenResult.Data);
            }

            return BadRequest();
        }

        [HttpDelete("Premium/DM")]
        [Authorize]
        public IActionResult RemoveDungeonMasterPremium()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                string userEmail = claims.First(x => x.Type == ClaimTypes.Email).Value;
                var user = _userService.GetByMail(userEmail).Data;
                var result = _userService.RemoveDungeonMasterPremium(user);
                if (!result.Success)
                {
                    return BadRequest(result.Message);
                }

                var refreshedTokenResult = RefreshToken(user);

                if (!refreshedTokenResult.Success)
                {
                    return BadRequest(refreshedTokenResult.Message);
                }

                return Ok(refreshedTokenResult.Data);
            }

            return BadRequest();
        }

        [HttpDelete("Premium/Player")]
        [Authorize]
        public IActionResult RemovePlayerPremium()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                string userEmail = claims.First(x => x.Type == ClaimTypes.Email).Value;
                var user = _userService.GetByMail(userEmail).Data;
                var result = _userService.RemovePlayerPremium(user);
                if (!result.Success)
                {
                    return BadRequest(result.Message);
                }

                var refreshedTokenResult = RefreshToken(user);

                if (!refreshedTokenResult.Success)
                {
                    return BadRequest(refreshedTokenResult.Message);
                }

                return Ok(refreshedTokenResult.Data);
            }

            return BadRequest();
        }
        #endregion
    }
}