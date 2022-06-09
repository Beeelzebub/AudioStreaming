using AudioStreaming.Application.Abstractions.DbContexts;
using AudioStreaming.Application.DTOs.Releases;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace AudioStreaming.WebApi.Filters
{
    public class CheckAccessToReleaseFilter : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var dbContext = context.HttpContext.RequestServices.GetService<IAudioStreamingContext>();

            var claim = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

            if (context.ActionArguments["payload"] is ReleaseBaseDto payload && int.TryParse(claim, out var userId))
            {
                var hasPermission = await dbContext.ReleaseParticipant
                    .AnyAsync(p => p.ReleaseId == payload.ReleaseId && p.ArtistId == userId);
                
                if (!hasPermission)
                {
                    context.Result = new ObjectResult("Forbidden") { StatusCode = 403 };
                }
            }
            else
            {
                context.Result = new BadRequestResult();
            }

            await next();
        }
    }
}
