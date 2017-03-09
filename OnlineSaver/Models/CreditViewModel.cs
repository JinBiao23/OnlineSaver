using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineSaver.Models
{
    public class CreditViewModel
    {
        public int Id { get; set; }
        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "The value must be greater than 1")]
        public decimal CreditAmount { get; set; }
        public string UserId { get; set; }
        public string Description { get; set; }
    }
}