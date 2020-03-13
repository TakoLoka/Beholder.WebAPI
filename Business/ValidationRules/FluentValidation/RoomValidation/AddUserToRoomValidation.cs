using Core.Dtos.RoomDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.ValidationRules.FluentValidation.RoomValidation
{
    public class AddUserToRoomValidation: AbstractValidator<AddUserToRoomDto>
    {
        public AddUserToRoomValidation()
        {
            RuleFor(dto => dto.UserEmail).NotEmpty().EmailAddress();
            RuleFor(dto => dto.RoomId).NotEmpty();   
        }
    }
}
