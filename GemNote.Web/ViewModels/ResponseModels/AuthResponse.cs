﻿namespace GemNote.Web.ViewModels.ResponseModels;

public class AuthResponse
{
	public bool IsSucceed { get; init; }
	public List<string> ErrorMessages { get; init; } = [];
	public string Message { get; init; }
}