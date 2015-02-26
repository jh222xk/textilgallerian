using System;
using System.Collections.Generic;
using Domain.Entities;

namespace AdminView.ViewModel
{
    public class AuthorizationViewModel
    {
        public User User { get; set; }
        public String Email { get; set; }
        public String Password { get; set; }

        public IEnumerable<Role> Roles { get; set; }

        public String Role { get; set; }
    }
}