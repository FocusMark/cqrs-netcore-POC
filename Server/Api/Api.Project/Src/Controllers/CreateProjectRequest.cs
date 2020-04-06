using FocusMark.Domain;
using System;

namespace FocusMark.Api.Controllers
{
    public class CreateProjectRequest
    {
        public string Title { get; set; }
        public string Path { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }

        public string Priority { get; set; }
        public byte PercentageCompleted { get; set; }
    }
}
