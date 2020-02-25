using Core.Entities;
using Core.Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Dtos
{
    public class UserRepresentationDto: IDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime RegistryDate { get; set; }
        public DateTime BirthDay { get; set; }
        public List<Character> Characters { get; set; }
    }
}
