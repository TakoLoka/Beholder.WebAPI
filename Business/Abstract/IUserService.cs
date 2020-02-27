using Core.Entities.Models;
using Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IUserService
    {
        IDataResult<List<OperationClaim>> GetClaims(User user);
        IResult Create(User user);
        IDataResult<User> GetByMail(string email);
        IResult BecomeDungeonMasterPremium(User user);
        IResult BecomePlayerPremium(User user);
        IResult RemoveDungeonMasterPremium(User user);
        IResult RemovePlayerPremium(User user);
        IResult UpdateUser(User user);
    }
}
