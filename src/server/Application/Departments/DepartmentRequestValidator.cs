using Application.Common.Exceptions;

namespace Application.Departments;

public static class DepartmentRequestValidator
{
    public static void Validate(DepartmentRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.DepartmentName))
            throw new ValidationException("Department name is required.");
        if (request.DepartmentName.Length > 200)
            throw new ValidationException("Department name must not exceed 200 characters.");
        if (request.DepartmentAddress?.Length > 500)
            throw new ValidationException("Department address must not exceed 500 characters.");
    }
}
