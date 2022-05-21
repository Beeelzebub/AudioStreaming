using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MusicStreaming.Application.Abstractions.DbContexts;
using MusicStreaming.Application.Abstractions.Responses;
using MusicStreaming.Application.DTOs.Responses;
using MusicStreaming.Application.Mediator.Common.Commands;

namespace MusicStreaming.Application.Mediator.Genres.Commands
{
    public record EditGenreCommand(string Name, string Description) : ICommand<Unit>;

    public class EditGenreCommandHandler : ICommandHandler<EditGenreCommand, Unit>
    {
        private readonly IMusicStreamingContext _context;
        private readonly ILogger _logger;

        public EditGenreCommandHandler(IMusicStreamingContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IApiResult<Unit>> Handle(EditGenreCommand request, CancellationToken cancellationToken)
        {
            var genre = await _context.Genre.SingleOrDefaultAsync(e => e.Name == request.Name);

            if (genre == null)
            {
                _logger.LogError("Genre with name {name} not found.", request.Name);
                return ApiResult<Unit>.CreateFailedResult($"Genre with name {request.Name} not found.");
            }

            genre.Description = request.Description;
            await _context.SaveChangesAsync();

            return ApiResult<Unit>.CreateSuccessfulResult();
        }
    }
}
