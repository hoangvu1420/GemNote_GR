namespace GemNote.Web.Services.Contracts;

public interface IToastMessageService
{
	void PushMessage(string? message);
	string? PopMessage();
}