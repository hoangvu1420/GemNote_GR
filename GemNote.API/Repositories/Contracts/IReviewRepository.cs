using GemNote.API.Models;

namespace GemNote.API.Repositories.Contracts;

public interface IReviewRepository : IRepository<CardReviewSession>
{
	Task CreateRangeAsync(IEnumerable<CardReviewSession> cardReviewSessions);
	Task DeleteRangeAsync(IEnumerable<CardReviewSession> cardReviewSessions);
}