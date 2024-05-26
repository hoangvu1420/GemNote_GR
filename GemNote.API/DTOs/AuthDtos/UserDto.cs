﻿namespace GemNote.API.DTOs.AuthDtos;

public class UserDto
{
	public string Id { get; set; }
	public string Email { get; set; }
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public List<string> Roles { get; set; } = [];
	public string? Language { get; set; }
	public string? AvatarUrl { get; set; }
}