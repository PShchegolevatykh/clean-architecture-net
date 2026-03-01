using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Flashcard> Flashcards { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
