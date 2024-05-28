namespace GemNote.Web.ViewModels.UserModel;

public class UserVm
{
	public string? Id { get; set; }
	public string? Email { get; set; }
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
	public List<string>? Roles { get; set; } 
	public string? Language { get; set; }
	public string? AvatarUrl { get; set; }
}