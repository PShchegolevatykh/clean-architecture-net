using CleanArchitecture.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace CleanArchitecture.Application.Features.Flashcards.Queries.GetFlashcardsList;

public record GetFlashcardsListQuery : IRequest<List<FlashcardDto>>;

public record FlashcardDto(Guid Id, string Front, string Back, int Difficulty);

public class GetFlashcardsListHandler(IApplicationDbContext context) : IRequestHandler<GetFlashcardsListQuery, List<FlashcardDto>>
{
    public async Task<List<FlashcardDto>> Handle(GetFlashcardsListQuery request, CancellationToken cancellationToken)
    {
        return await context.Flashcards
            .AsNoTracking()
            .Select(f => new FlashcardDto(f.Id, f.Front, f.Back, f.Difficulty))
            .ToListAsync(cancellationToken);
    }
}
