﻿namespace GemNote.API.DTOs.FlashcardDtos;

public class UpdateFlashcardDto
{
	public int Id { get; set; }
	public string Front { get; set; }
	public string Back { get; set; }
}