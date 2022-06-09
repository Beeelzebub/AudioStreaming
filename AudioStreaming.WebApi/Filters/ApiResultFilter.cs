using AudioStreaming.Application.Abstractions.Responses;
using AudioStreaming.Application.DTOs.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace AudioStreaming.WebApi.Filters
{
    public class ApiResultFilter : Attribute, IAsyncResultFilter
    {
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (context.Result is ObjectResult result)
            {
                var value = result.Value;

                if (value != null && value is IApiResult apiResult)
                {
                    var apiResultType = apiResult.GetType();

                    if (apiResultType.IsGenericType)
                    {
                        var payload = apiResultType?.GetProperty("Payload")?.GetValue(apiResult, null);
                        var payloadType = payload?.GetType();

                        if (payload != null && payload.GetType().IsGenericType)
                        {
                            var pagedListType = typeof(PagedList<>);
                            var payloadGenericType = payloadType?.GetGenericArguments()[0];
                            var genericPagedListType = pagedListType.MakeGenericType(payloadGenericType);

                            if (payloadType == genericPagedListType)
                            {
                                var navigationMetadata = payloadType?.GetProperty("PaginationMetadata")?.GetValue(payload, null);

                                context.HttpContext.Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(navigationMetadata));
                            }
                        }
                    }

                    context.HttpContext.Response.StatusCode = apiResult.IsSuccess ? 200 : 400;
                }
            }

            await next();
        }
    }
}
