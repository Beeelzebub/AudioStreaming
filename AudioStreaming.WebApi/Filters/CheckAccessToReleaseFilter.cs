using AudioStreaming.Application.Abstractions.DbContexts;
using AudioStreaming.Application.DTOs.Releases;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AudioStreaming.WebApi.Filters
{
    public class CheckAccessToReleaseFilter : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var dbContext = context.HttpContext.RequestServices.GetService<IAudioStreamingContext>();

            var userId = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (context.ActionArguments["payload"] is ReleaseBaseDto payload && !string.IsNullOrEmpty(userId))
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
