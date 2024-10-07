namespace SnapTalk.Common.DTO;

public record Response<TResponse>(Error Error, TResponse? Data = null) 
    where TResponse : class
{
    public bool IsSuccess => Error == Error.None;
    
    public static implicit operator Response<TResponse>(TResponse data) 
        => new(Error.None, data);
    
    public static implicit operator Response<TResponse>(Error error) 
        => new(error);
}