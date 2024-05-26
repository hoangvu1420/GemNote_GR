using Microsoft.AspNetCore.Mvc;

namespace GemNote.API.CustomFilters;

public class ResourceAuthorizeAttribute(Type resourceType)
	: TypeFilterAttribute(typeof(ResourceAuthorizationFilter<>).MakeGenericType(resourceType));