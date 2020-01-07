// <copyright file="AddAwardModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ModelViewController.Models
{
    using System;
    using System.Collections.Generic;
    using ModelViewController.DAL.Entities;

    /// <summary>
    /// AddAwardModel class.
    /// </summary>
    public class AddAwardModel
    {
        /// <summary>
        /// Gets or sets UserId.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets userAwards.
        /// </summary>
        public List<Award> UserAwards { get; set; }

        /// <summary>
        /// Gets or sets Awards.
        /// </summary>
        public List<Award> Awards { get; set; }
    }
}
