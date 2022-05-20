namespace MusicStreaming.Application.Abstractions.Response
{
    public interface IApiResult
    {
        bool IsSuccess { get; set; }

        IEnumerable<string>? Errors { get; set; }
    }

    public interface IApiResult<T>
    {
        T? Payload { get; set; } 
    }
}
