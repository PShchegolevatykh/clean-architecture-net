using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Infrastructure.Persistence;

public static class ApplicationDbContextSeed
{
    public static async Task SeedSampleDataAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        if (context.Database.IsSqlite())
        {
            await context.Database.EnsureCreatedAsync();
        }

        if (!await context.Flashcards.AnyAsync())
        {
            context.Flashcards.AddRange(
                new Flashcard { Front = "Clean Architecture", Back = "A software design philosophy that separates the concerns of a software system into concentric layers.", Difficulty = 3 },
                new Flashcard { Front = "CQRS", Back = "Command Query Responsibility Segregation - a pattern that separates read and update operations for a data store.", Difficulty = 4 },
                new Flashcard { Front = "MediatR", Back = "A library that helps implement the Mediator pattern in .NET.", Difficulty = 2 }
            );

            await context.SaveChangesAsync();
        }
    }
}
