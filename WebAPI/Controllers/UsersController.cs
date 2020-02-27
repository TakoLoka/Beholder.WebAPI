using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Business.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Utilities;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        IUserService _userService;
        IAuthService _authService;

        public UsersController(IUserService userService, IAuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetCurrentUser()
        {
            return this.OperateOnEmail(_userService.GetByMail);
        }

        [HttpPost("Premium/DM")]
        [Authorize]
        public IActionResult BecomePremiumDM()
        {
            return this.OperateOnUser(_userService, _userService.BecomeDungeonMasterPremium, _authService.CreateAccessToken);
        }

        [HttpPost("Premium/Player")]
        [Authorize]
        public IActionResult BecomePremiumPlayer()
        {
            return this.OperateOnUser(_userService, _userService.BecomePlayerPremium, _authService.CreateAccessToken);
        }

        [HttpDelete("Premium/DM")]
        [Authorize]
        public IActionResult RemoveDungeonMasterPremium()
        {
            return this.OperateOnUser(_userService, _userService.RemoveDungeonMasterPremium, _authService.CreateAccessToken);
        }

        [HttpDelete("Premium/Player")]
        [Authorize]
        public IActionResult RemovePlayerPremium()
        {
            return this.OperateOnUser(_userService, _userService.RemovePlayerPremium, _authService.CreateAccessToken);
        }
    }
}