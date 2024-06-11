using System.ComponentModel.DataAnnotations;

namespace GemNote.Web.ViewModels.RequestModels;

public class RefreshTokenRequest
{
	[Required]
	public string UserId { get; set; }
	[Required]
	public string RefreshToken { get; set; }
}