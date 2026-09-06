using System.ComponentModel.DataAnnotations;

namespace Application.Employees;

public record EmployeeDto(
    int EmployeeId, int DepartmentId, string DepartmentName,
    string FirstName, string LastName, string Gender,
    DateOnly DateOfBirth, DateOnly DateJoined, string? EmployeeAddress, string? Photo)
{
    public string FullName => $"{FirstName} {LastName}";
}

public class EmployeeRequest : IValidatableObject
{
    [Range(1, int.MaxValue, ErrorMessage = "Please select a department.")]
    public int DepartmentId { get; set; }

    [Required, MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required, RegularExpression("^(Male|Female|Other|Prefer not to say)$",
        ErrorMessage = "Please select a valid gender.")]
    public string Gender { get; set; } = string.Empty;

    public DateOnly DateOfBirth { get; set; }
    public DateOnly DateJoined { get; set; }

    [MaxLength(500)]
    public string? EmployeeAddress { get; set; }

    // Photo upload is optional and intentionally omitted in this demo.
    // Photo remains a nullable path on the entity and read DTO.

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        if (DateOfBirth == default || DateOfBirth > today)
            yield return new("Date of birth is required and must not be in the future.", [nameof(DateOfBirth)]);
        if (DateJoined == default || DateJoined > today)
            yield return new("Date joined is required and must not be in the future.", [nameof(DateJoined)]);
        if (DateJoined <= DateOfBirth)
            yield return new("Date joined must be later than date of birth.", [nameof(DateJoined)]);
    }
}
