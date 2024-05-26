using Azure.Core;
using GemNote.API.Infrastructure.DataContext;
using GemNote.API.Models;
using GemNote.API.Services.Contracts;
using GemNote.API.StaticDetails;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GemNote.API.Services.Implementations;

public class Seeder(GemNoteDbContext dbContext, RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
	: ISeeder
{
	public async Task SeedRolesAsync()
	{
		if (!await dbContext.Database.CanConnectAsync())
			return;

		if (await dbContext.Roles.AnyAsync())
			return;

		var roles = new List<string>
		{
			UserRoles.Admin, UserRoles.User
		};
		foreach (var role in roles)
		{
			await roleManager.CreateAsync(new IdentityRole(role));
		}
	}

	public async Task SeedUsersAsync()
	{
		if (!await dbContext.Database.CanConnectAsync())
			return;

		if (await dbContext.Users.AnyAsync())
			return;

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
		await userManager.CreateAsync(admin, adminPassword);
		await userManager.AddToRoleAsync(admin, UserRoles.Admin);

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
		await userManager.CreateAsync(user, userPassword);
		await userManager.AddToRoleAsync(user, UserRoles.User);
	}

	public async Task SeedCategoriesAsync()
	{
		if (!await dbContext.Database.CanConnectAsync())
			return;

		if (await dbContext.Categories.AnyAsync())
			return;

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
	}

	public async Task SeedNotebookAsync()
	{
		if (!await dbContext.Database.CanConnectAsync())
			return;

		if (await dbContext.Notebooks.AnyAsync())
			return;

		var user = await userManager.FindByEmailAsync("user1@example.com");

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
	}
}