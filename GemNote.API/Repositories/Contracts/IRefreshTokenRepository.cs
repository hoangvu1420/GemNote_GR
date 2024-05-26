using GemNote.API.Models;

namespace GemNote.API.Repositories.Contracts;

public interface IRefreshTokenRepository : IRepository<RefreshToken>
{
	public Task SetRevoked(RefreshToken refreshToken);
}