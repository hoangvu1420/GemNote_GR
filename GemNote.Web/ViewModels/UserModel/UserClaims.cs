namespace GemNote.Web.ViewModels.UserModel;

public record UserClaims(string? Id, string? Email, string? FullName, List<string>? Roles);