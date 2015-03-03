using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain.Entities;

namespace AdminView.ViewModel
{
    public class AuthorizationViewModel
    {
        public User User { get; set; }

        public String Id { get; set; }

        [Required(ErrorMessage = "E-post måste anges.")]
        public String Email { get; set; }

        [Required(ErrorMessage = "Lösenord måste anges.")]
        public String Password { get; set; }

        public IEnumerable<Role> Roles { get; set; }

        public String Role { get; set; }

        public Role CurrentRole { get; set; }
    }
}