using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace g2hotel_server.DTOs
{
    public class UserDTO
    {
        public string? Username { get; set; }
        public string? Token { get; set; }
        public string? PhotoUrl { get; set; }
    }
}