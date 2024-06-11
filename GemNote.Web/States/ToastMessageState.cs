namespace GemNote.Web.States;

public class ToastMessageState
{
	private readonly List<string?> _messages = new();

	public void PushMessage(string? message)
	{
		_messages.Add(message);
	}

	public string? PopMessage()
	{
		if (_messages.Count == 0)
		{
			return null;
		}

		var message = _messages[0];
		_messages.RemoveAt(0);

		return message;
	}
}