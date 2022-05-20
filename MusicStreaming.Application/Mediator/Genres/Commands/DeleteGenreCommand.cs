using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MusicStreaming.Application.Abstractions.DbContexts;
using MusicStreaming.Application.Abstractions.Response;
using MusicStreaming.Application.DTOs.Response;
using MusicStreaming.Application.Mediator.Common.Commands;

namespace MusicStreaming.Application.Mediator.Genres.Commands
{
    public record DeleteGenreCommand(string Name) : ICommand<Unit>;

    public class DeleteGenreCommandHandler : ICommandHandler<DeleteGenreCommand, Unit>
    {
        private readonly IMusicStreamingContext _context;
        private readonly ILogger _logger;

        public DeleteGenreCommandHandler(IMusicStreamingContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IApiResult<Unit>> Handle(DeleteGenreCommand request, CancellationToken cancellationToken)
        {
            var genre = await _context.Genre.SingleOrDefaultAsync(e => e.Name == request.Name);

            if (genre == null)
            {
                _logger.LogError("Genre with name {name} not found.", request.Name);
                return ApiResult<Unit>.CreateFailedResult($"Genre with name {request.Name} not found.");
            }

            _context.Genre.Remove(genre);
            await _context.SaveChangesAsync();

            return ApiResult<Unit>.CreateSuccessfulResult();
        }
    }
}
