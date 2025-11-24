namespace NileFood.Application.Contracts.MenuItems;

public class UpdateMenuItemRequestValidator : AbstractValidator<UpdateMenuItemRequest>
{
    public UpdateMenuItemRequestValidator()
    {
        RuleFor(x => x.Name)
           .NotEmpty().WithMessage("Menu item name is required.")
           .MaximumLength(100).WithMessage("Menu item name cannot exceed 100 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.BasePrice)
            .GreaterThan(0).WithMessage("Base price must be greater than 0.");

        RuleFor(x => x.Image).Must(ValidateFile).WithMessage("Invalid file format or size. Allowed formats are .jpg, .jpeg, and .png. Maximum size is 5MB.");


        RuleFor(x => x.MenuItemSchedules)
            .Must(NoOverlappingSchedules)
            .WithMessage("Schedules in the request cannot overlap on the same day and branch");

        RuleForEach(x => x.MenuItemSchedules)
            .SetValidator(new MenuItemScheduleRequestValidator());
    }


    private bool NoOverlappingSchedules(List<MenuItemScheduleRequest> schedules)
    {
        var grouped = schedules.GroupBy(s => new { s.Day, s.BranchId });

        foreach (var group in grouped)
        {
            var ordered = group.OrderBy(s => s.StartTime).ToList();

            for (int i = 0; i < ordered.Count - 1; i++)
            {
                if (ordered[i].EndTime > ordered[i + 1].StartTime)
                    return false;
            }
        }

        return true;
    }


    private static bool ValidateFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return false;

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
        var extension = Path.GetExtension(file.FileName).ToLower();

        if (!allowedExtensions.Contains(extension))
            return false;


        const long maxFileSize = 5 * 1024 * 1024; // 5MB
        return file.Length <= maxFileSize;
    }
}
