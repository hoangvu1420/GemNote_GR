using GemNote.API.Infrastructure.DataContext;
using GemNote.API.Models;
using GemNote.API.Repositories.Contracts;

namespace GemNote.API.Repositories.Implementations;

public class FlashcardRepository(GemNoteDbContext dbContext) : Repository<Flashcard>(dbContext), IFlashcardRepository
{
	private readonly GemNoteDbContext _dbContext1 = dbContext;

	public async Task<Flashcard?> UpdateAsync(Flashcard flashcard)
	{
		var flashcardToUpdate = await _dbContext1.Flashcards.FindAsync(flashcard.Id);
		if (flashcardToUpdate == null)
		{
			return null;
		}

		flashcardToUpdate.Front = flashcard.Front;
		flashcardToUpdate.Back = flashcard.Back;
		flashcardToUpdate.UpdatedAt = DateTime.UtcNow;

		await _dbContext1.SaveChangesAsync();

		return flashcardToUpdate;
	}
}