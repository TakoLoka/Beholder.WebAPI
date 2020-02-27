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
using WebAPI.Services.Cache.Redis;
using WebAPI.Utilities;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        IUserService _userService;
        IAuthService _authService;
        IRedisService _redisService;

        public UsersController(IUserService userService, IAuthService authService, IRedisService redisService)
        {
            _userService = userService;
            _authService = authService;
            _redisService = redisService;
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetCurrentUser()
        {
            return this.OperateOnEmail(_userService.GetByMail);
        }

        private IDataResult<AccessToken> RefreshToken(User user)
        {
            var accessToken = Request.Headers["Authorization"];
            var res = _authService.CreateAccessToken(user);
            if (res.Success)
            {
                _redisService.SetKey("ExpiredToken" + Guid.NewGuid().ToString(), accessToken.ToString(), TimeSpan.FromDays(1));
            }

            return res;
        }

        private bool CheckToken()
        {
            var token = Request.Headers["Authorization"];
            var blackList = _redisService.GetAll("ExpiredToken");
            return string.IsNullOrEmpty(blackList.Find(expToken => expToken.Equals(token)));
        }

        [HttpPost("Premium/DM")]
        [Authorize]
        public IActionResult BecomePremiumDM()
        {
            if (!CheckToken())
            {
                return Unauthorized("Token Expired");
            }
            return this.OperateOnUser(_userService, _userService.BecomeDungeonMasterPremium, RefreshToken);
        }

        [HttpPost("Premium/Player")]
        [Authorize]
        public IActionResult BecomePremiumPlayer()
        {
            if (!CheckToken())
            {
                return Unauthorized("Token Expired");
            }
            return this.OperateOnUser(_userService, _userService.BecomePlayerPremium, RefreshToken);
        }

        [HttpDelete("Premium/DM")]
        [Authorize]
        public IActionResult RemoveDungeonMasterPremium()
        {
            if (!CheckToken())
            {
                return Unauthorized("Token Expired");
            }
            return this.OperateOnUser(_userService, _userService.RemoveDungeonMasterPremium, RefreshToken);
        }

        [HttpDelete("Premium/Player")]
        [Authorize]
        public IActionResult RemovePlayerPremium()
        {
            if (!CheckToken())
            {
                return Unauthorized("Token Expired");
            }
            return this.OperateOnUser(_userService, _userService.RemovePlayerPremium, RefreshToken);
        }
    }
}