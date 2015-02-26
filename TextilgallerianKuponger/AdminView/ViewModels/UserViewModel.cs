using System;
using System.Collections.Generic;
using Domain.Entities;

namespace AdminView.ViewModel
{
    public class UserViewModel
    {
        public IEnumerable<User> Users { get; set; }


        public static Dictionary<Permission, String> Permissions = new Dictionary<Permission, String>
        {
            {Permission.CanAddCoupons, "Kan skapa rabatter"},
            {Permission.CanAddUsers, "Kan skapa användare"},
            {Permission.CanChangeCoupons, "Kan editera rabatter"},
            {Permission.CanChangeRules, "Kan editera Regler"},
            {Permission.CanChangeUsers, "Kan editera användare"},
            {Permission.CanDeleteCoupons, "Kan ta bort rabatter"},
            {Permission.CanDeleteUsers, "Kan ta bort användare"},
            {Permission.CanListUsers, "Kan lista användare"},
            {Permission.CanOverrideRules, "Kan överse regler"}
        };
        public List<Permission> ChosenPermissions = new List<Permission>();
    }
}