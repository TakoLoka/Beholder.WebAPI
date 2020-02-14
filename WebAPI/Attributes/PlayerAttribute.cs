using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Attributes
{
    public class PlayerAttribute: AuthorizeAttribute
    {
        public PlayerAttribute(): base()
        {
            Roles = Core.Constants.OperationClaimNames.Player;
        }
    }
}
