using GemNote.API.Extensions;

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