using System.Collections.Generic;
using WEBCV.Models;
using WEBCV.Models;

namespace WEBCV.ViewModels
{
    public class JobDetailsViewModel
    {
        public Job Job { get; set; }
        public bool HasApplied { get; set; }
        public List<Resume> UserResumes { get; set; }
    }
}