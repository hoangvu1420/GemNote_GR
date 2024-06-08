using GemNote.API.Infrastructure.DataContext;
using GemNote.API.Models;
using GemNote.API.Repositories.Contracts;

namespace GemNote.API.Repositories.Implementations;

public class UnitRepository(GemNoteDbContext dbContext) : Repository<Unit>(dbContext), IUnitRepository
{
	private readonly GemNoteDbContext _dbContext1 = dbContext;

	public async Task<Unit?> UpdateAsync(Unit unit)
	{
		var unitToUpdate = await _dbContext1.Units.FindAsync(unit.Id);
		if (unitToUpdate == null)
		{
			return null;
		}

		unitToUpdate.Name = unit.Name;
		unitToUpdate.Description = unit.Description;
		unitToUpdate.UpdatedAt = DateTime.UtcNow;

		await _dbContext1.SaveChangesAsync();

		return unitToUpdate;
	}
}