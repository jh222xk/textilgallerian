using System;
using System.Collections.Generic;
using System.Linq;
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
        
        public int CurrentPage { get; set; }
        public string Name { get; set; }

        public int AmountOfPages()
        {
            var calculated = (Users.Count()/10.0);

            return (int) (Math.Ceiling(calculated));
        }

        public IEnumerable<User> FindUsersByPage(int page)
        {
            return Users.OrderBy(u => u.Email).Skip((page)*10).Take(10).ToList();
        }

    }
}