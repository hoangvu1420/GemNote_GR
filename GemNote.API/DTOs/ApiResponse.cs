namespace GemNote.API.DTOs;

public class ApiResponse
{
	public bool IsSucceed { get; set; }
	public List<string>? ErrorMessages { get; set; }
	public object? Data { get; set; }
}