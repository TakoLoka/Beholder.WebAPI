using Business.Abstract;
using Business.Constants;
using Core.Abstract;
using Core.Entities;
using DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Concrete
{
    public class UserManager : IUserService
    {
        IUserDal _userDal;
        IOperationClaimDal _operationClaimDal;
        public UserManager(IUserDal userDal, IOperationClaimDal operationClaimDal)
        {
            _userDal = userDal;
            _operationClaimDal = operationClaimDal;
        }

        public void BecomeDungeonMasterPremium(User user)
        {
            var premiumUser = _userDal.GetOne(usr => user.Id == user.Id);
            premiumUser.OperationClaims.Add(_operationClaimDal.GetOperationClaimByName(OperationClaimNames.DungeonMaster));
            _userDal.Update(user.Id.ToString(), premiumUser);
        }

        public void BecomePlayerPremium(User user)
        {
            var premiumUser = _userDal.GetOne(usr => user.Id == user.Id);
            premiumUser.OperationClaims.Add(_operationClaimDal.GetOperationClaimByName(OperationClaimNames.Player));
            _userDal.Update(user.Id.ToString(), premiumUser);
        }

        public void Create(User user)
        {
            _userDal.Create(user);
        }

        public User GetByMail(string email)
        {
            return _userDal.GetOne(user => user.Email == email);
        }

        public List<OperationClaim> GetClaims(User user)
        {
            return _userDal.GetOne(usr => usr.Id == user.Id).OperationClaims.ToList();
        }
    }
}
