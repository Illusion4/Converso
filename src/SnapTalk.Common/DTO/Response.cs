namespace SnapTalk.Common.DTO;

public class Response<TResponse>
{
    public TResponse? Data { get; }
    public IReadOnlyList<Error>? Errors { get; }
    
    private Response(TResponse? data, IReadOnlyList<Error>? errors = null)
    {
        Data = data;
        Errors = errors;
    }
    
    public static implicit operator Response<TResponse>(TResponse data) => Success(data);
    
    public static implicit operator Response<TResponse>(List<Error> errors) => Error(errors);
    
    public static implicit operator Response<TResponse>(Error error) => Error(new List<Error> { error });

    public static Response<TResponse> Success() => Success(default!);
    
    public static Response<TResponse> Success(TResponse data) => new(data);
    
    public static Response<TResponse> Error(string code, string message) =>
        new(default, new List<Error> { new(code, message) });

    public static Response<TResponse> Error(List<Error> errors) => new(default, errors);
}