using AutoMapper;
using GemNote.API.Models;

namespace GemNote.API.DTOs.ReviewDtos;

public class ReviewProfile : Profile
{
	public ReviewProfile()
	{
		CreateMap<CardReviewSession, ReviewDto>();
		CreateMap<CreateReviewDto, CardReviewSession>();
	}
}