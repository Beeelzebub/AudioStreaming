using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AudioStreaming.Application.Abstractions.DbContexts;
using AudioStreaming.Application.Abstractions.Responses;
using AudioStreaming.Application.DTOs.Responses;
using AudioStreaming.Application.Mediator.Common.Commands;
using AudioStreaming.Application.DTOs.Genres;

namespace AudioStreaming.Application.Mediator.Genres.Commands
{
    public record EditGenreCommand(GenreDto Genre) : ICommand<Unit>;

    public class EditGenreCommandHandler : ICommandHandler<EditGenreCommand, Unit>
    {
        private readonly IAudioStreamingContext _context;
        private readonly ILogger _logger;

        public EditGenreCommandHandler(IAudioStreamingContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IApiResult<Unit>> Handle(EditGenreCommand request, CancellationToken cancellationToken)
        {
            var genre = await _context.Genre.SingleOrDefaultAsync(e => e.Name == request.Genre.Name);

            if (genre == null)
            {
                _logger.LogError("Genre with name {name} not found.", request.Genre.Name);
                return ApiResult<Unit>.CreateFailedResult($"Genre with name {request.Genre.Name} not found.");
            }

            genre.Description = request.Genre.Description;
            await _context.SaveChangesAsync();

            return ApiResult<Unit>.CreateSuccessfulResult();
        }
    }
}
