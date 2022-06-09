namespace AudioStreaming.Application.Abstractions.Responses
{
    public interface IApiResult
    {
        bool IsSuccess { get; set; }

        IEnumerable<string>? Errors { get; set; }
    }

    public interface IApiResult<T> : IApiResult
    {
        T? Payload { get; set; } 
    }
}
