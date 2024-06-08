﻿using System.ComponentModel.DataAnnotations;

namespace GemNote.API.DTOs.UnitDtos;

public class UpdateUnitDto
{
	public int Id { get; set; }
	[Required]
	public string Name { get; set; }
	public string Description { get; set; }
}