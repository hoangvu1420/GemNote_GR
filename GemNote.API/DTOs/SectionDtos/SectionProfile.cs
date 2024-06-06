using AutoMapper;
using GemNote.API.Models;

namespace GemNote.API.DTOs.SectionDtos;

public class SectionProfile : Profile
{
	public SectionProfile()
	{
		CreateMap<Section, SectionDto>()
			.ForMember(dest => dest.NotebookName, opt => opt.MapFrom(src => src.Notebook.Name))
			.ForMember(dest => dest.UnitQty, opt => opt.MapFrom(src => src.Units.Count));
		CreateMap<CreateSectionDto, Section>();
		CreateMap<UpdateSectionDto, Section>();
	}
}