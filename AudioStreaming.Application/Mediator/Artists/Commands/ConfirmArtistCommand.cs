using AudioStreaming.Application.Abstractions.DbContexts;
using AudioStreaming.Application.Abstractions.Responses;
using AudioStreaming.Application.Abstractions.Services.Users;
using AudioStreaming.Application.Mediator.Common.Commands;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AudioStreaming.Application.DTOs.Responses;

namespace AudioStreaming.Application.Mediator.Artists.Commands
{
    public record ConfirmArtistCommand(string ArtistId) : ICommand<Unit>;

    public class ConfirmArtistHandler: ICommandHandler<ConfirmArtistCommand, Unit>
    {
        private readonly IAudioStreamingContext _context;
        private readonly IUserService _userService;
        private readonly ILogger _logger;

        public ConfirmArtistHandler(IAudioStreamingContext context, ILogger logger, IUserService userService)
        {
            _context = context;
            _logger = logger;
            _userService = userService;
        }


        public async Task<IApiResult<Unit>> Handle(ConfirmArtistCommand request, CancellationToken cancellationToken)
        {
            var artist = await _context.Artist.SingleOrDefaultAsync(a => a.Id == request.ArtistId, cancellationToken);

            if (artist == null)
            {
                var errorMessage = $"Artist with id {request.ArtistId} not found.";

                _logger.LogError(errorMessage);

                return ApiResult<Unit>.CreateFailedResult(errorMessage);
            }

            artist.IsConfirmed = true;

            await _context.SaveChangesAsync(cancellationToken);

            await _userService.AddRoleToUserAsync(artist.Id, "Artist");

            return ApiResult<Unit>.CreateSuccessfulResult();
        }
    }
}
