namespace exam_system.Features.Shared;

public class EndpointResponse<T>
{
    public bool Success { get; set; }
    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public IDictionary<string, string[]>? Errors { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public EndpointResponse() { }

    public EndpointResponse(bool success, int statusCode, string message, T? data = default, IDictionary<string, string[]>? errors = null)
    {
        Success = success;
        StatusCode = statusCode;
        Message = message;
        Data = data;
        Errors = errors;
        Timestamp = DateTime.UtcNow;
    }

    public static EndpointResponse<T> FromResult(RequestResponse<T> result)
    {
        return new EndpointResponse<T>(
            result.Success,
            result.StatusCode,
            result.Message,
            result.Data,
            result.Errors
        );
    }
}

public class EndpointResponse
{
    public bool Success { get; set; }
    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
    public IDictionary<string, string[]>? Errors { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    
    public static EndpointResponse FromResult(RequestResponse result)
    {
        return new EndpointResponse
        {
            Success = result.Success,
            StatusCode = result.StatusCode,
            Message = result.Message,
            Errors = result.Errors,
            Timestamp = DateTime.UtcNow
        };
    }
}
