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
        /// Gets or sets .
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets .
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets .
        /// </summary>
        public DateTime Birthdate { get; set; }

        /// <summary>
        /// Gets or sets .
        /// </summary>
        public string Photo { get; set; }

        /// <summary>
        /// Gets or sets .
        /// </summary>
        public virtual List<UserAward> UserAwards { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        public User()
        {
            this.UserAwards = new List<UserAward>();
        }
    }
}
