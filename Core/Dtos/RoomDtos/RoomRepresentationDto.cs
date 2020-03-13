using Core.Dtos.UserDtos;
using Core.Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Dtos.RoomDtos
{
    public class RoomRepresentationDto
    {
        public RoomRepresentationDto(Room room)
        {
            RoomId = room.RoomId;
            RoomName = room.RoomName;
            Creator = new UserRepresentationDto(room.Creator);
            Description = room.Description;
            Users = new List<UserRepresentationDto>();
            foreach (var user in room.Users)
            {
                Users.Add(new UserRepresentationDto(user));
            }
        }

        public Guid RoomId { get; set; }
        public string RoomName { get; set; }
        public UserRepresentationDto Creator { get; set; }
        public string Description { get; set; }
        public List<UserRepresentationDto> Users { get; set; }
    }
}
