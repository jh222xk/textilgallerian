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
            // Coupons
            {Permission.CanAddCoupons, "Kan skapa rabatter"},
            {Permission.CanChangeCoupons, "Kan editera rabatter"},
            {Permission.CanDeleteCoupons, "Kan ta bort rabatter"},
            {Permission.CanListCoupons, "Kan lista rabatter"},

            // Users
            {Permission.CanAddUsers, "Kan skapa användare"},
            {Permission.CanChangeUsers, "Kan editera användare"},
            {Permission.CanDeleteUsers, "Kan ta bort användare"},
            {Permission.CanListUsers, "Kan lista användare"},

            // Roles
            {Permission.CanAddRoles, "Kan skapa roller"},
            {Permission.CanChangeRoles, "Kan editera roller"},
            {Permission.CanDeleteRoles, "Kan ta bort roller"},
            {Permission.CanListRoles, "Kan lista roller"},
        };

        public List<Permission> ChoosenPermissions { get; set; }

        public PostedPermissions PostedPermissions { get; set; }
    }

    public class PostedPermissions
    {
        public string[] PermissionIDs { get; set; }
    }
}