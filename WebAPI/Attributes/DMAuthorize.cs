using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Attributes
{
    public class DMAuthorize: AuthorizeAttribute
    {
        public DMAuthorize(): base()
        {
            Roles = Core.Constants.OperationClaimNames.DungeonMaster;
        }
    }
}
