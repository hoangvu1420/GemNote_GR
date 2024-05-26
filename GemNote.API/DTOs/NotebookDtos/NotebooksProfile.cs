using AutoMapper;
using GemNote.API.Models;

namespace GemNote.API.DTOs.NotebookDtos;

public class NotebooksProfile : Profile
{
	public NotebooksProfile()
	{
		CreateMap<Notebook, NotebookDto>()
			.ForMember(dest => dest.SectionQty, opt => opt.MapFrom(src => src.Sections.Count))
			.ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name));
		CreateMap<CreateNotebookDto, Notebook>();
		CreateMap<UpdateNotebookDto, Notebook>();
	}
}