using System;
using System.Collections.Generic;

namespace Domain.Entities
{

    /// <summary>
    ///     Permissions for the user
    /// </summary>
    enum Permission {
        // Coupon permissions
        CanChangeCoupons,
        CanAddCoupons,
        CanDeleteCoupons,

        // User permissions
        CanChangeUsers,
        CanAddUsers,
        CanDeleteUsers,
        CanListUsers,

        // Rules permissions
        CanOverrideRules,
        CanChangeRules
    }

    /// <summary>
    ///     A given role for the user
    /// </summary>
    class Role
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
