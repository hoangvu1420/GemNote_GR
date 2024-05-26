﻿using System.ComponentModel.DataAnnotations;

namespace GemNote.API.DTOs.AuthDtos;

public class LoginRequestDto
{
	[Required(ErrorMessage = "Email is required")]
	public string Email { get; init; }

	[Required(ErrorMessage = "Password is required")]
	public string Password { get; init; }
}