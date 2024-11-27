using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Store_Management.DTO
{
    public class UserDTO
    {
        public string Id { get; set; } 

        public string BusinessName { get; set; }
        public string Ownername { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }


    }
}