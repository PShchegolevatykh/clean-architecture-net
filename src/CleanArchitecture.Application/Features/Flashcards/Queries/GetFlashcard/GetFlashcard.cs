using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Exceptions;
using CleanArchitecture.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Features.Flashcards.Queries.GetFlashcard;

public record GetFlashcardQuery(Guid Id) : IRequest<FlashcardDetailDto>;

public record FlashcardDetailDto
{
    public Guid Id { get; init; }
    public string Front { get; init; } = string.Empty;
    public string Back { get; init; } = string.Empty;
    public string? Description { get; init; }
    public int Difficulty { get; init; }
    public DateTime CreatedAt { get; init; }
}

public class GetFlashcardHandler(IApplicationDbContext context) : IRequestHandler<GetFlashcardQuery, FlashcardDetailDto>
{
    public async Task<FlashcardDetailDto> Handle(GetFlashcardQuery request, CancellationToken cancellationToken)
    {
        var entity = await context.Flashcards
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.Id == request.Id, cancellationToken);

        if (entity == null)
            throw new NotFoundException(nameof(Flashcard), request.Id);

        return new FlashcardDetailDto
        {
            Id = entity.Id,
            Front = entity.Front,
            Back = entity.Back,
            Description = entity.Description,
            Difficulty = entity.Difficulty,
            CreatedAt = entity.CreatedAt
        };
    }
}
