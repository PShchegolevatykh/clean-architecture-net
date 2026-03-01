using CleanArchitecture.Application.Features.Flashcards.Commands.CreateFlashcard;
using CleanArchitecture.Application.Features.Flashcards.Commands.DeleteFlashcard;
using CleanArchitecture.Application.Features.Flashcards.Queries.GetFlashcard;
using CleanArchitecture.Application.Features.Flashcards.Queries.GetFlashcardsList;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FlashcardsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<FlashcardDto>>> GetFlashcards(CancellationToken cancellationToken)
    {
        return await mediator.Send(new GetFlashcardsListQuery(), cancellationToken);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<FlashcardDetailDto>> GetFlashcard(Guid id, CancellationToken cancellationToken)
    {
        return await mediator.Send(new GetFlashcardQuery(id), cancellationToken);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateFlashcard(CreateFlashcardCommand command, CancellationToken cancellationToken)
    {
        var id = await mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetFlashcard), new { id }, id);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteFlashcard(Guid id, CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteFlashcardCommand(id), cancellationToken);
        return NoContent();
    }
}
