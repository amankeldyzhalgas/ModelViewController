// <copyright file="User.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ModelViewController.DAL.Entities
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// User class.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets or sets Id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets UserName.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets Password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Birthdate.
        /// </summary>
        public DateTime Birthdate { get; set; }

        /// <summary>
        /// Gets or sets Photo.
        /// </summary>
        public string Photo { get; set; }

        /// <summary>
        /// Gets or sets .
        /// </summary>
        public virtual List<UserAward> UserAwards { get; set; }

        /// <summary>
        /// Gets or sets UserRoles.
        /// </summary>
        public virtual List<UserRole> UserRoles { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        public User()
        {
            this.UserAwards = new List<UserAward>();
        }
    }
}
