// <copyright file="Role.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ModelViewController.DAL.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Role class.
    /// </summary>
    public class Role
    {
        /// <summary>
        /// Gets or sets .
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets .
        /// </summary>
        public string RoleName { get; set; }
    }
}
