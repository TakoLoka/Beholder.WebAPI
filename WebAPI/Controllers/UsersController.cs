using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Business.Abstract;
using Core.Entities.Models;
using Core.Utilities.Results;
using Core.Utilities.Security.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Services.Cache;
using WebAPI.Utilities;

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
            return this.OperateOnUser(_userService, _userService.BecomeDungeonMasterPremium, RefreshToken);
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetCurrentUser()
        {
            return this.OperateOnEmail(_userService.GetByMail);
        }

        [HttpPost("Premium/Player")]
        [Authorize]
        public IActionResult BecomePremiumPlayer()
        {
            return this.OperateOnUser(_userService, _userService.BecomePlayerPremium, RefreshToken);
        }

        [HttpDelete("Premium/DM")]
        [Authorize]
        public IActionResult RemoveDungeonMasterPremium()
        {
            return this.OperateOnUser(_userService, _userService.RemoveDungeonMasterPremium, RefreshToken);
        }

        [HttpDelete("Premium/Player")]
        [Authorize]
        public IActionResult RemovePlayerPremium()
        {
            return this.OperateOnUser(_userService, _userService.RemovePlayerPremium, RefreshToken);
        }
        #endregion
    }
}