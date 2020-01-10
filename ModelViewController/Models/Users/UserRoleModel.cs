// <copyright file="UserRoleModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ModelViewController.Models.Users
{
    using System;

    /// <summary>
    /// RoleModel class.
    /// </summary>
    public class UserRoleModel
    {
        /// <summary>
        /// Gets or sets RoleId.
        /// </summary>
        public Guid RoleId { get; set; }

        /// <summary>
        /// Gets or sets Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether .
        /// </summary>
        public bool Selected { get; set; }
    }
}
