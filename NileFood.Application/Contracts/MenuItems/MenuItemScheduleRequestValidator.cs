using NileFood.Application.Contracts.MenuItems;

public class MenuItemScheduleRequestValidator : AbstractValidator<MenuItemScheduleRequest>
{
    private static readonly string[] ValidArabicDays = ["الأحد", "الاثنين", "الثلاثاء", "الأربعاء", "الخميس", "الجمعة", "السبت"];


    public MenuItemScheduleRequestValidator()
    {
        // EndTime > StartTime
        RuleFor(x => x.EndTime)
            .GreaterThan(x => x.StartTime)
            .WithMessage("EndTime must be greater than StartTime");

        // StartTime >= 00:00
        RuleFor(x => x.StartTime)
            .GreaterThanOrEqualTo(TimeSpan.Zero)
            .WithMessage("StartTime must be greater than or equal to 00:00");

        // EndTime <= 23:59:59
        RuleFor(x => x.EndTime)
            .LessThanOrEqualTo(new TimeSpan(23, 59, 59))
            .WithMessage("EndTime must be less than or equal to 23:59:59");

        RuleFor(x => x.Day)
            .NotEmpty().WithMessage("Day is required.")
            .Must(day => ValidArabicDays.Any(d => d.Equals(day, StringComparison.OrdinalIgnoreCase)))
            .WithMessage("Day must be a valid Arabic day of the week.");

        // BranchId > 0 if exists
        RuleFor(x => x.BranchId)
            .GreaterThan(0)
            .When(x => x.BranchId.HasValue)
            .WithMessage("BranchId must be greater than 0");

        // Notes length
        RuleFor(x => x.Notes)
            .MaximumLength(200)
            .WithMessage("Notes cannot exceed 200 characters");
    }
}
