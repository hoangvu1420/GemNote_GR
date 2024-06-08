using AutoMapper;
using GemNote.API.Models;

namespace GemNote.API.DTOs.UnitDtos;

public class UnitProfile : Profile
{
	public UnitProfile()
	{
		CreateMap<Unit, UnitDto>()
			.ForMember(dest => dest.SectionName, opt => opt.MapFrom(src => src.Section.Name))
			.ForMember(dest => dest.CardQty, opt => opt.MapFrom(src => src.Flashcards.Count));
		CreateMap<CreateUnitDto, Unit>();
		CreateMap<UpdateUnitDto, Unit>();
	}
}