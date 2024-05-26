﻿using System.Text;
using GemNote.API.Infrastructure.DataContext;
using GemNote.API.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace GemNote.API.Extensions;

public static class ServiceCollectionExtensions
{
	public static void AddIdentity(this IServiceCollection services, IConfiguration configuration)
	{
		// Add DbContext
		services.AddDbContext<GemNoteDbContext>(option =>
		{
			option.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
		});

		// Add Identity
		services
			.AddIdentity<AppUser, IdentityRole>()
			.AddEntityFrameworkStores<GemNoteDbContext>()
			.AddDefaultTokenProviders();

		// Configure Identity
		services.Configure<IdentityOptions>(options =>
		{
			// Password settings
			options.Password.RequireDigit = true;
			options.Password.RequiredLength = 8;
			options.Password.RequireNonAlphanumeric = false;
			options.Password.RequireUppercase = false;
			options.Password.RequireLowercase = false;

			// Signin settings
			options.SignIn.RequireConfirmedEmail = false;

			// User settings
			options.User.RequireUniqueEmail = true;
		});
	}

	public static void AddJwtBearer(this IServiceCollection services, IConfiguration configuration)
	{
		// Add Authentication and JWT Bearer
		services.AddAuthentication(options =>
		{
			options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
			options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
		}).AddJwtBearer(options =>
		{
			options.TokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuer = true,
				ValidateAudience = true,
				ValidateLifetime = true,
				ValidateIssuerSigningKey = true,
				ValidIssuer = configuration["Jwt:Issuer"],
				ValidAudience = configuration["Jwt:Audience"],
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
			};
		});
	}

	public static void AddSwagger(this IServiceCollection services)
	{
		services.AddEndpointsApiExplorer();
		services.AddSwaggerGen(options =>
		{
			options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
			{
				Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
				Name = "Authorization",
				In = ParameterLocation.Header,
				Type = SecuritySchemeType.ApiKey,
				Scheme = "Bearer"
			});

			options.AddSecurityRequirement(new OpenApiSecurityRequirement
			{
				{
					new OpenApiSecurityScheme
					{
						Reference = new OpenApiReference
						{
							Type = ReferenceType.SecurityScheme,
							Id = "Bearer"
						}
					},
					Array.Empty<string>()
				}
			});
		});
	}

	public static void AddCorsConfig(this IServiceCollection services)
	{
		services.AddCors(options =>
		{
			options.AddPolicy("AllowAll",
				builder =>
				{
					builder
						.AllowAnyOrigin()
						.AllowAnyMethod()
						.AllowAnyHeader();
				});
		});
	}

	public static void AddServices(this IServiceCollection services)
	{
		services.AddControllers();
		services.AddEndpointsApiExplorer();
	}

	public static void AddRepositories(this IServiceCollection services)
	{

	}
}