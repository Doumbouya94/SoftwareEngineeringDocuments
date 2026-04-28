using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using EventGoApp.Models;
using EventGoApp.Services;

namespace EventGoApp.ViewModels;

public class TicketsViewModel : INotifyPropertyChanged
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IAuthState _authState;
    private bool _isEmpty;

    public TicketsViewModel(ITicketRepository ticketRepository, IAuthState authState)
    {
        _ticketRepository = ticketRepository;
        _authState = authState;
        Tickets = new ObservableCollection<Ticket>();
    }

    public ObservableCollection<Ticket> Tickets { get; }

    public bool IsEmpty
    {
        get => _isEmpty;
        set { _isEmpty = value; OnPropertyChanged(); }
    }

    public async Task LoadAsync()
    {
        var userId = _authState.CurrentUser?.Id ?? AuthStateService.GuestId;
        var tickets = await _ticketRepository.GetByUserAsync(userId);
        Tickets.Clear();
        foreach (var t in tickets)
            Tickets.Add(t);
        IsEmpty = Tickets.Count == 0;
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
