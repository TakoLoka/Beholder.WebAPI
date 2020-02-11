using System;
using System.Collections.Generic;
using System.Linq;
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

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("Premium/DM")]
        [Authorize]
        public ActionResult BecomePremiumDM()
        {
            _userService.BecomeDungeonMasterPremium(_userService.GetByMail("tako.n.kta@gmail.com"));
            return Ok(new SuccessResult("DM Premium"));
        }

        [HttpPost("Premium/Player")]
        [Authorize]
        public ActionResult BecomePremiumPlayer()
        {
            _userService.BecomePlayerPremium(_userService.GetByMail("tako.n.kta@gmail.com"));
            return Ok(new SuccessResult("DM Premium"));
        }

    }
}