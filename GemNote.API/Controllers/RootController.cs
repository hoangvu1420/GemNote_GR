using Microsoft.AspNetCore.Mvc;

namespace GemNote.API.Controllers;

[ApiController]
[Route("/")]
public class RootController : ControllerBase
{
	[HttpGet]
	public ActionResult<string> Get()
	{
		return Ok("Welcome to GemNote API!\nRefer to the API documentation in the link below:");
	}
}