using NileFood.Domain.Errors;
using NileFood.Infrastructure.Data;

namespace NileFood.Application.Contracts.MenuItems;

public class MenuItemRequestValidator : AbstractValidator<MenuItemRequest>
{
    public MenuItemRequestValidator(ApplicationDbContext context)
    {
        RuleFor(x => x.MenuItemSchedules)
            .Must(NoIdsProvided)
            .WithMessage(MenuItemErrors.ScheduleIdNotAllowed.Description);

        RuleFor(x => x.Name)
           .NotEmpty().WithMessage("Menu item name is required.")
           .MaximumLength(100).WithMessage("Menu item name cannot exceed 100 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.BasePrice)
            .GreaterThan(0).WithMessage("Base price must be greater than 0.");

        RuleFor(x => x.Image)
            .Must(ValidateFile)
            .WithMessage("Invalid file format or size. Allowed formats are .jpg, .jpeg, and .png. Maximum size is 5MB.");


        RuleFor(x => x.Calories)
            .GreaterThanOrEqualTo(0)
            .When(x => x.Calories.HasValue)
            .WithMessage("Calories must be 0 or greater.");

        RuleFor(x => x.PreparationTimeStart)
            .GreaterThanOrEqualTo(0)
            .When(x => x.PreparationTimeStart.HasValue)
            .WithMessage("Preparation time start must be 0 or greater.");

        RuleFor(x => x.PreparationTimeEnd)
            .GreaterThanOrEqualTo(0)
            .When(x => x.PreparationTimeEnd.HasValue)
            .WithMessage("Preparation time end must be 0 or greater.");

        RuleFor(x => x)
            .Must(x => x.PreparationTimeStart.HasValue && x.PreparationTimeEnd.HasValue && x.PreparationTimeStart.Value < x.PreparationTimeEnd.Value)
            .When(x => x.PreparationTimeStart.HasValue && x.PreparationTimeEnd.HasValue)
            .WithMessage("PreparationTimeStart must be less than PreparationTimeEnd.");



        // ✅ Validation for MenuItemOptions
        RuleForEach(x => x.MenuItemOptions)
            .ChildRules(option =>
            {
                option.RuleFor(o => o.Name)
                      .NotEmpty().WithMessage("Option name is required.")
                      .MaximumLength(100).WithMessage("Option name cannot exceed 100 characters.");

                option.RuleFor(o => o.Price)
                      .GreaterThanOrEqualTo(0).WithMessage("Option price must be 0 or greater.");

                option.RuleFor(o => o.TypeId)
                      .GreaterThan(0).WithMessage("Option TypeId must be greater than 0.");
            });

        RuleFor(x => x.MenuItemOptions)
            .Must(options =>
                options == null
                || options.GroupBy(o => new { o.Name, o.Price, o.TypeId })
                          .All(g => g.Count() == 1)
            )
            .WithMessage("Option (Name, Price, TypeId) must be unique.");





        RuleFor(x => x.MenuItemSchedules)
            .Must(NoOverlappingSchedules)
            .WithMessage("Schedules in the request cannot overlap on the same day and branch");

        RuleForEach(x => x.MenuItemSchedules)
            .SetValidator(new MenuItemScheduleRequestValidator());
    }



    private bool NoIdsProvided(List<MenuItemScheduleRequest> schedules)
    {
        return schedules.All(s => !s.Id.HasValue);
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
