using GemNote.Web.Services.Contracts;
using GemNote.Web.States;
using GemNote.Web.ViewModels.FlashcardViewModels;
using GemNote.Web.ViewModels.UnitViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Newtonsoft.Json;
using System.Net;
using Microsoft.JSInterop;
using GemNote.Web.Components.UnitComponents;
using GemNote.Web.Services.Implementations;

namespace GemNote.Web.Pages;

public partial class UnitPage : IAsyncDisposable
{
	[Inject] private IDialogService DialogService { get; set; } = default!;
	[Inject] private IToastService ToastService { get; set; } = default!;
	[Inject] private ToastMessageState ToastMessageState { get; set; } = default!;
	[Inject] private IKeyCodeService KeyCodeService { get; set; } = default!;
	[Inject] private IJSRuntime JsRuntime { get; set; } = default!;

	[Inject] private IUnitService UnitService { get; set; } = default!;
	[Inject] private IFlashcardService FlashcardService { get; set; } = default!;
	[Inject] private NavigationManager NavigationManager { get; set; } = default!;
	[Inject] private UserState UserState { get; set; } = default!;

	[Parameter] public string UnitId { get; set; } = string.Empty;

	private bool _isLoading = true;
	private string _description = string.Empty;
	private string _message = string.Empty;

	FluentHorizontalScroll _horizontalScroll = default!;

	private DetailedUnitVm _unit = new();
	private Dictionary<int, bool> _isCardFlipped = new();
	private int _currentCardIndex = 1;

	protected override async Task OnInitializedAsync()
	{
		if (string.IsNullOrEmpty(UserState.UserId))
		{
			NavigationManager.NavigateTo("/unauthorized");
			return;
		}

		KeyCodeService.RegisterListener(OnKeyDownAsync);

		var unitId = int.Parse(UnitId);

		var (response, statusCode) = await UnitService.GetUnitAsync(unitId);

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
			_unit = JsonConvert.DeserializeObject<DetailedUnitVm>(response.Data!.ToString()!)!;
			_description = _unit.Description;
			_message = _unit.CardQty > 0
				? $"This unit has {_unit.CardQty} flashcards."
				: "This unit has no flashcards.";
			_isCardFlipped = _unit.Flashcards.ToDictionary(card => card.Id, card => false);

			await JsRuntime.InvokeVoidAsync("logger.object", _unit.Flashcards);
		}
		else if (statusCode == HttpStatusCode.NotFound)
		{
			var dialog404 = await DialogService.ShowErrorAsync("This unit does not exist.");
			await dialog404.Result;
		}

		_isLoading = false;
	}

	private void HandleCardClicked(int cardId)
	{
		if (_isCardFlipped.ContainsKey(cardId))
		{
			_isCardFlipped[cardId] = !_isCardFlipped[cardId];
		}
	}

	private void HandlePreviousCard()
	{
		ScrollToPrevious();
	}

	private void HandleNextCard()
	{
		ScrollToNext();
	}

	private void ScrollToPrevious()
	{
		if (_currentCardIndex > 1)
		{
			_currentCardIndex--;
		}
		_horizontalScroll.ScrollToPrevious();
	}

	private void ScrollToNext()
	{
		if (_currentCardIndex < _unit.CardQty)
		{
			_currentCardIndex++;
		}
		_horizontalScroll.ScrollToNext();
	}

	private async Task<ValueTask> OnKeyDownAsync(FluentKeyCodeEventArgs key)
	{
		await JsRuntime.InvokeVoidAsync("logger.info", key.Value);
		switch (key.KeyCode)
		{
			case 32:
				_isCardFlipped = _isCardFlipped.ToDictionary(card => card.Key, card => false);
				StateHasChanged();
				break;
			case 39:
				ScrollToNext();
				StateHasChanged();
				break;
			case 37:
				ScrollToPrevious();
				StateHasChanged();
				break;
		}

		return ValueTask.CompletedTask;
	}

	private async Task HandleEditUnit()
	{
		var input = new CreateUnitVm()
		{
			Name = _unit.Name,
			Description = _unit.Description
		};
		var dialog = await DialogService.ShowDialogAsync<CreateUnitComp>(input, new DialogParameters()
		{
			Title = "Edit Unit",
			Width = "500px",
			Height = "fit-content",
			TrapFocus = true,
			Modal = true
		});
		var result = await dialog.Result;

		if (result is { Cancelled: false, Data: not null })
		{
			var createUnitVm = (result.Data as CreateUnitVm)!;
			var updateUnitVm = new UpdateUnitVm
			{
				Id = _unit.Id,
				Name = createUnitVm.Name,
				Description = createUnitVm.Description
			};

			var (response, statusCode) = await UnitService.UpdateUnitAsync(updateUnitVm);

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
				var updatedUnit = JsonConvert.DeserializeObject<UnitVm>(response.Data!.ToString()!);
				_unit.Name = updatedUnit!.Name;
				_unit.Description = updatedUnit.Description;
				_description = updatedUnit.Description;

				ToastService.ShowSuccess("Unit updated successfully");
			}
			else
			{
				ToastService.ShowError("Failed to update unit");
			}
		}
	}

	private async void HandleDeleteUnit()
	{
		var dialog = await DialogService.ShowConfirmationAsync("Are you sure you want to delete this unit");
		var result = await dialog.Result;

		if (!result.Cancelled)
		{
			var (response, statusCode) = await UnitService.DeleteUnitAsync(_unit.Id);

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
				ToastMessageState.PushMessage("Unit deleted successfully");

				NavigationManager.NavigateTo("/resources");
			}
			else
			{
				ToastService.ShowError("Failed to delete unit");
			}
		}
	}

	private async Task HandleCreateFlashcard()
	{
		var input = new CreateFlashcardVm()
		{
			UnitId = _unit.Id,
			Front = "null",
			Back = "null"
		};

		var (response, statusCode) = await FlashcardService.CreateFlashcardAsync(input);

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
			var result = JsonConvert.DeserializeObject<FlashcardVm>(response.Data!.ToString()!);
			result!.Front = string.Empty;
			result.Back = string.Empty;
			var flashcardVms = _unit.Flashcards.Append(result);
			_unit.Flashcards = flashcardVms.ToList();
			_isCardFlipped.Add(result.Id, false);
			_message = _unit.Flashcards.Any()
				? $"This unit has {_unit.Flashcards.Count()} flashcards."
				: "This unit has no flashcards.";
			ToastService.ShowSuccess("Flashcard created successfully");
		}
		else
		{
			ToastService.ShowError("Failed to create flashcard");
		}
	}

	private async Task HandleDeleteFlashcard(int flashcardId)
	{
		var dialog = await DialogService.ShowConfirmationAsync("Are you sure you want to delete this flashcard");
		var result = await dialog.Result;

		if (!result.Cancelled)
		{
			var (response, statusCode) = await FlashcardService.DeleteFlashcardAsync(flashcardId);

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
				_unit.Flashcards = _unit.Flashcards.Where(f => f.Id != flashcardId).ToList();
				_message = _unit.Flashcards.Any()
					? $"This unit has {_unit.Flashcards.Count()} flashcards."
					: "This unit has no flashcards.";
				ToastService.ShowSuccess("Flashcard deleted successfully");
			}
			else
			{
				ToastService.ShowError("Failed to delete flashcard");
			}
		}
	}

	private async Task HandleEditFlashcard(UpdateFlashcardVm flashcardVm)
	{
		var (response, statusCode) = await FlashcardService.UpdateFlashcardAsync(flashcardVm);

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
			var index = _unit.Flashcards.ToList().FindIndex(f => f.Id == flashcardVm!.Id);

			if (index >= 0)
			{
				var flashcardsList = _unit.Flashcards.ToList();
				flashcardsList[index].Front = flashcardVm.Front;
				flashcardsList[index].Back = flashcardVm.Back;
				_unit.Flashcards = flashcardsList;
			}

			ToastService.ShowSuccess("Flashcard updated successfully");
		}
		else
		{
			ToastService.ShowError("Failed to update flashcard");
		}
	}

	private async Task OnFinishedReview()
	{
		await InvokeAsync(StateHasChanged);
	}

	public ValueTask DisposeAsync()
	{
		KeyCodeService.UnregisterListener(OnKeyDownAsync);
		return ValueTask.CompletedTask;
	}
}