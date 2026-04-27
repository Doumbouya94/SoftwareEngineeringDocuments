using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using EventGoApp.Models;
using EventGoApp.Services;

namespace EventGoApp.ViewModels;

public class MyEventsViewModel : INotifyPropertyChanged
{
    private readonly IEventAdapter _eventAdapter;
    private readonly IAuthState _authState;
    private bool _isBusy;
    private bool _isEmpty;

    public MyEventsViewModel(IEventAdapter eventAdapter, IAuthState authState)
    {
        _eventAdapter = eventAdapter;
        _authState = authState;
        Events = new ObservableCollection<Event>();
        EditEventCommand = new Command<Event>(OnEditEvent);
        DeleteEventCommand = new Command<Event>(OnDeleteEvent);
        GoToCreateEventCommand = new Command(async () => await Shell.Current.GoToAsync("createevent"));
    }

    public ObservableCollection<Event> Events { get; }

    public bool IsBusy
    {
        get => _isBusy;
        set { _isBusy = value; OnPropertyChanged(); }
    }

    public bool IsEmpty
    {
        get => _isEmpty;
        set { _isEmpty = value; OnPropertyChanged(); }
    }

    public ICommand EditEventCommand { get; }
    public ICommand DeleteEventCommand { get; }
    public ICommand GoToCreateEventCommand { get; }

    public async Task LoadAsync()
    {
        IsBusy = true;
        var organizerId = _authState.CurrentUser?.Id ?? AuthStateService.GuestId;
        var events = await _eventAdapter.GetByOrganizerAsync(organizerId);
        Events.Clear();
        foreach (var e in events)
            Events.Add(e);
        IsEmpty = Events.Count == 0;
        IsBusy = false;
    }

    private async void OnEditEvent(Event ev)
    {
        await Shell.Current.GoToAsync($"editevent?id={ev.Id}");
    }

    private async void OnDeleteEvent(Event ev)
    {
        bool confirmed = await Shell.Current.DisplayAlert(
            "Supprimer l'événement",
            $"Voulez-vous vraiment supprimer « {ev.Title} » ?",
            "Supprimer", "Annuler");

        if (!confirmed) return;

        await _eventAdapter.DeleteAsync(ev.Id);
        Events.Remove(ev);
        IsEmpty = Events.Count == 0;
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
