// <copyright file="Role.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ModelViewController.DAL.Entities
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Role class.
    /// </summary>
    public class Role
    {
        /// <summary>
        /// Gets or sets Id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets DisplayName.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets UserRoles.
        /// </summary>
        public virtual List<UserRole> UserRoles { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Role"/> class.
        /// Constructor.
        /// </summary>
        public Role()
        {
        }
    }
}
