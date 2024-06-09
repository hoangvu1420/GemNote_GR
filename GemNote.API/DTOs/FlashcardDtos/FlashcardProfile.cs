using AutoMapper;
using GemNote.API.Models;

namespace GemNote.API.DTOs.FlashcardDtos;

public class FlashcardProfile : Profile
{
	public FlashcardProfile()
	{
		CreateMap<Flashcard, FlashcardDto>()
			.ForMember(dest => dest.UnitId, opt => opt.MapFrom(src => src.Unit.Id))
			.ForMember(dest => dest.UnitName, opt => opt.MapFrom(src => src.Unit.Name));

		CreateMap<CreateFlashcardDto, Flashcard>()
			.ForMember(dest => dest.Unit, opt => opt.Ignore());

		CreateMap<UpdateFlashcardDto, Flashcard>()
			.ForMember(dest => dest.Unit, opt => opt.Ignore());
	}
}