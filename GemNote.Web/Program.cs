using Blazored.LocalStorage;
using GemNote.Web.Authentication;
using GemNote.Web.Services.Contracts;
using GemNote.Web.Services.Implementations;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.FluentUI.AspNetCore.Components;

namespace GemNote.Web;

public class Program
{
	public static async Task Main(string[] args)
	{
		var builder = WebAssemblyHostBuilder.CreateDefault(args);
		builder.RootComponents.Add<App>("#app");
		builder.RootComponents.Add<HeadOutlet>("head::after");

		builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
		builder.Services.AddFluentUIComponents();

		builder.Services.AddHttpClient("ServerApi", client => client.BaseAddress = new Uri("https://localhost:7214/"));

		builder.Services.AddBlazoredLocalStorage();
		builder.Services.AddAuthorizationCore();
		builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

		// Add services to the container.
		builder.Services.AddScoped<IAuthService, AuthService>();

		await builder.Build().RunAsync();
	}
}