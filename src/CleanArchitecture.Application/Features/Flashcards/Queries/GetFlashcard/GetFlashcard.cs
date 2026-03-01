using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Exceptions;
using CleanArchitecture.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Features.Flashcards.Queries.GetFlashcard;

public record GetFlashcardQuery(Guid Id) : IRequest<FlashcardDetailDto>;

public record FlashcardDetailDto(Guid Id, string Front, string Back, string? Description, int Difficulty, DateTime CreatedAt);

public class GetFlashcardHandler(IApplicationDbContext context) : IRequestHandler<GetFlashcardQuery, FlashcardDetailDto>
{
    public async Task<FlashcardDetailDto> Handle(GetFlashcardQuery request, CancellationToken cancellationToken)
    {
        var entity = await context.Flashcards
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.Id == request.Id, cancellationToken);

        if (entity == null)
            throw new NotFoundException(nameof(Flashcard), request.Id);

        return new FlashcardDetailDto(entity.Id, entity.Front, entity.Back, entity.Description, entity.Difficulty, entity.CreatedAt);
    }
}
