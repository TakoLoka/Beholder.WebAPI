using Core.Dtos.RoomDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.ValidationRules.FluentValidation.RoomValidation
{
    public class CreateRoomValidation : AbstractValidator<CreateRoomDto>
    {
        public CreateRoomValidation()
        {
            RuleFor(room => room.RoomName).NotEmpty();
        }
    }
}
