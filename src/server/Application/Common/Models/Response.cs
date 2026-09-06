namespace Application.Common.Models;

public class Response<T>
{
    public bool Succeeded { get; set; }

    public string? Message { get; set; }

    public T? Data { get; set; }

    public static Response<T> Success(T data, string? message = null)
    {
        return new Response<T>
        {
            Succeeded = true,
            Message = message,
            Data = data
        };
    }

    public static Response<T> Failure(string message)
    {
        return new Response<T>
        {
            Succeeded = false,
            Message = message
        };
    }
}
