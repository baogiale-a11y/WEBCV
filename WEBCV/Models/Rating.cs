using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBCV.Models;

namespace WEBCV.Models
{
    public class Rating
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Score { get; set; }

        [Required]
        public string Comment { get; set; }

        [Required]
        public DateTime RatedDate { get; set; }

        [Required]
        public string RatedUserId { get; set; }

        [ForeignKey("RatedUserId")]
        public virtual ApplicationUser RatedUser { get; set; }

        [Required]
        public string RatingUserId { get; set; }

        [ForeignKey("RatingUserId")]
        public virtual ApplicationUser RatingUser { get; set; }
    }
}