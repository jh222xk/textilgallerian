using System;
using System.Linq;
using Domain.Entities;
using Raven.Client;

namespace Domain.Repositories
{
    public class RoleRepository
    {
        private readonly IDocumentSession _session;

        public RoleRepository(IDocumentSession session)
        {
            _session = session;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IQueryable<Role> FindAllRoles()
        {
            return _session.Query<Role>();
        }

        /// <summary>
        ///     Finds a user by the email
        /// </summary>
        public Role FindByEmail(String email)
        {
            return _session.Query<Role>().FirstOrDefault(r => r.Users.Any(u => u.Email == email));
        }

        /// <summary>
        ///     Creates or updates the user
        /// </summary>
        public void Store(Role role)
        {
            _session.Store(role);
        }

        /// <summary>
        ///     Save changes specified with the Store method
        /// </summary>
        public void SaveChanges()
        {
            _session.SaveChanges();
        }

        public Role FindByName(string name)
        {
            return _session.Query<Role>().FirstOrDefault(r => r.Name == name);
        }
    }
}