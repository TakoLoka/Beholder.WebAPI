using Core.Dtos.RoomDtos;
using Core.Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Extensions
{
    public static class RoomExtensions
    {
        public static List<RoomRepresentationDto> GetRepresentationList(this ICollection<Room> rooms)
        {
            List<RoomRepresentationDto> allRooms = new List<RoomRepresentationDto>();
            foreach (var room in rooms)
            {
                allRooms.Add(new RoomRepresentationDto(room));
            }

            return allRooms;
        }
    }
}
