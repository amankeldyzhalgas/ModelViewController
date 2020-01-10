// <copyright file="ChangeUserAwardModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ModelViewController.Models
{
    using System;
    using System.Collections.Generic;
    using ModelViewController.DAL.Entities;
    using ModelViewController.Models.Users;

    /// <summary>
    /// AddAwardModel class.
    /// </summary>
    public class ChangeUserAwardModel
    {
        /// <summary>
        /// Gets or sets UserId.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets UserRoles.
        /// </summary>
        public List<UserAwardModel> UserAwards { get; set; }
    }
}
