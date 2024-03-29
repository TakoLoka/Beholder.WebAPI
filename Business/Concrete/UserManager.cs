﻿using Business.Abstract;
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
            var premiumUser = _userDal.GetOne(usr => user.Id.Equals(usr.Id));
            if (premiumUser.OperationClaims == null)
            {
                user.OperationClaims = new List<OperationClaim>();
            }
            else if (premiumUser.OperationClaims.FirstOrDefault(x => x.Name.Equals(OperationClaimNames.DungeonMaster)) != null)
            {
                return new ErrorResult(Messages.UserMessages.UserAlreadyPremiumDM(premiumUser));
            }

            user.OperationClaims.Add(_operationClaimDal.GetOperationClaimByName(OperationClaimNames.DungeonMaster));
            _userDal.Update(user.Id.ToString(), user);
            return new SuccessResult(Messages.UserMessages.UserBecamePremiumDM(premiumUser));
        }

        public IResult BecomePlayerPremium(User user)
        {
            var premiumUser = _userDal.GetOne(usr => user.Id.Equals(usr.Id));
            if (premiumUser.OperationClaims == null)
            {
                premiumUser.OperationClaims = new List<OperationClaim>();
            }
            else if (premiumUser.OperationClaims.FirstOrDefault(x => x.Name.Equals(OperationClaimNames.Player)) != null)
            {
                return new ErrorResult(Messages.UserMessages.UserAlreadyPremiumPlayer(premiumUser));
            }

            user.OperationClaims.Add(_operationClaimDal.GetOperationClaimByName(OperationClaimNames.Player));
            _userDal.Update(user.Id.ToString(), user);
            return new SuccessResult(Messages.UserMessages.UserBecamePremiumPlayer(premiumUser));
        }

        public IResult Create(User user)
        {
            _userDal.Create(user);
            return new SuccessResult(Messages.UserMessages.UserCreated(user));
        }

        public IDataResult<User> GetByMail(string email)
        {
            return new SuccessDataResult<User>(_userDal.GetOne(user => user.Email.Equals(email)));
        }

        public IDataResult<List<OperationClaim>> GetClaims(User user)
        {
            return new SuccessDataResult<List<OperationClaim>>(_userDal.GetOne(usr => usr.Id.Equals(user.Id)).OperationClaims.ToList());
        }

        public IResult IsDungeonMasterPremium(User user)
        {
            if (user.OperationClaims.Find(x => x.Name.Equals("DM")) != null)
                return new SuccessResult(Messages.UserMessages.UserIsDungeonMaster);
            return new ErrorResult(Messages.UserMessages.UserIsNotDungeonMaster);
        }

        public IResult IsPlayerPremium(User user)
        {
            if (user.OperationClaims.Find(x => x.Name.Equals("Player")) != null)
                return new SuccessResult(Messages.UserMessages.UserIsPlayerPremium);
            return new ErrorResult(Messages.UserMessages.UserIsNotPlayerPremium);
        }

        public IResult RemoveDungeonMasterPremium(User user)
        {
            var premiumUser = _userDal.GetOne(usr => user.Id.Equals(usr.Id));
            if (premiumUser.OperationClaims == null)
            {
                return new ErrorResult(Messages.UserMessages.UserNotPremiumDM(premiumUser));
            }
            else if (premiumUser.OperationClaims.FirstOrDefault(x => x.Name.Equals(OperationClaimNames.DungeonMaster)) == null)
            {
                return new ErrorResult(Messages.UserMessages.UserNotPremiumDM(premiumUser));
            }
            var removeClaim = user.OperationClaims.Find(claim => claim.Id.Equals(_operationClaimDal.GetOperationClaimByName(OperationClaimNames.DungeonMaster).Id));
            user.OperationClaims.Remove(removeClaim);
            _userDal.Update(user.Id.ToString(), user);
            return new SuccessResult(Messages.UserMessages.UserRemovedPremiumDM(premiumUser));
        }

        public IResult RemovePlayerPremium(User user)
        {
            var premiumUser = _userDal.GetOne(usr => user.Id.Equals(usr.Id));
            if (premiumUser.OperationClaims == null)
            {
                return new ErrorResult(Messages.UserMessages.UserNotPremiumPlayer(premiumUser));
            }
            else if (premiumUser.OperationClaims.FirstOrDefault(x => x.Name.Equals(OperationClaimNames.Player)) == null)
            {
                return new ErrorResult(Messages.UserMessages.UserNotPremiumPlayer(premiumUser));
            }
            var removeClaim = user.OperationClaims.Find(claim => claim.Id.Equals(_operationClaimDal.GetOperationClaimByName(OperationClaimNames.Player).Id));
            user.OperationClaims.Remove(removeClaim);
            _userDal.Update(user.Id.ToString(), user);
            return new SuccessResult(Messages.UserMessages.UserRemovedPremiumPlayer(premiumUser));
        }

        public IResult UpdateUser(User user)
        {
            _userDal.Update(user.Id.ToString(), user);
            return new SuccessResult(Messages.UserMessages.UserUpdated);
        }
    }
}
