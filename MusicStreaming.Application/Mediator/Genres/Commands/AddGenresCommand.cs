using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MusicStreaming.Application.Abstractions.DbContexts;
using MusicStreaming.Application.Abstractions.Response;
using MusicStreaming.Application.DTOs.Genres;
using MusicStreaming.Application.DTOs.Response;
using MusicStreaming.Application.Mediator.Common.Commands;
using MusicStreaming.Domain.Entities;

namespace MusicStreaming.Application.Mediator.Genres.Commands
{
    public record AddGenresCommand(List<GenreDto> Genres) : ICommand<IEnumerable<string>>;

    public class AddGenresCommandHandler : ICommandHandler<AddGenresCommand, IEnumerable<string>>
    {
        private readonly IMusicStreamingContext _context;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public AddGenresCommandHandler(IMusicStreamingContext context, ILogger logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IApiResult<IEnumerable<string>>> Handle(AddGenresCommand request, CancellationToken cancellationToken)
        {
            var names = request.Genres.Select(g => g.Name);

            var existingGenres = await _context.Genre
                .Where(g => names.Contains(g.Name))
                .Select(g => g.Name)
                .ToListAsync();

            if (existingGenres.Count != 0)
            {
                _logger.LogError($"Genre{(existingGenres.Count == 1 ? "" : "s")} {string.Join(",", existingGenres)} already exist.");
            }

            var genresToAdd = _mapper.Map<List<Genre>>(request.Genres.Where(g => !existingGenres.Contains(g.Name)));

            await _context.Genre.AddRangeAsync(genresToAdd);
            await _context.SaveChangesAsync(cancellationToken);

            return ApiResult<IEnumerable<string>>.CreateSuccessfulResult(genresToAdd.Select(g => g.Name).ToList());
        }
    }
}
