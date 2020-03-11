using Core.Constants;
using Core.Entities;
using Core.Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Dtos.UserDtos
{
    public class UserRepresentationDto: IDto
    {
        public UserRepresentationDto(User user)
        {
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            RegistryDate = user.RegistryDate;
            BirthDay = user.BirthDay;
            Characters = user.Characters;
            IsDM = user.OperationClaims.Find(claim => claim.Name == OperationClaimNames.DungeonMaster) != null;
            IsPlayer = user.OperationClaims.Find(claim => claim.Name == OperationClaimNames.Player) != null;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime RegistryDate { get; set; }
        public DateTime BirthDay { get; set; }
        public bool IsDM { get; set; }
        public bool IsPlayer { get; set; }
        public List<Character> Characters { get; set; }
    }
}
