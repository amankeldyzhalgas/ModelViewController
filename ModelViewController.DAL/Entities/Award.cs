// <copyright file="Award.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ModelViewController.DAL.Entities
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Award class.
    /// </summary>
    public class Award
    {
        /// <summary>
        /// Gets or sets .
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets .
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets .
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets .
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// Gets or sets .
        /// </summary>
        public virtual List<UserAward> UserAwards { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Award"/> class.
        /// .
        /// </summary>
        public Award()
        {
            this.UserAwards = new List<UserAward>();
        }
    }
}
