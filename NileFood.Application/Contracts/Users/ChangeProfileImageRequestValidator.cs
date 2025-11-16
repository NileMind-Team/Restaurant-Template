namespace NileFood.Application.Contracts.Users;

public class ChangeProfileImageRequestValidator : AbstractValidator<ChangeProfileImageRequest>
{
    public ChangeProfileImageRequestValidator()
    {
        RuleFor(x => x.Image).Must(ValidateFile).WithMessage("Invalid file format or size. Allowed formats are .jpg, .jpeg, and .png. Maximum size is 5MB.");
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
