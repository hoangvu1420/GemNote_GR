using GemNote.Web.Components.NotebookComponents;
using GemNote.Web.ViewModels.NotebookViewModels;
using Microsoft.FluentUI.AspNetCore.Components;
using Newtonsoft.Json;
using System.Net;
using GemNote.Web.Services.Contracts;
using GemNote.Web.States;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace GemNote.Web.Pages;

public partial class Resources
{
	// Inject services
	[Inject] private IDialogService DialogService { get; set; } = null!;
	[Inject] private IToastService ToastService { get; set; } = null!;
	[Inject] private IJSRuntime JsRuntime { get; set; } = null!;
	[Inject] private ToastMessageState ToastMessageState { get; set; } = default!;

	[Inject] private INotebookService NotebookService { get; set; } = null!;
	[Inject] private UserState UserState { get; set; } = null!;
	[Inject] private NavigationManager NavigationManager { get; set; } = null!;

	private IEnumerable<NotebookVm?> Notebooks { get; set; } = new List<NotebookVm>();
	private bool _isLoading = true;
	private string _message = string.Empty;

	protected override async Task OnInitializedAsync()
	{
		if (string.IsNullOrEmpty(UserState.UserId))
		{
			NavigationManager.NavigateTo("/unauthorized");
			return;
		}

		var message = ToastMessageState.PopMessage();
		if (message is not null)
		{
			ToastService.ShowSuccess(message);
		}

		var userId = UserState.UserId;
		var (response, statusCode) = await NotebookService.GetNotebooksByUserIdAsync(userId!);

		switch (statusCode)
		{
			case HttpStatusCode.Unauthorized:
				var dialog401 = await DialogService.ShowErrorAsync("You are not authorized to access this page.");
				await dialog401.Result;
				NavigationManager.NavigateTo("/");
				break;
			case HttpStatusCode.Forbidden:
				var dialog403 = await DialogService.ShowErrorAsync("You are not allowed to access this page.");
				await dialog403.Result;
				NavigationManager.NavigateTo("/");
				break;
		}

		// Update UI based on response
		if (response.IsSucceed)
		{
			Notebooks = JsonConvert.DeserializeObject<List<NotebookVm>>(response.Data!.ToString()!) ?? [];
			_message = $"You have {Notebooks.Count()} notebooks.";
		}
		else if (statusCode == HttpStatusCode.NotFound)
		{
			_message = "You don't have any notebooks yet. Create one now! :)";
		}

		_isLoading = false;
	}

	private async Task OpenCreateNotebookDialogAsync()
	{
		var notebook = new CreateNotebookVm();

		var dialog = await DialogService.ShowDialogAsync<CreateNotebookComp>(notebook, new DialogParameters()
		{
			Title = "New notebook",
			OnDialogResult = DialogService.CreateDialogCallback(this, HandleCreateDialog),
			Width = "500px",
			Height = "fit-content",
			TrapFocus = true,
			Modal = true
		});
		await dialog.Result;
	}

	private async Task HandleCreateDialog(DialogResult result)
	{
		if (result.Cancelled)
		{
			ToastService.ShowError("Dialog cancelled");
			return;
		}

		if (result.Data is not null)
		{
			CreateNotebookVm createNotebookVm = (result.Data as CreateNotebookVm)!;
			createNotebookVm.AppUserId = UserState.UserId!;

			var (response, statusCode) = await NotebookService.CreateNotebookAsync(createNotebookVm);

			switch (statusCode)
			{
				case HttpStatusCode.Unauthorized:
					var dialog401 = await DialogService.ShowErrorAsync("You are not authorized to access this page.");
					await dialog401.Result;
					NavigationManager.NavigateTo("/");
					break;
				case HttpStatusCode.Forbidden:
					var dialog403 = await DialogService.ShowErrorAsync("You are not allowed to access this page.");
					await dialog403.Result;
					NavigationManager.NavigateTo("/");
					break;
			}

			if (response.IsSucceed)
			{
				Notebooks = Notebooks.Append(JsonConvert.DeserializeObject<NotebookVm>(response.Data!.ToString()!));
				ToastService.ShowSuccess("Notebook created successfully");
			}
			else
			{
				ToastService.ShowError("Failed to create notebook");
			}
		}
	}

	private async Task HandleEditNotebook(NotebookVm notebook)
	{
		var input = new CreateNotebookVm
		{
			Name = notebook.Name,
			CategoryName = notebook.Category,
			Description = notebook.Description
		};
		var dialog = await DialogService.ShowDialogAsync<CreateNotebookComp>(input, new DialogParameters()
		{
			Title = "Edit notebook",
			Width = "500px",
			Height = "fit-content",
			TrapFocus = true,
			Modal = true
		});
		var result = await dialog.Result;

		if (result is { Cancelled: false, Data: not null })
		{
			var createNotebookVm = (result.Data as CreateNotebookVm)!;
			var updateNotebookVm = new UpdateNotebookVm
			{
				Id = notebook.Id,
				Name = createNotebookVm.Name,
				CategoryName = createNotebookVm.CategoryName,
				Description = createNotebookVm.Description
			};

			var (response, statusCode) = await NotebookService.UpdateNotebookAsync(updateNotebookVm);

			switch (statusCode)
			{
				case HttpStatusCode.Unauthorized:
					var dialog401 = await DialogService.ShowErrorAsync("You are not authorized to access this page.");
					await dialog401.Result;
					NavigationManager.NavigateTo("/");
					break;
				case HttpStatusCode.Forbidden:
					var dialog403 = await DialogService.ShowErrorAsync("You are not allowed to access this page.");
					await dialog403.Result;
					NavigationManager.NavigateTo("/");
					break;
			}

			if (response.IsSucceed)
			{
				var index = Notebooks.ToList().FindIndex(n => n!.Id == notebook.Id);
				var updatedNotebook = JsonConvert.DeserializeObject<NotebookVm>(response.Data!.ToString()!);

				Notebooks = Notebooks.Select((n, i) => i == index ? updatedNotebook : n).ToList();
				ToastService.ShowSuccess("Notebook updated successfully");
			}
			else
			{
				ToastService.ShowError("Failed to update notebook");
			}
		}
	}

	private async Task HandleDeleteNotebook(NotebookVm notebook)
	{
		var dialog = await DialogService.ShowConfirmationAsync("Are you sure you want to delete this notebook");
		var result = await dialog.Result;

		if (!result.Cancelled)
		{
			var (response, statusCode) = await NotebookService.DeleteNotebookAsync(notebook.Id);

			switch (statusCode)
			{
				case HttpStatusCode.Unauthorized:
					var dialog401 = await DialogService.ShowErrorAsync("You are not authorized to access this page.");
					await dialog401.Result;
					NavigationManager.NavigateTo("/");
					break;
				case HttpStatusCode.Forbidden:
					var dialog403 = await DialogService.ShowErrorAsync("You are not allowed to access this page.");
					await dialog403.Result;
					NavigationManager.NavigateTo("/");
					break;
			}

			if (response.IsSucceed)
			{
				Notebooks = Notebooks.Where(n => n!.Id != notebook.Id).ToList();
				ToastService.ShowSuccess("Notebook deleted successfully");
			}
			else
			{
				ToastService.ShowError("Failed to delete notebook");
			}
		}
	}
}
