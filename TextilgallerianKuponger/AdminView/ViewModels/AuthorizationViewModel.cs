using System;
using Domain.Entities;

namespace AdminView.ViewModel
{
    public class AuthorizationViewModel
    {
        public User User { get; set; }
        public String Email { get; set; }
        public String Password { get; set; }
    }
}