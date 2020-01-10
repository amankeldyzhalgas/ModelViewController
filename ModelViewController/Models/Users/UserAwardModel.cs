// <copyright file="UserAwardModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ModelViewController.Models.Users
{
    using System;

    /// <summary>
    /// UserAwardModel class.
    /// </summary>
    public class UserAwardModel
    {
        /// <summary>
        /// Gets or sets AwardId.
        /// </summary>
        public Guid AwardId { get; set; }

        /// <summary>
        /// Gets or sets Title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets Image.
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether .
        /// </summary>
        public bool Selected { get; set; }
    }
}
