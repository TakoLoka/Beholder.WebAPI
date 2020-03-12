using Core.Constants;
using Core.Dtos.AuthDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.ValidationRules.FluentValidation.UserValidation
{
    public class UserForRegisterValidator: AbstractValidator<UserForRegisterDto>
    {
        public UserForRegisterValidator()
        {
            RuleFor(user => user.Email).NotEmpty().EmailAddress();
            RuleFor(user => user.FirstName).NotEmpty();
            RuleFor(user => user.LastName).NotEmpty();
            RuleFor(user => user.Password).NotEmpty().Length(CheckValues.PasswordMinLength, CheckValues.PasswordMaxLength);
            RuleFor(user => user.ConfirmPassword).Matches(usr => usr.Password);
            RuleFor(user => user.BirthDay.Year).LessThanOrEqualTo(DateTime.Now.Year - 18);
        }
    }
}
