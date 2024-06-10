using AutoMapper;
using GemNote.API.Models;

namespace GemNote.API.DTOs.UnitDtos;

public class UnitProfile : Profile
{
	public UnitProfile()
	{
		CreateMap<Unit, UnitDto>()
			.ForMember(dest => dest.CardQty, opt => opt.MapFrom(src => src.Flashcards.Count));
		CreateMap<CreateUnitDto, Unit>();
		CreateMap<UpdateUnitDto, Unit>();

		CreateMap<Unit, DetailedUnitDto>()
			.ForMember(dest => dest.CardQty, opt => opt.MapFrom(src => src.Flashcards.Count))
			.ForMember(dest => dest.SectionName, opt => opt.MapFrom(src => src.Section.Name))
			.ForMember(dest => dest.NotebookName, opt => opt.MapFrom(src => src.Section.Notebook.Name));
	}
}