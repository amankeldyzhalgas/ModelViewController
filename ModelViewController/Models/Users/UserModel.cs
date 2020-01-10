// <copyright file="UserModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ModelViewController.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Http;
    using ModelViewController.DAL.Entities;

    /// <summary>
    /// UserViewModel.
    /// </summary>
    public class UserModel
    {
        /// <summary>
        /// Gets or sets Id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets Name.
        /// </summary>
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "The length of the string must be between 3 and 50 characters")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Birthdate.
        /// </summary>
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime Birthdate { get; set; }

        /// <summary>
        /// Gets or sets Photo.
        /// </summary>
        public IFormFile Photo { get; set; }

        /// <summary>
        /// Gets or sets PhotoSrc.
        /// </summary>
        public string PhotoSrc { get; set; }

        /// <summary>
        /// Gets or sets UserAwards.
        /// </summary>
        public ChangeUserAwardModel Awards { get; set; }
    }
}
