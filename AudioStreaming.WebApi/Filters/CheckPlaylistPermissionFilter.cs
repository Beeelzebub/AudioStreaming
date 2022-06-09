﻿using AudioStreaming.Application.Abstractions.DbContexts;
using AudioStreaming.Application.DTOs.Playlists;
using AudioStreaming.Domain.Enums;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace AudioStreaming.WebApi.Filters
{
    public class CheckPlaylistPermissionFilter : Attribute, IAsyncActionFilter
    {
        public PermissionType PermissionType { get; set; }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var dbContext = context.HttpContext.RequestServices.GetService<IAudioStreamingContext>();

            var claim = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

            if (context.ActionArguments["payload"] is PlaylistBaseDto payload && int.TryParse(claim, out var userId))
            {
                var hasPermission = await dbContext.Permission
                    .AnyAsync(p => p.UserId == userId && p.PlaylistId == payload.PlaylistId && p.Type == PermissionType);

                await next();
            }
            else
            {

            }
        }
    }
}
