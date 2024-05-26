using GemNote.API.Infrastructure.DataContext;
using GemNote.API.Models;
using GemNote.API.Repositories.Contracts;
using GemNote.API.Repositories.Implementations;

namespace PetCareSystem.Repositories.Implementations;

public class RefreshTokenRepository(GemNoteDbContext dbContext)
	: Repository<RefreshToken>(dbContext), IRefreshTokenRepository
{
	private readonly GemNoteDbContext _dbContext1 = dbContext;

	public async Task SetRevoked(RefreshToken refreshToken)
	{
		refreshToken.IsRevoked = true;
		await _dbContext1.SaveChangesAsync();
	}
}