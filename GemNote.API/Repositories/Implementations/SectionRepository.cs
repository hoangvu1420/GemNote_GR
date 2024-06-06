using GemNote.API.Infrastructure.DataContext;
using GemNote.API.Models;
using GemNote.API.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace GemNote.API.Repositories.Implementations;

public class SectionRepository(GemNoteDbContext dbContext) : Repository<Section>(dbContext), ISectionRepository
{
	private readonly GemNoteDbContext _dbContext1 = dbContext;

	public async Task<Section?> UpdateAsync(Section section)
	{
		var sectionToUpdate = await _dbContext1.Sections.FindAsync(section.Id);
		if (sectionToUpdate == null)
		{
			return null;
		}

		sectionToUpdate.Name = section.Name;
		sectionToUpdate.Description = section.Description;
		sectionToUpdate.UpdatedAt = DateTime.UtcNow;

		await _dbContext1.SaveChangesAsync();

		return sectionToUpdate;
	}

	public async Task<IEnumerable<Section>> GetSectionsByNotebookIdAsync(int notebookId)
	{
		return await _dbContext1.Sections.Where(s => s.NotebookId == notebookId).ToListAsync();
	}

	public async Task<int> GetUnitQtyAsync(int sectionId)
	{
		var sections = _dbContext1.Sections.Include("Units");
		var section = await sections.FirstOrDefaultAsync(s => s.Id == sectionId);
		return section == null ? 0 : section.Units.Count;
	}
}