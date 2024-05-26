namespace GemNote.API.Services.Contracts;

public interface ISeeder
{
	Task SeedRolesAsync();
	Task SeedUsersAsync();
	Task SeedCategoriesAsync();
	Task SeedNotebookAsync();
}