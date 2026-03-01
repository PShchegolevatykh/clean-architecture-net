using CleanArchitecture.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace CleanArchitecture.Application.Features.Flashcards.Queries.GetFlashcardsList;

public record GetFlashcardsListQuery : IRequest<List<FlashcardDto>>;

public record FlashcardDto
{
    public Guid Id { get; init; }
    public string Front { get; init; } = string.Empty;
    public string Back { get; init; } = string.Empty;
    public int Difficulty { get; init; }
}

public class GetFlashcardsListHandler(IApplicationDbContext context) : IRequestHandler<GetFlashcardsListQuery, List<FlashcardDto>>
{
    public async Task<List<FlashcardDto>> Handle(GetFlashcardsListQuery request, CancellationToken cancellationToken)
    {
        return await context.Flashcards
            .AsNoTracking()
            .Select(f => new FlashcardDto
            {
                Id = f.Id,
                Front = f.Front,
                Back = f.Back,
                Difficulty = f.Difficulty
            })
            .ToListAsync(cancellationToken);
    }
}
