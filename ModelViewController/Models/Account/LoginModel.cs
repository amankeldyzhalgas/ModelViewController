// <copyright file="LoginModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ModelViewController.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// LoginModel class.
    /// </summary>
    public class LoginModel
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
    }
}
