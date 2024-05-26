using GemNote.API.Infrastructure.DataContext;
using GemNote.API.Models;
using GemNote.API.Repositories.Contracts;

namespace GemNote.API.Repositories.Implementations;

public class NotebookRepository(GemNoteDbContext dbContext) : Repository<Notebook>(dbContext), INotebookRepository
{
	private readonly GemNoteDbContext _dbContext1 = dbContext;

	public async Task<Notebook?> UpdateAsync(Notebook notebook)
	{
		var notebookToUpdate = await _dbContext1.Notebooks.FindAsync(notebook.Id);
		if (notebookToUpdate == null)
		{
			return null;
		}

		notebookToUpdate.Name = notebook.Name;
		notebookToUpdate.Description = notebook.Description;
		notebookToUpdate.CategoryId = notebook.CategoryId;
		notebookToUpdate.UpdatedAt = DateTime.UtcNow;

		await _dbContext1.SaveChangesAsync();

		return notebookToUpdate;
	}
}