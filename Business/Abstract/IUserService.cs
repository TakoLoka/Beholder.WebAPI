using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IUserService
    {
        List<OperationClaim> GetClaims(User user);
        void Create(User user);
        User GetByMail(string email);
        void BecomeDungeonMasterPremium(User user);
        void BecomePlayerPremium(User user);
    }
}
