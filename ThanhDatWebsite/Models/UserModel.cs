using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThanhDatWebsite.Models
{
    public class UserModel
    {
        public string ID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }
}