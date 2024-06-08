using GemNote.API.Models;

namespace GemNote.API.Repositories.Contracts;

public interface IUnitRepository : IRepository<Unit>
{
	Task<Unit?> UpdateAsync(Unit unit);
}