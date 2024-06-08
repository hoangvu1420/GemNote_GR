using GemNote.API.CustomFilters;
using GemNote.API.DTOs;
using GemNote.API.DTOs.UnitDtos;
using GemNote.API.Models;
using GemNote.API.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GemNote.API.Controllers;

[Route("api/units")]
[ApiController]
[Authorize]
public class UnitController(IUnitService unitService) : ControllerBase
{
	private ApiResponse _response = new();

	[HttpGet(Name = "GetUnits")]
	[ResourceAuthorize(typeof(Section))] // Custom filter to authorize access to resources
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	public async Task<IActionResult> GetUnitsAsync([FromQuery] int? sectionId)
	{
		try
		{
			if (sectionId.HasValue)
			{
				_response = await unitService.GetUnitsBySectionIdAsync(sectionId.Value);
				if (!_response.IsSucceed)
					return NotFound(_response);

				return Ok(_response);
			}

			_response = await unitService.GetUnitsAsync();
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

	[HttpGet("{unitId}", Name = "GetUnitById")]
	[ResourceAuthorize(typeof(Unit))] // Custom filter to authorize access to resources
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	public async Task<IActionResult> GetUnitByIdAsync(int unitId)
	{
		try
		{
			_response = await unitService.GetUnitByIdAsync(unitId);
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

	[HttpPost(Name = "CreateUnit")]
	[ResourceAuthorize(typeof(Section))] // Custom filter to authorize access to resources
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status201Created)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	public async Task<IActionResult> CreateUnitAsync([FromBody] CreateUnitDto unitDto)
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

			_response = await unitService.CreateUnitAsync(unitDto);
			if (!_response.IsSucceed)
				return BadRequest(_response);

			var createdUnitId = (_response.Data as UnitDto)!.Id;

			return CreatedAtRoute("GetUnitById", new { unitId = createdUnitId }, _response);
		}
		catch (Exception e)
		{
			_response.IsSucceed = false;
			_response.ErrorMessages = [e.Message];
			return StatusCode(StatusCodes.Status500InternalServerError, _response);
		}
	}

	[HttpPut("{unitId}", Name = "UpdateUnit")]
	[ResourceAuthorize(typeof(Unit))] // Custom filter to authorize access to resources
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	public async Task<IActionResult> UpdateUnitAsync(int unitId, [FromBody] UpdateUnitDto unitDto)
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

			_response = await unitService.UpdateUnitAsync(unitId, unitDto);
			if (!_response.IsSucceed)
				return BadRequest(_response);

			return Ok(_response);
		}
		catch (Exception e)
		{
			_response.IsSucceed = false;
			_response.ErrorMessages = [e.Message];
			return StatusCode(StatusCodes.Status500InternalServerError, _response);
		}
	}

	[HttpDelete("{unitId}", Name = "DeleteUnit")]
	[ResourceAuthorize(typeof(Unit))] // Custom filter to authorize access to resources
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	public async Task<IActionResult> DeleteUnitAsync(int unitId)
	{
		try
		{
			_response = await unitService.DeleteUnitAsync(unitId);
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