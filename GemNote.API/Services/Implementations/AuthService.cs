using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using GemNote.API.Models;
using GemNote.API.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using GemNote.API.DTOs.AuthDtos;
using GemNote.API.Repositories.Contracts;
using GemNote.API.StaticDetails;

namespace GemNote.API.Services.Implementations;

public class AuthService(
	UserManager<AppUser> userManager,
	RoleManager<IdentityRole> roleManager,
	IConfiguration configuration,
	IRefreshTokenRepository refreshTokenRepository)
	: IAuthService
{
	public async Task<AuthResponse> RegisterAsync(RegisterRequestDto request)
	{
		var isUserExist = await userManager.FindByEmailAsync(request.Email);
		if (isUserExist != null)
		{
			return new AuthResponse
			{
				IsSucceed = false,
				ErrorMessages = ["Email already exists"]
			};
		}

		var language = string.IsNullOrWhiteSpace(request.Language) ? "en" : request.Language;
		var avatarUrl = string.IsNullOrWhiteSpace(request.AvatarUrl) ? PictureStock.GetRandomPicture() : request.AvatarUrl;
		var newUser = new AppUser
		{
			UserName = request.Email,
			Email = request.Email,
			FirstName = request.FirstName,
			LastName = request.LastName,
			Language = language,
			AvatarUrl = avatarUrl
		};

		var result = await userManager.CreateAsync(newUser, request.Password);

		if (!result.Succeeded)
		{
			return new AuthResponse
			{
				IsSucceed = false,
				ErrorMessages = result.Errors.Select(e => e.Description).ToList()
			};
		}

		await userManager.AddToRoleAsync(newUser, UserRoles.User);

		return new AuthResponse
		{
			IsSucceed = true,
			Message = "User created successfully"
		};
	}

	public async Task<LoginResponse> LoginAsync(LoginRequestDto request)
	{
		var user = await userManager.FindByEmailAsync(request.Email);
		if (user == null)
		{
			return new LoginResponse
			{
				IsSucceed = false,
				ErrorMessages = ["Email is not registered"]
			};
		}

		var result = await userManager.CheckPasswordAsync(user, request.Password);

		if (!result)
		{
			return new LoginResponse
			{
				IsSucceed = false,
				ErrorMessages = ["Password is incorrect"]
			};
		}

		var userRoles = await userManager.GetRolesAsync(user);

		var claims = GetClaims(user, userRoles);

		var accessToken = GenerateJwtToken(claims);
		var currRefreshToken = await refreshTokenRepository.GetAsync(filter: r => r.UserId == user.Id && r.IsRevoked == false);
		var newRefreshToken = GenerateRefreshToken(user.Id);

		if (currRefreshToken != null)
		{
			await refreshTokenRepository.SetRevoked(currRefreshToken);
		}
		await refreshTokenRepository.CreateAsync(newRefreshToken);

		return new LoginResponse
		{
			IsSucceed = true,
			Token = accessToken.AccessToken,
			ExpirationDate = accessToken.ExpirationDate,
			RefreshToken = newRefreshToken.Token,
			UserInfo = new UserDto
			{
				Id = user.Id,
				Email = user.Email,
				FirstName = user.FirstName,
				LastName = user.LastName,
				Language = user.Language,
				AvatarUrl = user.AvatarUrl,
				Roles = userRoles.ToList()
			}
		};
	}

	private IEnumerable<Claim> GetClaims(AppUser user, IEnumerable<string> roles)
	{
		var claims = new List<Claim>
		{
			new(ClaimTypes.NameIdentifier, user.Id),
			new(ClaimTypes.Email, user.Email),
			new(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
			new("JWT_ID", Guid.NewGuid().ToString())
		};
		claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
		return claims;
	}

	private Token GenerateJwtToken(IEnumerable<Claim> claims)
	{
		var jwtTokenHandler = new JwtSecurityTokenHandler();
		var key = Encoding.ASCII.GetBytes(configuration["Jwt:Secret"]);

		var expirationDate = DateTime.Now.AddMinutes(10);

		var tokenObject = new JwtSecurityToken(
			issuer: configuration["JWT:Issuer"],
			audience: configuration["JWT:Audience"],
			expires: expirationDate,
			claims: claims,
			signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key),
				SecurityAlgorithms.HmacSha256Signature)
		);

		var token = jwtTokenHandler.WriteToken(tokenObject);
		return new Token
		{
			AccessToken = token,
			ExpirationDate = expirationDate
		};
	}

	private RefreshToken GenerateRefreshToken(string userId)
	{
		var randomNumber = new byte[32];
		using var rng = RandomNumberGenerator.Create();
		rng.GetBytes(randomNumber);
		var token = Convert.ToBase64String(randomNumber);

		return new RefreshToken
		{
			UserId = userId,
			Token = token,
			ExpiryDate = DateTime.UtcNow.AddDays(15),
			IsRevoked = false
		};
	}

	public async Task<RefreshTokenResponse> IssueRefreshTokenAsync(RefreshTokenRequestDto request)
	{
		var user = await userManager.FindByIdAsync(request.UserId);
		if (user == null)
		{
			return new RefreshTokenResponse
			{
				IsSucceed = false,
				ErrorMessages = ["Invalid UserId"]
			};
		}

		var currRefreshToken = await refreshTokenRepository.GetAsync(filter: r => r.Token == request.RefreshToken);
		if (currRefreshToken == null || currRefreshToken.ExpiryDate <= DateTime.UtcNow)
		{
			return new RefreshTokenResponse
			{
				IsSucceed = false,
				ErrorMessages = ["Invalid or expired refresh token"]
			};
		}

		if (currRefreshToken.UserId != request.UserId)
		{
			return new RefreshTokenResponse
			{
				IsSucceed = false,
				ErrorMessages = ["Invalid refresh token for this user"]
			};
		}

		if (currRefreshToken.IsRevoked)
		{
			return new RefreshTokenResponse
			{
				IsSucceed = false,
				ErrorMessages = ["Refresh token is revoked"]
			};
		}

		var userRoles = await userManager.GetRolesAsync(user);
		var claims = GetClaims(user, userRoles);

		var accessToken = GenerateJwtToken(claims);
		var newRefreshToken = GenerateRefreshToken(user.Id);

		// Revoke the current refresh token and create a new one
		await refreshTokenRepository.SetRevoked(currRefreshToken);
		await refreshTokenRepository.CreateAsync(newRefreshToken);

		return new RefreshTokenResponse
		{
			IsSucceed = true,
			Token = accessToken.AccessToken,
			ExpirationDate = accessToken.ExpirationDate,
			RefreshToken = newRefreshToken.Token
		};
	}

	public async Task<AuthResponse> AddToRoleAsync(AddToRoleDto request)
	{
		var user = await userManager.FindByEmailAsync(request.Email);
		if (user == null)
		{
			return new AuthResponse
			{
				IsSucceed = false,
				ErrorMessages = ["Email is not registered"]
			};
		}

		var role = await roleManager.FindByNameAsync(request.Role);
		if (role == null)
		{
			return new AuthResponse
			{
				IsSucceed = false,
				ErrorMessages = ["Role does not exist"]
			};
		}

		var result = await userManager.AddToRoleAsync(user, request.Role);

		if (!result.Succeeded)
		{
			return new AuthResponse
			{
				IsSucceed = false,
				ErrorMessages = result.Errors.Select(e => e.Description).ToList()
			};
		}

		return new AuthResponse
		{
			IsSucceed = true,
			Message = $"User is added to {request.Role} role successfully"
		};
	}
}

internal struct Token
{
	public string AccessToken { get; set; }
	public DateTime ExpirationDate { get; set; }
}