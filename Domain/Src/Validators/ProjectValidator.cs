using FluentValidation;

namespace FocusMark.Domain.Validators
{
    public class ProjectValidator : AbstractValidator<Project>
    {
        public ProjectValidator()
        {
            base.RuleFor(project => project.Id)
                .NotEmpty();
            base.RuleFor(project => project.DueDate)
                .Must((project, dueDate) =>
                {
                    if (!project.StartDate.HasValue)
                    {
                        return true;
                    }

                    return dueDate > project.StartDate;
                });
            base.RuleFor(project => project.Owner)
                .NotEmpty();
            base.RuleFor(project => project.PercentageCompleted)
                .LessThanOrEqualTo<Project, byte>(100);
            base.RuleFor(project => project.Title)
                .NotEmpty();
        }
    }
}
