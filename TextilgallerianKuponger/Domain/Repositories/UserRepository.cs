using System;
using System.Linq;
using Domain.Entities;
using Raven.Client;
using System.Collections.Generic;

namespace Domain.Repositories
{
    public class UserRepository
    {
        private readonly IDocumentSession _session;

        public UserRepository(IDocumentSession session)
        {
            _session = session;
        }

        /// <summary>
        ///     Finds a user by the email
        /// </summary>
        public User FindByEmail(String email)
        {
            return _session.Query<User>()
                .FirstOrDefault(user => user.Email == email);
        }

        public IEnumerable<User> FindAllUsers()
        {
            return _session.Query<User>().ToList();
        }

        public IEnumerable<User> FindUsersByPage(int page, int count)
        {
            return _session.Query<User>().OrderBy(u => u.Email).Skip((page) * count).Take(count).ToList(); 
        }

        public int AmountOfPages()
        {
            var amountOfPosts = FindAllUsers();
            double calculated = (amountOfPosts.Count() / 10.0);

            return (int)(Math.Ceiling(calculated));

        }

        /// <summary>
        ///     Creates or updates the user
        /// </summary>
        public void Store(User user)
        {
            _session.Store(user);
        }

        /// <summary>
        ///     Save changes specified with the Store method
        /// </summary>
        public void SaveChanges()
        {
            _session.SaveChanges();
        }
    }
}