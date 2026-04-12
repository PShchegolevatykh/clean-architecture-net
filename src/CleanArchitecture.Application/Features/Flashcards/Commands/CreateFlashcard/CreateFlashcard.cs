using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using FluentValidation;
using MediatR;

namespace CleanArchitecture.Application.Features.Flashcards.Commands.CreateFlashcard;

public record CreateFlashcardCommand(string Front, string Back, string? Description, int Difficulty) : IRequest<Guid>;

public class CreateFlashcardValidator : AbstractValidator<CreateFlashcardCommand>
{
    public CreateFlashcardValidator()
    {
        RuleFor(v => v.Front).NotEmpty().MaximumLength(500);
        RuleFor(v => v.Back).NotEmpty().MaximumLength(500);
        RuleFor(v => v.Difficulty).InclusiveBetween(1, 5);
    }
}

public class CreateFlashcardHandler(IApplicationDbContext context) : IRequestHandler<CreateFlashcardCommand, Guid>
{
    public async Task<Guid> Handle(CreateFlashcardCommand request, CancellationToken cancellationToken)
    {
        var entity = Flashcard.Create(request.Front, request.Back, request.Description, request.Difficulty);

        context.Flashcards.Add(entity);
        await context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
