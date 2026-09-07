using Application.Common.Exceptions;

namespace Application.Employees.Commands;

public static class EmployeeRequestValidator
{
    private static readonly HashSet<string> AllowedGenders =
        ["Male", "Female", "Other", "Prefer not to say"];

    public static void Validate(EmployeeRequest request)
    {
        if (request.DepartmentId <= 0)
            throw new ValidationException("Please select a department.");

        ValidateRequiredText(request.FirstName, 100, "First name");
        ValidateRequiredText(request.LastName, 100, "Last name");

        if (string.IsNullOrWhiteSpace(request.Gender) || !AllowedGenders.Contains(request.Gender))
            throw new ValidationException("Please select a valid gender.");

        var today = DateOnly.FromDateTime(DateTime.Today);
        if (request.DateOfBirth == default || request.DateOfBirth > today)
            throw new ValidationException("Date of birth is required and must not be in the future.");
        if (request.DateJoined == default || request.DateJoined > today)
            throw new ValidationException("Date joined is required and must not be in the future.");
        if (request.DateJoined <= request.DateOfBirth)
            throw new ValidationException("Date joined must be later than date of birth.");
        if (request.EmployeeAddress?.Length > 500)
            throw new ValidationException("Employee address must not exceed 500 characters.");
    }

    private static void ValidateRequiredText(string? value, int maximumLength, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ValidationException($"{fieldName} is required.");
        if (value.Length > maximumLength)
            throw new ValidationException($"{fieldName} must not exceed {maximumLength} characters.");
    }
}
