using GemNote.API.Models;

namespace GemNote.API.Repositories.Contracts;

public interface ISectionRepository : IRepository<Section>
{
	Task<Section?> UpdateAsync(Section section);
}