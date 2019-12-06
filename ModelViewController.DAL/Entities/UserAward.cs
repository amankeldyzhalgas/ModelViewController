// <copyright file="UserAward.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ModelViewController.DAL.Entities
{
    using System;

    /// <summary>
    /// UserAward class.
    /// </summary>
    public class UserAward
    {
        /// <summary>
        /// Gets or sets .
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets .
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Gets or sets .
        /// </summary>
        public Guid AwardId { get; set; }

        /// <summary>
        /// Gets or sets .
        /// </summary>
        public virtual Award Award { get; set; }
    }
}
