using System.ComponentModel.DataAnnotations;

namespace GemNote.API.DTOs.AuthDtos;

public class RefreshTokenRequestDto
{
	[Required]
	public string UserId { get; set; }
	[Required]
	public string RefreshToken { get; set; }
}