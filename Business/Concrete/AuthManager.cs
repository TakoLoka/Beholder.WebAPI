using Business.Abstract;
using Business.Constants;
using Core.Constants;
using Core.Dtos;
using Core.Entities.Models;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using Core.Utilities.Security.Jwt;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

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
                return new ErrorDataResult<AccessToken>(Messages.UserNotFound);
            }

            return new SuccessDataResult<AccessToken>(_tokenHelper.CreateToken(user), Messages.AuthMessages.AccessTokenCreated);
        }

        public IDataResult<User> Login(UserForLoginDto userForLogin)
        {
            if (!UserExists(userForLogin.Email).Success)
            {
                return new ErrorDataResult<User>(Messages.UserNotFound);
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

            if (!(userForRegisterDto.Email.Contains("@")))
            {
                return new ErrorDataResult<User>(Messages.AuthMessages.InvalidEmail);
            }

            if (userForRegisterDto.BirthDay.Year > DateTime.Now.AddYears(-CheckValues.AgeBarrier).Year)
            {
                return new ErrorDataResult<User>(Messages.AuthMessages.UserMustBeOlder);
            }

            if(userForRegisterDto.Password.Length < CheckValues.PasswordMinLength || userForRegisterDto.Password.Length > CheckValues.PasswordMaxLength)
            {
                return new ErrorDataResult<User>(Messages.AuthMessages.PasswordLengthError);
            }

            if (!userForRegisterDto.Password.Equals(userForRegisterDto.ConfirmPassword))
            {
                return new ErrorDataResult<User>(Messages.AuthMessages.PasswordsDoNotMatch);
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
            if (userToCheck.Data == null)
            {
                return new ErrorResult(Messages.UserNotFound);
            }

            return new SuccessResult(Messages.UserFound);
        }
    }
}
