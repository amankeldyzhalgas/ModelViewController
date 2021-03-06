﻿// <copyright file="UserAward.cs" company="PlaceholderCompany">
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
        /// Gets or sets UserId.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets User.
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Gets or sets AwardId.
        /// </summary>
        public Guid AwardId { get; set; }

        /// <summary>
        /// Gets or sets Award.
        /// </summary>
        public virtual Award Award { get; set; }
    }
}
