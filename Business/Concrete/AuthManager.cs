using Business.Abstract;
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
                return new ErrorDataResult<AccessToken>(Messages.UserNotFound);
            }

            return new SuccessDataResult<AccessToken>(_tokenHelper.CreateToken(user), Messages.AccessTokenCreated);
        }

        public IDataResult<User> Login(UserForLoginDto userForLogin)
        {
            if (!UserExists(userForLogin.Email).Success)
            {
                return new ErrorDataResult<User>(Messages.UserNotFound);
            }

            var userToCheck = _userService.GetByMail(userForLogin.Email);

            if (!HashingHelper.VerifyPasswordHash(userForLogin.Password, userToCheck.PasswordHash, userToCheck.PasswordSalt))
            {
                return new ErrorDataResult<User>(Messages.PasswordError);
            }

            return new SuccessDataResult<User>(userToCheck, Messages.LoginSuccessful);
        }

        public IDataResult<User> Register(UserForRegisterDto userForRegisterDto)
        {

            if (UserExists(userForRegisterDto.Email).Success)
            {
                return new ErrorDataResult<User>(Messages.UserAlreadyExists);
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

            return new SuccessDataResult<User>(user, Messages.RegisterSuccessful);
        }

        public IResult UserExists(string email)
        {
            var userToCheck = _userService.GetByMail(email);
            if (userToCheck == null)
            {
                return new ErrorResult(Messages.UserNotFound);
            }

            return new SuccessResult(Messages.UserFound);
        }
    }
}
