using System.Security.Claims;
using GemNote.API.CustomFilters;
using GemNote.API.DTOs;
using GemNote.API.DTOs.NotebookDtos;
using GemNote.API.Models;
using GemNote.API.Services.Contracts;
using GemNote.API.StaticDetails;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GemNote.API.Controllers;

[Route("api/notebooks")]
[ApiController]
[Authorize]
public class NotebookController(INotebookService notebookService) : ControllerBase
{
	private ApiResponse _response = new();

	[HttpGet(Name = "GetNotebooks")]
	[ResourceAuthorize(typeof(Notebook))] // Custom filter to authorize access to resources
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	public async Task<IActionResult> GetNotebooksAsync([FromQuery] string? userId)
	{
		try
		{
			if (!string.IsNullOrEmpty(userId))
			{
				_response = await notebookService.GetNotebooksByUserIdAsync(userId);
				if (!_response.IsSucceed)
					return NotFound(_response);

				return Ok(_response);
			}

			var userRoles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();

			if (!userRoles.Contains(UserRoles.Admin)) return Forbid();

			_response = await notebookService.GetNotebooksAsync();
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

	[HttpGet("{notebookId}", Name = "GetNotebookById")]
	[ResourceAuthorize(typeof(Notebook))] // Custom filter to authorize access to resources
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	public async Task<IActionResult> GetNotebookById(int notebookId)
	{
		try
		{
			_response = await notebookService.GetNotebookByIdAsync(notebookId);
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

	[HttpPost(Name = "CreateNotebook")]
	[ResourceAuthorize(typeof(Notebook))] // Custom filter to authorize access to resources
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status201Created)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	public async Task<IActionResult> CreateNotebookAsync([FromBody] CreateNotebookDto notebook)
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

			_response = await notebookService.CreateNotebookAsync(notebook);
			if (!_response.IsSucceed)
				return BadRequest(_response);

			var createdNotebookId = (_response.Data as NotebookDto)!.Id;

			return CreatedAtRoute("GetNotebookById", new { notebookId = createdNotebookId }, _response);
		}
		catch (Exception e)
		{
			_response.IsSucceed = false;
			_response.ErrorMessages = [e.Message];
			return StatusCode(StatusCodes.Status500InternalServerError, _response);
		}
	}

	[HttpPut("{notebookId}", Name = "UpdateNotebook")]
	[ResourceAuthorize(typeof(Notebook))] // Custom filter to authorize access to resources
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	public async Task<IActionResult> UpdateNotebookAsync(int notebookId, [FromBody] UpdateNotebookDto notebookDto)
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

			_response = await notebookService.UpdateNotebookAsync(notebookId, notebookDto);
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

	[HttpDelete("{notebookId}", Name = "DeleteNotebook")]
	[ResourceAuthorize(typeof(Notebook))] // Custom filter to authorize access to resources
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	public async Task<IActionResult> DeleteNotebookAsync(int notebookId)
	{
		try
		{
			_response = await notebookService.DeleteNotebookAsync(notebookId);
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