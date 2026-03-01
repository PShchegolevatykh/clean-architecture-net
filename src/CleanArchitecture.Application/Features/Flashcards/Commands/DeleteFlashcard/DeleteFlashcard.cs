using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Exceptions;
using CleanArchitecture.Domain.Entities;
using MediatR;

namespace CleanArchitecture.Application.Features.Flashcards.Commands.DeleteFlashcard;

public record DeleteFlashcardCommand(Guid Id) : IRequest;

public class DeleteFlashcardHandler(IApplicationDbContext context) : IRequestHandler<DeleteFlashcardCommand>
{
    public async Task Handle(DeleteFlashcardCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Flashcards
            .FindAsync([request.Id], cancellationToken);

        if (entity == null)
            throw new NotFoundException(nameof(Flashcard), request.Id);

        context.Flashcards.Remove(entity);
        await context.SaveChangesAsync(cancellationToken);
    }
}
