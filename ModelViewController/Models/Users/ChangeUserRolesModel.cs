// <copyright file="ChangeUserRolesModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ModelViewController.Models.Users
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ModelViewController.DAL.Entities;

    /// <summary>
    /// ChangeUserRolesModel class.
    /// </summary>
    public class ChangeUserRolesModel
    {
        /// <summary>
        /// Gets or sets UserId.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets UserName.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets UserRoles.
        /// </summary>
        public List<Role> UserRoles { get; set; }

        /// <summary>
        /// Gets or sets UserRoles.
        /// </summary>
        public List<Role> Roles { get; set; }
    }
}
