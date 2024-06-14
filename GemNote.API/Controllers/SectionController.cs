using GemNote.API.CustomFilters;
using GemNote.API.DTOs;
using GemNote.API.DTOs.SectionDtos;
using GemNote.API.Models;
using GemNote.API.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GemNote.API.Controllers;

[Route("api/sections")]
[ApiController]
[Authorize]
public class SectionController(ISectionService sectionService) : ControllerBase
{
	private ApiResponse _response = new();

	[HttpGet(Name = "GetSections")]
	[ResourceAuthorize(typeof(Notebook))] // Custom filter to authorize access to resources
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	public async Task<IActionResult> GetSectionsAsync([FromQuery] int? notebookId)
	{
		try
		{
			if (notebookId.HasValue)
			{
				_response = await sectionService.GetSectionsByNotebookIdAsync(notebookId.Value);
				if (!_response.IsSucceed)
					return NotFound(_response);

				return Ok(_response);
			}

			_response = await sectionService.GetSectionsAsync();
			if (!_response.IsSucceed)
				return NotFound(_response);

			return Ok(_response);
		}
		catch (Exception e)
		{
			_response.IsSucceed = false;
			_response.ErrorMessages = [e.Message];
			return StatusCode(StatusCodes.Status500InternalServerError, _response);
		}
	}

	[HttpGet("{sectionId}", Name = "GetSectionById")]
	[ResourceAuthorize(typeof(Section))] // Custom filter to authorize access to resources
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	public async Task<IActionResult> GetSectionByIdAsync(int sectionId)
	{
		try
		{
			_response = await sectionService.GetSectionByIdAsync(sectionId);
			if (!_response.IsSucceed)
				return NotFound(_response);

			return Ok(_response);
		}
		catch (Exception e)
		{
			_response.IsSucceed = false;
			_response.ErrorMessages = [e.Message];
			return StatusCode(StatusCodes.Status500InternalServerError, _response);
		}
	}

	[HttpPost(Name = "CreateSection")]
	[ResourceAuthorize(typeof(Section))] // Custom filter to authorize access to resources
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status201Created)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	public async Task<IActionResult> CreateAsync([FromBody] CreateSectionDto sectionDto)
	{
		try
		{
			if (!ModelState.IsValid)
			{
				var errorMessages = ModelState.Values
					.SelectMany(v => v.Errors)
					.Select(e => e.ErrorMessage)
					.ToList();

				_response.IsSucceed = false;
				_response.ErrorMessages = errorMessages;
				return BadRequest(_response);
			}

			_response = await sectionService.CreateSectionAsync(sectionDto);
			if (!_response.IsSucceed)
				return BadRequest(_response);

			var createdSectionId = (_response.Data as SectionDto)!.Id;

			return CreatedAtRoute("GetSectionById", new { sectionId = createdSectionId }, _response);
		}
		catch (Exception e)
		{
			_response.IsSucceed = false;
			_response.ErrorMessages = [e.Message];
			return StatusCode(StatusCodes.Status500InternalServerError, _response);
		}
	}

	[HttpPut("{sectionId}", Name = "UpdateSection")]
	[ResourceAuthorize(typeof(Section))] // Custom filter to authorize access to resources
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	public async Task<IActionResult> UpdateAsync(int sectionId, [FromBody] UpdateSectionDto sectionDto)
	{
		try
		{
			if (!ModelState.IsValid)
			{
				var errorMessages = ModelState.Values
					.SelectMany(v => v.Errors)
					.Select(e => e.ErrorMessage)
					.ToList();

				_response.IsSucceed = false;
				_response.ErrorMessages = errorMessages;
				return BadRequest(_response);
			}

			_response = await sectionService.UpdateSectionAsync(sectionId, sectionDto);
			if (!_response.IsSucceed)
				return NotFound(_response);

			return Ok(_response);
		}
		catch (Exception e)
		{
			_response.IsSucceed = false;
			_response.ErrorMessages = [e.Message];
			return StatusCode(StatusCodes.Status500InternalServerError, _response);
		}
	}

	[HttpDelete("{sectionId}", Name = "DeleteSection")]
	[ResourceAuthorize(typeof(Section))] // Custom filter to authorize access to resources
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	public async Task<IActionResult> DeleteAsync(int sectionId)
	{
		try
		{
			_response = await sectionService.DeleteSectionAsync(sectionId);
			if (!_response.IsSucceed)
				return NotFound(_response);

			return Ok(_response);
		}
		catch (Exception e)
		{
			_response.IsSucceed = false;
			_response.ErrorMessages = [e.Message];
			return StatusCode(StatusCodes.Status500InternalServerError, _response);
		}
	}
}