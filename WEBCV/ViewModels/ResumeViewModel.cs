using System;
using System.ComponentModel.DataAnnotations;

namespace WEBCV.ViewModels
{
    public class ResumeViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tiêu đề hồ sơ")]
        [Display(Name = "Tiêu đề")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập thông tin học vấn")]
        [Display(Name = "Học vấn")]
        public string Education { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập kinh nghiệm làm việc")]
        [Display(Name = "Kinh nghiệm")]
        public string Experience { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập kỹ năng")]
        [Display(Name = "Kỹ năng")]
        public string Skills { get; set; }
    }
}