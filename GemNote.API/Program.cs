using GemNote.API.Extensions;
using GemNote.API.Services.Contracts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddServices();

// Add repositories to the container.
builder.Services.AddRepositories();

// Add DbContext
builder.Services.AddIdentity(builder.Configuration);

// Add Authentication and JWT Bearer
builder.Services.AddJwtBearer(builder.Configuration);

builder.Services.AddSwagger();

builder.Services.AddCorsConfig();

var app = builder.Build();

#region Seeding data

var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<ISeeder>();

await seeder.SeedRolesAsync();
await seeder.SeedUsersAsync();
await seeder.SeedCategoriesAsync();
await seeder.SeedNotebookAsync();

#endregion

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll"); 

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();