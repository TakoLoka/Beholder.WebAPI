using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Dtos.AuthDtos
{
    public class UserForLoginDto: IDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
