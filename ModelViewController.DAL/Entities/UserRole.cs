// <copyright file="UserRole.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ModelViewController.DAL.Entities
{
    using System;

    /// <summary>
    /// UserRole class.
    /// </summary>
    public class UserRole
    {
        /// <summary>
        /// Gets or sets UserId.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets User.
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Gets or sets RoleId.
        /// </summary>
        public Guid RoleId { get; set; }

        /// <summary>
        /// Gets or sets Role.
        /// </summary>
        public virtual Role Role { get; set; }
    }
}
