// <copyright file="RegisterModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ModelViewController.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// RegisterModel class.
    /// </summary>
    public class RegisterModel
    {
        /// <summary>
        /// Gets or sets Login.
        /// </summary>
        [Required(ErrorMessage = "Login is requiered")]
        public string Login { get; set; }

        /// <summary>
        /// Gets or sets Password.
        /// </summary>
        [Required(ErrorMessage = "Password is requiered")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets ConfirmPassword.
        /// </summary>
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords don't match")]
        public string ConfirmPassword { get; set; }

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
    }
}
