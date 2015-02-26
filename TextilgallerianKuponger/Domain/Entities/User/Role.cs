using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    /// <summary>
    ///     Permissions for the user
    /// </summary>
    public enum Permission
    {
        // Coupon permissions
        CanChangeCoupons,
        CanAddCoupons,
        CanDeleteCoupons,
        CanListCoupons,

        // User permissions
        CanChangeUsers,
        CanAddUsers,
        CanDeleteUsers,
        CanListUsers,

        // Roles permissions
        CanChangeRoles,
        CanAddRoles,
        CanDeleteRoles,
        CanListRoles
    }

    /// <summary>
    ///     A given role for the user
    /// </summary>
    public class Role
    {
        /// <summary>
        ///     List of users being in this perticular role
        /// </summary>
        public List<User> Users { get; set; }

        /// <summary>
        ///     The given screen name of the role
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        ///     List of permissions this role has access to
        /// </summary>
        public List<Permission> Permissions { get; set; }
    }
}