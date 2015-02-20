using System;
using System.Linq;
using Domain.Entities;
using Raven.Client;

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

        /// <summary>
        ///     Finds all the users
        /// </summary>
        /// <returns></returns>
        public IQueryable<User> FindAllUsers()
        {
            return _session.Query<User>();
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