using Business.Abstract;
using Core.Entities.Models;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebAPI.Utilities
{
    public static class TokenUserExtensions
    {
        public static IActionResult OperateOnEmail(this ControllerBase controller, params Func<string, IResult>[] checkFunctions)
        {
            var identity = controller.HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                string userEmail = claims.First(x => x.Type == ClaimTypes.Email).Value;
                IResult result = new ErrorResult();
                foreach (var fn in checkFunctions)
                {
                    result = fn.Invoke(userEmail);
                    if (!result.Success)
                    {
                        return controller.BadRequest(result.Message);
                    }
                }
                return controller.Ok(result);
            }

            return controller.BadRequest();
        }

        public static IActionResult OperateOnUser(this ControllerBase controller, IUserService _userService, params Func<User, IResult>[] checkFunctions)
        {
            var identity = controller.HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                string userEmail = claims.First(x => x.Type == ClaimTypes.Email).Value;
                IResult result = new ErrorResult();
                foreach (var fn in checkFunctions)
                {
                    var user = _userService.GetByMail(userEmail).Data;
                    result = fn.Invoke(user);
                    if (!result.Success)
                    {
                        return controller.BadRequest(result.Message);
                    }
                }
                return controller.Ok(result);
            }

            return controller.BadRequest();
        }
    }
}
