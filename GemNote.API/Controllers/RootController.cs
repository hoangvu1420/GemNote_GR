using Microsoft.AspNetCore.Mvc;

namespace GemNote.API.Controllers;

[ApiController]
[Route("/")]
public class RootController : ControllerBase
{
	[HttpGet]
	public ActionResult<string> Get()
	{
		var message = @"Welcome to GemNote API!
Refer to the API documentation in the link below:";
		return Ok(message);
	}
}