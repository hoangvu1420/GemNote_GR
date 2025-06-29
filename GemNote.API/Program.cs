using GemNote.API.Extensions;
using GemNote.API.Infrastructure.DataContext;
using GemNote.API.Services.Contracts;
using Microsoft.Net.Http.Headers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

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

app.MapDefaultEndpoints();

var scope = app.Services.CreateScope();

// Configure the HTTP request pipeline.
app.UseSwagger();
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI();
    app.UseCors("AllowClientWebLocal");

    // Apply database migrations
    var dbContext = scope.ServiceProvider.GetRequiredService<GemNoteDbContext>();
    dbContext.Database.Migrate();
}
else if (app.Environment.IsProduction())
{
	app.UseCors("AllowClientWeb");
}

#region Seeding data

var seeder = scope.ServiceProvider.GetRequiredService<ISeeder>();

await seeder.SeedRolesAsync();
await seeder.SeedUsersAsync();
await seeder.SeedCategoriesAsync();
await seeder.SeedNotebookAsync();

#endregion

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();