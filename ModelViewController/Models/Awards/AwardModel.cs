// <copyright file="AwardModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ModelViewController.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// AwardViewModel.
    /// </summary>
    public class AwardModel
    {
        /// <summary>
        /// Gets or sets Id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets Title.
        /// </summary>
        [Required]
        [StringLength(50, ErrorMessage = "The maximum length must be at least 50 characters")]
        [RegularExpression(@"^[a-zA-Z0-9\s-]+$", ErrorMessage = "Invalid value")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets Description.
        /// </summary>
        [StringLength(150, ErrorMessage = "The maximum length must be at least 250 characters")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets Image.
        /// </summary>
        [Required]
        public IFormFile Image { get; set; }
    }
}
