using GemNote.API.Infrastructure.DataContext;
using GemNote.API.Models;
using GemNote.API.Repositories.Contracts;

namespace GemNote.API.Repositories.Implementations;

public class ReviewRepository(GemNoteDbContext dbContext) : Repository<CardReviewSession>(dbContext), IReviewRepository
{
	private readonly GemNoteDbContext _dbContext1 = dbContext;

	public async Task CreateRangeAsync(IEnumerable<CardReviewSession> cardReviewSessions)
	{
		await _dbContext1.CardReviewSessions.AddRangeAsync(cardReviewSessions);
		await _dbContext1.SaveChangesAsync();
	}

	public async Task DeleteRangeAsync(IEnumerable<CardReviewSession> cardReviewSessions)
	{
		_dbContext1.CardReviewSessions.RemoveRange(cardReviewSessions);
		await _dbContext1.SaveChangesAsync();
	}
}