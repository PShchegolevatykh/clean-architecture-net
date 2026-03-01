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
    public async Task<ActionResult<List<FlashcardDto>>> GetFlashcards()
    {
        return await mediator.Send(new GetFlashcardsListQuery());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<FlashcardDetailDto>> GetFlashcard(Guid id)
    {
        return await mediator.Send(new GetFlashcardQuery(id));
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateFlashcard(CreateFlashcardCommand command)
    {
        var id = await mediator.Send(command);
        return CreatedAtAction(nameof(GetFlashcard), new { id }, id);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteFlashcard(Guid id)
    {
        await mediator.Send(new DeleteFlashcardCommand(id));
        return NoContent();
    }
}
