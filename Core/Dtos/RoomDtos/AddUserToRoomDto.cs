using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Dtos.RoomDtos
{
    public class AddUserToRoomDto: IDto
    {
        public string RoomId { get; set; }
        public string UserEmail { get; set; }
    }
}
