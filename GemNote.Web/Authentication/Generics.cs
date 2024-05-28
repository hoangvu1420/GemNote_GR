using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using GemNote.Web.ViewModels.UserModel;

namespace GemNote.Web.Authentication;

public static class Generics
{
	public static ClaimsPrincipal GetClaimsPrincipalFromClaims(UserClaims model)
	{
		var claims = new List<Claim>
		{
			new(ClaimTypes.NameIdentifier, model.Id!),
			new(ClaimTypes.Name, model.FullName!),
			new(ClaimTypes.Email, model.Email!)
		};
		claims.AddRange(model.Roles!.Select(role => new Claim(ClaimTypes.Role, role)));

		return new ClaimsPrincipal(new ClaimsIdentity(claims, "JwtAuth"));
	}

	public static UserClaims GetUserClaimsFromJwt(string token)
	{
		var handler = new JwtSecurityTokenHandler();
		var jsonToken = handler.ReadJwtToken(token);

		var id = jsonToken.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
		var email = jsonToken.Claims.First(claim => claim.Type == ClaimTypes.Email).Value;
		var fullName = jsonToken.Claims.First(claim => claim.Type == ClaimTypes.Name).Value;
		var roles = jsonToken.Claims.Where(claim => claim.Type == ClaimTypes.Role).Select(claim => claim.Value).ToList();

		return new UserClaims(id, email, fullName, roles);
	}
}