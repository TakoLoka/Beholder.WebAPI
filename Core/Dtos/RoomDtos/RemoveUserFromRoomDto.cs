using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Dtos.RoomDtos
{
    public class RemoveUserFromRoomDto: IDto
    {
        public string UserEmail { get; set; }
        public string RoomId { get; set; }
    }
}
