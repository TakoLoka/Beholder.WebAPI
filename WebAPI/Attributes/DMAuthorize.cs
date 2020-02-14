using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class DMAuthorize: AuthorizeAttribute
    {
        public DMAuthorize() : base()
        {
            Roles = Core.Constants.OperationClaimNames.DungeonMaster;
        }
    }
}
