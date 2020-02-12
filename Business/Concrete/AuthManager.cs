﻿using Business.Abstract;
using Business.Constants;
using Core.Entities;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using Core.Utilities.Security.Jwt;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class AuthManager : IAuthService
    {
        ITokenHelper _tokenHelper;
        IUserService _userService;

        public AuthManager(ITokenHelper tokenHelper, IUserService userService)
        {
            _tokenHelper = tokenHelper;
            _userService = userService;
        }

        public IDataResult<AccessToken> CreateAccessToken(User user)
        {
            if (!UserExists(user.Email).Success)
            {
                return new ErrorDataResult<AccessToken>(Messages.AuthMessages.UserNotFound);
            }

            return new SuccessDataResult<AccessToken>(_tokenHelper.CreateToken(user), Messages.AuthMessages.AccessTokenCreated);
        }

        public IDataResult<User> Login(UserForLoginDto userForLogin)
        {
            if (!UserExists(userForLogin.Email).Success)
            {
                return new ErrorDataResult<User>(Messages.AuthMessages.UserNotFound);
            }

            var userToCheck = _userService.GetByMail(userForLogin.Email);

            if (!HashingHelper.VerifyPasswordHash(userForLogin.Password, userToCheck.Data.PasswordHash, userToCheck.Data.PasswordSalt))
            {
                return new ErrorDataResult<User>(Messages.AuthMessages.PasswordError);
            }

            return new SuccessDataResult<User>(userToCheck.Data, Messages.AuthMessages.LoginSuccessful);
        }

        public IDataResult<User> Register(UserForRegisterDto userForRegisterDto)
        {

            if (UserExists(userForRegisterDto.Email).Success)
            {
                return new ErrorDataResult<User>(Messages.AuthMessages.UserAlreadyExists);
            }

            HashingHelper.CreatePasswordHash(userForRegisterDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

            User user = new User
            {
                Email = userForRegisterDto.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                FirstName = userForRegisterDto.FirstName,
                LastName = userForRegisterDto.LastName,
                RegistryDate = DateTime.Now,
                BirthDay = userForRegisterDto.BirthDay
            };

            _userService.Create(user);

            return new SuccessDataResult<User>(user, Messages.AuthMessages.RegisterSuccessful);
        }

        public IResult UserExists(string email)
        {
            var userToCheck = _userService.GetByMail(email);
            if (userToCheck == null)
            {
                return new ErrorResult(Messages.AuthMessages.UserNotFound);
            }

            return new SuccessResult(Messages.AuthMessages.UserFound);
        }
    }
}
