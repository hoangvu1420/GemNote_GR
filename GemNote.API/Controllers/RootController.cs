using Microsoft.AspNetCore.Mvc;

namespace GemNote.API.Controllers;

[ApiController]
[Route("/")]
public class RootController : ControllerBase
{
	[HttpGet(Name = "Root")]
	public ActionResult<string> Get()
	{
		var message = "Welcome to GemNote API!";
		return Ok(message);
	}
}