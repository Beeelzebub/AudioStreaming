using MusicStreaming.Application.Abstractions.Response;

namespace MusicStreaming.Application.DTOs.Response
{
    public class ApiResult<T> : IApiResult<T>
    {
        public bool IsSuccess { get; set; }
        
        public T? Payload { get; set; } = default;

        public IEnumerable<string>? Errors { get; set; } = default;

        public static ApiResult<T> CreateSuccessfulResult() => 
            new ApiResult<T> { IsSuccess = true };

        public static ApiResult<T> CreateSuccessfulResult(T payload) =>
            new ApiResult<T> { IsSuccess = true, Payload = payload };

        public static ApiResult<T> CreateFailedResult() =>
            new ApiResult<T> { IsSuccess = false };

        public static ApiResult<T> CreateFailedResult(string error) =>
            new ApiResult<T> { IsSuccess = false, Errors = new List<string>() { error } };

        public static ApiResult<T> CreateFailedResult(IEnumerable<string> errors) =>
            new ApiResult<T> { IsSuccess = false, Errors = errors };


    }
}
