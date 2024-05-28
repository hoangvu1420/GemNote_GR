using Microsoft.AspNetCore.Components;

namespace GemNote.Web.ViewModels;

public record Country(string Code, string Name)
{
	public string Flag => $"https://fluentui-blazor.net/_content/FluentUI.Demo.Shared/flags/{Code}.svg";

	public MarkupString HtmlFlagAndName => (MarkupString)$"""
	                                                      		<div style="display: flex; gap: 10px;">
	                                                      			<img src="{Flag}" width: "20px" />
	                                                      			<div>{Name}</div>
	                                                      		</div>
	                                                      """;

	public static IEnumerable<Country> All
	{
		get
		{
			return new Country[]
			{
				new("us", "United States"),
				new("ca", "Canada"),
				new("mx", "Mexico"),
				new("br", "Brazil"),
				new("gb", "United Kingdom"),
				new("de", "Germany"),
				new("fr", "France"),
				new("es", "Spain"),
				new("it", "Italy"),
				new("ru", "Russia"),
				new("cn", "China"),
				new("jp", "Japan"),
				new("in", "India"),
				new("au", "Australia"),
				new("za", "South Africa"),
				new("sa", "Saudi Arabia"),
				new("ae", "United Arab Emirates"),
				new("eg", "Egypt"),
				new("ng", "Nigeria"),
				new("ke", "Kenya"),
				new("ar", "Argentina"),
				new("cl", "Chile"),
				new("co", "Colombia"),
				new("pe", "Peru"),
				new("ve", "Venezuela"),
				new("id", "Indonesia"),
				new("ph", "Philippines"),
				new("vn", "Vietnam"),
				new("th", "Thailand"),
				new("my", "Malaysia"),
				new("sg", "Singapore"),
				new("kr", "South Korea"),
				new("pk", "Pakistan"),
				new("tr", "Turkey"),
				new("ir", "Iran"),
				new("iq", "Iraq")
			}.OrderBy(c => c.Name);
		}
	}
}