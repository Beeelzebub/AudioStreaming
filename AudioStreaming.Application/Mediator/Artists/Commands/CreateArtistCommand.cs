using AudioStreaming.Application.Abstractions.DbContexts;
using AudioStreaming.Application.Abstractions.Responses;
using AudioStreaming.Application.DTOs.Artists;
using AudioStreaming.Application.DTOs.Responses;
using AudioStreaming.Application.Mediator.Common.Commands;
using AudioStreaming.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AudioStreaming.Application.Mediator.Artists.Commands
{
    public record CreateArtistCommand(CreateArtistDto Payload, string UserId) : ICommand<Unit>;

    public class CreateArtistHandler : ICommandHandler<CreateArtistCommand, Unit>
    {
        private readonly IAudioStreamingContext _context;
        private readonly ILogger<CreateArtistHandler> _logger;

        public CreateArtistHandler(IAudioStreamingContext context, ILogger<CreateArtistHandler> logger)
        {
            _context = context;
            _logger = logger;
        }


        public async Task<IApiResult<Unit>> Handle(CreateArtistCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.User
                .Include(u => u.Artist)
                .SingleOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

            if (user == null)
            {
                var errorMessage = $"User with id {request.UserId} not found.";

                _logger.LogError(errorMessage);

                return ApiResult<Unit>.CreateFailedResult(errorMessage);
            }

            if (user.Artist != null)
            {
                return ApiResult<Unit>.CreateFailedResult("Artist already exists.");
            }

            var artistToAdd = new Artist()
            {
                IsConfirmed = false,
                Pseudonym = request.Payload.Pseudonym,
                Country = request.Payload.Country,
                Email = request.Payload.Email,
                Description = request.Payload.Description
            };

            user.Artist = artistToAdd;

            await _context.SaveChangesAsync(cancellationToken);

            return ApiResult<Unit>.CreateSuccessfulResult();
        }
    }
}
