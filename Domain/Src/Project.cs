using System;

namespace FocusMark.Domain
{
    public class Project
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Owner { get; set; }

        public string Title { get; set; }
        public string Path { get; set; }
        public bool IsArchived { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }

        public Priority Priority { get; set; }
        public byte PercentageCompleted { get; set; }

        public override string ToString() => $"{this.Id}: {this.Title}";
    }
}
