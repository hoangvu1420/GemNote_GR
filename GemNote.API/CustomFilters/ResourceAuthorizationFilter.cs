using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using GemNote.API.Models;
using GemNote.API.Repositories.Contracts;
using GemNote.API.StaticDetails;

namespace GemNote.API.CustomFilters;

public class ResourceAuthorizationFilter<T>(
	IRepository<T> repository, 
	UserManager<AppUser> userManager)
	: IAsyncAuthorizationFilter
	where T : class
{
	private readonly UserManager<AppUser> _userManager = userManager;

	public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
	{
		var userId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
		var userRoles = context.HttpContext.User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();

		if (userRoles.Contains(UserRoles.Admin))
		{
			return; // Admins can access any resource
		}

		await CheckQueryParams(context, userId);

		await CheckRouteParams(context, userId);
	}

	private async Task CheckRouteParams(AuthorizationFilterContext context, string? userId)
	{
		var routeValues = context.RouteData.Values;

		if (routeValues.TryGetValue("unitId", out var unitIdValue))
		{
			// var unitId = int.Parse(unitIdValue.ToString());
			// var unit = await repository.GetAsync(filter: u => (u as Unit)!.Id == unitId, includeProperties: "Section");
			// var section = await repository.GetAsync(filter: s => (s as Section)!.Id == (unit as Unit)!.SectionId, includeProperties: "Notebook");
			//
			// if (unit == null || section == null)
			// {
			// 	return; // return without raising an error, the controller will handle the 404 response
			// }
			// if ((section as Section)!.Notebook.AppUserId != userId)
			// {
			// 	context.Result = new ForbidResult();
			// }
		}
		else if (routeValues.TryGetValue("sectionId", out var sectionIdValue))
		{
			var sectionId = int.Parse(sectionIdValue.ToString());
			var section = await repository.GetAsync(filter: s => (s as Section)!.Id == sectionId, includeProperties: "Notebook");

			if (section == null)
			{
				return; // return without raising an error, the controller will handle the 404 response
			}
			if ((section as Section)!.Notebook.AppUserId != userId)
			{
				context.Result = new ForbidResult();
			}
		}
		else if (routeValues.TryGetValue("notebookId", out var notebookIdValue))
		{
			var notebookId = int.Parse(notebookIdValue.ToString());
			var notebook = await repository.GetAsync(filter: n => (n as Notebook)!.Id == notebookId);

			if (notebook == null)
			{
				return; // return without raising an error, the controller will handle the 404 response
			}
			if ((notebook as Notebook)!.AppUserId != userId)
			{
				context.Result = new ForbidResult();
			}
		}
		else if (routeValues.TryGetValue("userId", out var userIdValue))
		{
			var routeUserId = userIdValue.ToString();

			if (routeUserId != userId)
			{
				context.Result = new ForbidResult();
			}
		}
	}

	private async Task CheckQueryParams(AuthorizationFilterContext context, string? userId)
	{
		var query = context.HttpContext.Request.Query;

		if (query.TryGetValue("notebookId", out var notebookIdValue))
		{
			var notebookId = int.Parse(notebookIdValue.ToString());
			var notebook = await repository.GetAsync(n => (n as Notebook)!.Id == notebookId);

			if (notebook == null)
			{
				return; // return without raising an error, the controller will handle the 404 response
			}
			if ((notebook as Notebook)!.AppUserId != userId)
			{
				context.Result = new ForbidResult();
			}
		}
		else if (query.TryGetValue("userId", out var userIdValue))
		{
			var routeUserId = userIdValue.ToString();

			if (routeUserId != userId)
			{
				context.Result = new ForbidResult();
			}
		}
	}
}