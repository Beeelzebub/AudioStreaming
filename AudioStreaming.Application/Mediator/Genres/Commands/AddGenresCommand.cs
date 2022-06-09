using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AudioStreaming.Application.Abstractions.DbContexts;
using AudioStreaming.Application.Abstractions.Responses;
using AudioStreaming.Application.DTOs.Genres;
using AudioStreaming.Application.DTOs.Responses;
using AudioStreaming.Application.Mediator.Common.Commands;
using AudioStreaming.Domain.Entities;

namespace AudioStreaming.Application.Mediator.Genres.Commands
{
    public record AddGenresCommand(ICollection<GenreDto> Genres) : ICommand<ICollection<string>>;

    public class AddGenresCommandHandler : ICommandHandler<AddGenresCommand, ICollection<string>>
    {
        private readonly IAudioStreamingContext _context;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public AddGenresCommandHandler(IAudioStreamingContext context, ILogger logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IApiResult<ICollection<string>>> Handle(AddGenresCommand request, CancellationToken cancellationToken)
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

            return ApiResult<ICollection<string>>.CreateSuccessfulResult(genresToAdd.Select(g => g.Name).ToList());
        }
    }
}
