using AudioStreaming.Application.Abstractions.Responses;

namespace AudioStreaming.Application.DTOs.Responses
{
    public class ApiResult : IApiResult
    {
        public bool IsSuccess { get; set; }

        public IEnumerable<string>? Errors { get; set; } = default;

        public static ApiResult CreateSuccessfulResult() =>
            new ApiResult { IsSuccess = true };

        public static ApiResult CreateFailedResult(string error) =>
            new ApiResult { IsSuccess = false, Errors = new List<string>() { error } };

        public static ApiResult CreateFailedResult(IEnumerable<string> errors) =>
            new ApiResult { IsSuccess = false, Errors = errors };
    }

    public class ApiResult<T> : ApiResult, IApiResult<T>
    {
        public T? Payload { get; set; } = default;

        public new static ApiResult<T> CreateSuccessfulResult() => 
            new ApiResult<T> { IsSuccess = true };

        public static ApiResult<T> CreateSuccessfulResult(T payload) =>
            new ApiResult<T> { IsSuccess = true, Payload = payload };

        public static ApiResult<T> CreateFailedResult() =>
            new ApiResult<T> { IsSuccess = false };

        public new static ApiResult<T> CreateFailedResult(string error) =>
            new ApiResult<T> { IsSuccess = false, Errors = new List<string>() { error } };

        public new static ApiResult<T> CreateFailedResult(IEnumerable<string> errors) =>
            new ApiResult<T> { IsSuccess = false, Errors = errors };


    }
}
