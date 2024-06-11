using GemNote.Web.Components.SectionComponents;
using GemNote.Web.ViewModels.NotebookViewModels;
using GemNote.Web.ViewModels.SectionViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Newtonsoft.Json;
using System.Net;
using GemNote.Web.Services.Contracts;
using GemNote.Web.States;
using Microsoft.JSInterop;

namespace GemNote.Web.Components.NotebookComponents;

public partial class NotebookComp
{
	[Inject] public IToastService ToastService { get; set; } = null!;
	[Inject] public IDialogService DialogService { get; set; } = null!;
	[Inject] public NavigationManager NavigationManager { get; set; } = null!;
	[Inject] public ISectionService SectionService { get; set; } = null!;

	[Parameter] public NotebookVm Notebook { get; set; } = null!;
	[Parameter] public EventCallback<NotebookVm> OnEdit { get; set; }
	[Parameter] public EventCallback<NotebookVm> OnDelete { get; set; }

	public IEnumerable<SectionVm?> Sections { get; set; } = new List<SectionVm?>();
	private string _message = string.Empty;

	private async Task OnExpanded(bool expanded)
	{
		if (expanded)
		{
			// Load sections
			var (response, statusCode) = await SectionService.GetSectionsByNotebookIdAsync(Notebook.Id);

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
				Sections = JsonConvert.DeserializeObject<List<SectionVm>>(response.Data!.ToString()!) ?? [];
				_message = $"This notebook has {Sections.Count()} sections.";
			}
			else
			{
				_message = "This notebook has no section.";
			}
		}
	}

	private void HandleEdit()
	{
		if (OnEdit.HasDelegate)
		{
			OnEdit.InvokeAsync(Notebook);
		}
	}

	private async void HandleDelete()
	{
		if (OnDelete.HasDelegate)
		{
			await OnDelete.InvokeAsync(Notebook);
		}
	}

	private async Task OpenCreateSectionDialogAsync()
	{
		var section = new CreateSectionVm();

		var dialog = await DialogService.ShowDialogAsync<CreateSectionComp>(section, new DialogParameters()
		{
			Title = $"New section in {Notebook.Name}",
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
			CreateSectionVm createSectionVm = (result.Data as CreateSectionVm)!;
			createSectionVm.NotebookId = Notebook.Id;

			var (response, statusCode) = await SectionService.CreateSectionAsync(createSectionVm);

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
				Sections = Sections.Append(JsonConvert.DeserializeObject<SectionVm>(response.Data!.ToString()!));
				ToastService.ShowSuccess("Section created successfully");
			}
			else
			{
				ToastService.ShowError("Failed to create section");
			}
		}
	}

	private async Task HandleEditSection(SectionVm section)
	{
		var input = new CreateSectionVm()
		{
			Name = section.Name,
			Description = section.Description
		};
		var dialog = await DialogService.ShowDialogAsync<CreateSectionComp>(input, new DialogParameters()
		{
			Title = "Edit section",
			Width = "500px",
			Height = "fit-content",
			TrapFocus = true,
			Modal = true
		});
		var result = await dialog.Result;

		if (result is { Cancelled: false, Data: not null })
		{
			var createSectionVm = (result.Data as CreateSectionVm)!;
			var updateSectionVm = new UpdateSectionVm
			{
				Id = section.Id,
				Name = createSectionVm.Name,
				Description = createSectionVm.Description
			};

			var (response, statusCode) = await SectionService.UpdateSectionAsync(updateSectionVm);

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
				var index = Sections.ToList().FindIndex(n => n!.Id == section.Id);
				var updatedSection = JsonConvert.DeserializeObject<SectionVm>(response.Data!.ToString()!);

				Sections = Sections.Select((n, i) => i == index ? updatedSection : n).ToList();
				ToastService.ShowSuccess("Section updated successfully");
			}
			else
			{
				ToastService.ShowError("Failed to update section");
			}
		}
	}

	private async Task HandleDeleteSection(SectionVm section)
	{
		var dialog = await DialogService.ShowConfirmationAsync("Are you sure you want to delete this section");
		var result = await dialog.Result;

		if (!result.Cancelled)
		{
			var (response, statusCode) = await SectionService.DeleteSectionAsync(section.Id);

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
				Sections = Sections.Where(n => n!.Id != section.Id).ToList();
				_message = $"This notebook has {Sections.Count()} sections.";
				ToastService.ShowSuccess("Section deleted successfully");
			}
			else
			{
				ToastService.ShowError("Failed to delete section");
			}
		}
	}
}