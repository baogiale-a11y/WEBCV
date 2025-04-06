using System;
using System.ComponentModel.DataAnnotations;

namespace WEBCV.ViewModels
{
    public class JobViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tiêu đề công việc")]
        [Display(Name = "Tiêu đề")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mô tả công việc")]
        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập yêu cầu công việc")]
        [Display(Name = "Yêu cầu")]
        public string Requirements { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa điểm làm việc")]
        [Display(Name = "Địa điểm")]
        public string Location { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mức lương")]
        [Display(Name = "Mức lương")]
        public string Salary { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên công ty")]
        [Display(Name = "Tên công ty")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn loại công việc")]
        [Display(Name = "Loại công việc")]
        public string JobType { get; set; } // Full-time, Part-time, etc.

        [Required(ErrorMessage = "Vui lòng nhập hạn nộp hồ sơ")]
        [Display(Name = "Hạn nộp hồ sơ")]
        [DataType(DataType.Date)]
        public DateTime DeadlineDate { get; set; }

        // Các thuộc tính bổ sung
        [Display(Name = "Tiêu đề công việc")]
        public string JobTitle { get; set; }

        [Display(Name = "Ngày nộp hồ sơ")]
        [DataType(DataType.Date)]
        public DateTime? AppliedDate { get; set; }

        [Display(Name = "Trạng thái")]
        public string Status { get; set; } // Applied, Interview, Hired, etc.
    }
}