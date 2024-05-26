namespace GemNote.API.StaticDetails;

public static class PictureStock
{
	public static List<string> Pictures =
	[
		"https://api.dicebear.com/8.x/identicon/svg?seed=Cookie",
		"https://api.dicebear.com/8.x/identicon/svg?seed=Bear",
		"https://api.dicebear.com/8.x/identicon/svg?seed=Chester",
		"https://api.dicebear.com/8.x/identicon/svg?seed=Fluffy",
		"https://api.dicebear.com/8.x/identicon/svg?seed=Cali",
		"https://api.dicebear.com/8.x/identicon/svg?seed=Scooter",
		"https://api.dicebear.com/8.x/identicon/svg?seed=Tiger",
		"https://api.dicebear.com/8.x/identicon/svg?seed=Bella",
		"https://api.dicebear.com/8.x/identicon/svg?seed=Baby",
		"https://api.dicebear.com/8.x/identicon/svg?seed=Maggie"
	];

	public static string GetRandomPicture()
	{
		Random random = new();
		var index = random.Next(0, Pictures.Count);
		return Pictures[index];
	}
}