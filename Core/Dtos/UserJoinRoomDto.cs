using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Dtos
{
    public class UserJoinRoomDto: IDto
    {
        public string RoomName { get; set; }
    }
}
