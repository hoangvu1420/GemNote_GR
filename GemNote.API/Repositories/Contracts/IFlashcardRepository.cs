using GemNote.API.Models;

namespace GemNote.API.Repositories.Contracts;

public interface IFlashcardRepository : IRepository<Flashcard>
{
	Task<Flashcard?> UpdateAsync(Flashcard flashcard);
}