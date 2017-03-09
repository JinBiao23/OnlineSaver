using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineSaver.Models
{
    public class GoalViewModel
    { 
        public int Id { get; set; }
        [Display(Name = "Title")]
        [Required]
        public string Title { get; set; }
        [Display(Name = "Target Amount")]
        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "The value must be greater than 1")]
        public decimal SaveGoal { get; set; }
        [Display(Name = "Amount Already Saved")]
        [Required]
        public decimal SavedAmount { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Display(Name = "Saved Progress")]
        public int progress { get; set; }
        public string UserId { get; set; }
        [Display(Name = "Created Date")]
        public DateTime CreateDate { get; set; }
    }
}