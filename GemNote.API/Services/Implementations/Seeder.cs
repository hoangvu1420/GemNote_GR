using Azure.Core;
using GemNote.API.Infrastructure.DataContext;
using GemNote.API.Models;
using GemNote.API.Services.Contracts;
using GemNote.API.StaticDetails;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GemNote.API.Services.Implementations;

public class Seeder(GemNoteDbContext dbContext, RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager, ILogger<Seeder> logger)
	: ISeeder
{
	public async Task SeedRolesAsync()
	{
		logger.LogInformation("Attempting to seed roles...");
		if (!await dbContext.Database.CanConnectAsync())
		{
			logger.LogWarning("Database connection not available. Skipping role seeding.");
			return;
		}

		if (await dbContext.Roles.AnyAsync())
		{
			logger.LogInformation("Roles already exist. Skipping role seeding.");
			return;
		}

		var roles = new List<string>
		{
			UserRoles.Admin, UserRoles.User
		};
		foreach (var role in roles)
		{
			var result = await roleManager.CreateAsync(new IdentityRole(role));
			if (result.Succeeded)
			{
				logger.LogInformation("Role '{RoleName}' created successfully.", role);
			}
			else
			{
				logger.LogError("Failed to create role '{RoleName}': {Errors}", role, string.Join(", ", result.Errors.Select(e => e.Description)));
			}
		}
		logger.LogInformation("Role seeding completed.");
	}

	public async Task SeedUsersAsync()
	{
		logger.LogInformation("Attempting to seed users...");
		if (!await dbContext.Database.CanConnectAsync())
		{
			logger.LogWarning("Database connection not available. Skipping user seeding.");
			return;
		}

		if (await dbContext.Users.AnyAsync())
		{
			logger.LogInformation("Users already exist. Skipping user seeding.");
			return;
		}

		var admin = new AppUser
		{
			UserName = "admin1@example.com",
			Email = "admin1@example.com",
			FirstName = "Admin",
			LastName = "1",
			Language = "en",
			AvatarUrl = "https://api.dicebear.com/8.x/identicon/svg?seed=Tiger"
		};

		const string adminPassword = "admin111";
		var adminResult = await userManager.CreateAsync(admin, adminPassword);
		if (adminResult.Succeeded)
		{
			logger.LogInformation("Admin user '{UserName}' created successfully.", admin.UserName);
			var addRoleResult = await userManager.AddToRoleAsync(admin, UserRoles.Admin);
			if (addRoleResult.Succeeded)
			{
				logger.LogInformation("Admin user '{UserName}' added to role '{RoleName}' successfully.", admin.UserName, UserRoles.Admin);
			}
			else
			{
				logger.LogError("Failed to add admin user '{UserName}' to role '{RoleName}': {Errors}", admin.UserName, UserRoles.Admin, string.Join(", ", addRoleResult.Errors.Select(e => e.Description)));
			}
		}
		else
		{
			logger.LogError("Failed to create admin user '{UserName}': {Errors}", admin.UserName, string.Join(", ", adminResult.Errors.Select(e => e.Description)));
		}

		var user = new AppUser
		{
			UserName = "user1@example.com",
			Email = "user1@example.com",
			FirstName = "Kim",
			LastName = "Clinton",
			Language = "en",
			AvatarUrl = "https://api.dicebear.com/8.x/identicon/svg?seed=Fluffy"
		};

		const string userPassword = "user1111";
		var userResult = await userManager.CreateAsync(user, userPassword);
		if (userResult.Succeeded)
		{
			logger.LogInformation("Regular user '{UserName}' created successfully.", user.UserName);
			var addRoleResult = await userManager.AddToRoleAsync(user, UserRoles.User);
			if (addRoleResult.Succeeded)
			{
				logger.LogInformation("Regular user '{UserName}' added to role '{RoleName}' successfully.", user.UserName, UserRoles.User);
			}
			else
			{
				logger.LogError("Failed to add regular user '{UserName}' to role '{RoleName}': {Errors}", user.UserName, UserRoles.User, string.Join(", ", addRoleResult.Errors.Select(e => e.Description)));
			}
		}
		else
		{
			logger.LogError("Failed to create regular user '{UserName}': {Errors}", user.UserName, string.Join(", ", userResult.Errors.Select(e => e.Description)));
		}
		logger.LogInformation("User seeding completed.");
	}

	public async Task SeedCategoriesAsync()
	{
		logger.LogInformation("Attempting to seed categories...");
		if (!await dbContext.Database.CanConnectAsync())
		{
			logger.LogWarning("Database connection not available. Skipping category seeding.");
			return;
		}

		if (await dbContext.Categories.AnyAsync())
		{
			logger.LogInformation("Categories already exist. Skipping category seeding.");
			return;
		}

		var categories = new List<Category>
		{
			new()
			{
				Name = "Language", 
				CreatedAt = DateTime.Now, 
				UpdatedAt = DateTime.Now
			},
			new()
			{
				Name = "Computer Science", 
				CreatedAt = DateTime.Now, 
				UpdatedAt = DateTime.Now
			}
		};

		await dbContext.Categories.AddRangeAsync(categories);
		await dbContext.SaveChangesAsync();
		logger.LogInformation("Categories seeded successfully.");
	}

	public async Task SeedNotebookAsync()
	{
		logger.LogInformation("Attempting to seed notebooks...");
		if (!await dbContext.Database.CanConnectAsync())
		{
			logger.LogWarning("Database connection not available. Skipping notebook seeding.");
			return;
		}

		if (await dbContext.Notebooks.AnyAsync())
		{
			logger.LogInformation("Notebooks already exist. Skipping notebook seeding.");
			return;
		}

		var user = await userManager.FindByEmailAsync("user1@example.com");
		if (user == null)
		{
			logger.LogError("User 'user1@example.com' not found. Cannot seed notebooks.");
			return;
		}

		var notebooks = new List<Notebook>
		{
			new()
			{
				Name = "Japanese N3", 
				AppUserId = user.Id, 
				CategoryId = 1, 
				CreatedAt = DateTime.Now,
				UpdatedAt = DateTime.Now
			},
			new()
			{
				Name = "C#", 
				AppUserId = user.Id, 
				CategoryId = 2, 
				CreatedAt = DateTime.Now, 
				UpdatedAt = DateTime.Now
			}
		};

		await dbContext.Notebooks.AddRangeAsync(notebooks);
		await dbContext.SaveChangesAsync();
		logger.LogInformation("Notebooks seeded successfully.");
	}
}
