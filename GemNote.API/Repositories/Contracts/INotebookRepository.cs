using GemNote.API.Models;

namespace GemNote.API.Repositories.Contracts;

public interface INotebookRepository : IRepository<Notebook>
{
	Task<Notebook?> UpdateAsync(Notebook notebook);
}