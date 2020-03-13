using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Dtos.RoomDtos
{
    public class CreateRoomDto
    {
        public string UserEmail { get; set; }
        public string RoomName { get; set; }
        public string Description { get; set; }
    }
}
