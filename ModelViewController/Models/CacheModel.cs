// <copyright file="CacheModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ModelViewController.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Action enum.
    /// </summary>
    public enum Action
    {
        /// <summary>
        /// Add action.
        /// </summary>
        Add,

        /// <summary>
        /// Update action.
        /// </summary>
        Update,

        /// <summary>
        /// Remove action.
        /// </summary>
        Remove,
    }

    /// <summary>
    /// CacheModel class.
    /// </summary>
    public class CacheModel
    {
        /// <summary>
        /// Gets or sets AddedUsers.
        /// </summary>
        public List<UserModel> AddedUsers { get; set; }

        /// <summary>
        /// Gets or sets UpdatedUsers.
        /// </summary>
        public List<UserModel> UpdatedUsers { get; set; }

        /// <summary>
        /// Gets or sets DeletedUsers.
        /// </summary>
        public List<UserModel> DeletedUsers { get; set; }

        /// <summary>
        /// Gets or sets AddedAwards.
        /// </summary>
        public List<AwardModel> AddedAwards { get; set; }

        /// <summary>
        /// Gets or sets UpdatedAwards.
        /// </summary>
        public List<AwardModel> UpdatedAwards { get; set; }

        /// <summary>
        /// Gets or sets DeletedAwards.
        /// </summary>
        public List<AwardModel> DeletedAwards { get; set; }
    }
}
