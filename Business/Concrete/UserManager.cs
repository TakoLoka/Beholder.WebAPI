using Business.Abstract;
using Business.Constants;
using Core.Abstract;
using Core.Entities;
using Core.Entities.Models;
using Core.Utilities.Results;
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

        public IResult BecomeDungeonMasterPremium(User user)
        {
            var premiumUser = _userDal.GetOne(usr => user.Id == user.Id);
            if (premiumUser.OperationClaims == null)
            {
                premiumUser.OperationClaims = new List<OperationClaim>();
            }
            else if (premiumUser.OperationClaims.FirstOrDefault(x => x.Name == OperationClaimNames.DungeonMaster) != null)
            {
                return new ErrorResult(Messages.UserMessages.UserAlreadyPremiumDM(premiumUser));
            }

            premiumUser.OperationClaims.Add(_operationClaimDal.GetOperationClaimByName(OperationClaimNames.DungeonMaster));
            _userDal.Update(user.Id.ToString(), premiumUser);
            return new SuccessResult(Messages.UserMessages.UserBecamePremiumDM(premiumUser));
        }

        public IResult BecomePlayerPremium(User user)
        {
            var premiumUser = _userDal.GetOne(usr => user.Id == user.Id);
            if (premiumUser.OperationClaims == null)
            {
                premiumUser.OperationClaims = new List<OperationClaim>();
            }
            else if(premiumUser.OperationClaims.FirstOrDefault(x => x.Name == OperationClaimNames.Player) != null)
            {
                return new ErrorResult(Messages.UserMessages.UserAlreadyPremiumPlayer(premiumUser));
            }

            premiumUser.OperationClaims.Add(_operationClaimDal.GetOperationClaimByName(OperationClaimNames.Player));
            _userDal.Update(user.Id.ToString(), premiumUser);
            return new SuccessResult(Messages.UserMessages.UserBecamePremiumPlayer(premiumUser));
        }

        public IResult Create(User user)
        {
            _userDal.Create(user);
            return new SuccessResult(Messages.UserMessages.UserCreated(user));
        }

        public IDataResult<User> GetByMail(string email)
        {
            return new SuccessDataResult<User>(_userDal.GetOne(user => user.Email == email));
        }

        public IDataResult<List<OperationClaim>> GetClaims(User user)
        {
            return new SuccessDataResult<List<OperationClaim>>(_userDal.GetOne(usr => usr.Id == user.Id).OperationClaims.ToList());
        }
    }
}
