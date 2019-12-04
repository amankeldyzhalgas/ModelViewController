using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ModelViewController.Models
{
    public class AwardModel
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The maximum length must be at least 50 characters")]
        [RegularExpression(@"^[a-zA-Z0-9\s-]+$", ErrorMessage = "Invalid value")]
        public string Title { get; set; }

        [Required]
        [StringLength(150, ErrorMessage = "The maximum length must be at least 250 characters")]
        public string Description { get; set; }

        [Required]
        public IFormFile Image { get; set; }
    }
}
