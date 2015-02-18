using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Entities;

namespace AdminView.ViewModel
{
    public class UserViewModel
    {
        public IEnumerable<User> Users { get; set; }
        public int CurrentPage { get; set; }

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