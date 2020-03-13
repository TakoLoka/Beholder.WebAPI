using Core.Dtos.UserDtos;
using Core.Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Dtos.RoomDtos
{
    public class RoomRepresentationDto
    {
        public RoomRepresentationDto(Guid roomId, string roomName, User creator, string description)
        {
            RoomId = roomId;
            RoomName = roomName;
            Creator = new UserRepresentationDto(creator);
            Description = description;
        }

        public Guid RoomId { get; set; }
        public string RoomName { get; set; }
        public UserRepresentationDto Creator { get; set; }
        public string Description { get; set; }
    }
}
