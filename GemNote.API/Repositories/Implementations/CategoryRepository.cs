using GemNote.API.Infrastructure.DataContext;
using GemNote.API.Models;
using GemNote.API.Repositories.Contracts;

namespace GemNote.API.Repositories.Implementations;

public class CategoryRepository(GemNoteDbContext dbContext) : Repository<Category>(dbContext), ICategoryRepository
{
}