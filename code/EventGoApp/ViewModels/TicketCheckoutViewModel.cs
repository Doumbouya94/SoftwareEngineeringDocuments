using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using EventGoApp.Models;
using EventGoApp.Services;

namespace EventGoApp.ViewModels;

[QueryProperty(nameof(EventId), "id")]
public class TicketCheckoutViewModel : INotifyPropertyChanged
{
    private readonly IEventAdapter _eventAdapter;
    private readonly ITicketRepository _ticketRepository;
    private readonly IAuthState _authState;

    private Event? _event;
    private int _quantity = 1;

    public TicketCheckoutViewModel(IEventAdapter eventAdapter,
        ITicketRepository ticketRepository, IAuthState authState)
    {
        _eventAdapter = eventAdapter;
        _ticketRepository = ticketRepository;
        _authState = authState;

        IncrementCommand = new Command(() => { if (Quantity < 10) { Quantity++; } });
        DecrementCommand = new Command(() => { if (Quantity > 1) { Quantity--; } });
        ConfirmCommand = new Command(OnConfirm);
    }

    public string EventId
    {
        set { if (Guid.TryParse(value, out var id))
            {
                LoadAsync(id);
            }
        }
    }

    public Event? Event
    {
        get => _event;
        set
        {
            _event = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(Title));
            OnPropertyChanged(nameof(ImageSource));
            OnPropertyChanged(nameof(Venue));
            OnPropertyChanged(nameof(Address));
            OnPropertyChanged(nameof(FormattedDate));
            OnPropertyChanged(nameof(UnitPrice));
            OnPropertyChanged(nameof(IsFree));
            OnPropertyChanged(nameof(FormattedUnitPrice));
            RefreshPricing();
        }
    }

    public int Quantity
    {
        get => _quantity;
        set { _quantity = value; OnPropertyChanged(); RefreshPricing(); }
    }

    public string Title => _event?.Title ?? string.Empty;
    public string ImageSource => _event?.ImageSource ?? string.Empty;
    public string Venue => _event?.Venue ?? string.Empty;
    public string Address => _event?.Address ?? string.Empty;
    public string FormattedDate => _event?.Date.ToString("dddd d MMMM yyyy — HH:mm",
        new System.Globalization.CultureInfo("fr-CA")) ?? string.Empty;
    public double UnitPrice => _event?.Price ?? 0;
    public bool IsFree => UnitPrice == 0;

    public double Subtotal => UnitPrice * Quantity;
    public double TPS => Subtotal * 0.05;
    public double TVQ => Subtotal * 0.09975;
    public double Total => Subtotal + TPS + TVQ;

    public string FormattedUnitPrice => IsFree ? "Gratuit" : $"{UnitPrice:0.##} $";
    public string FormattedSubtotal => IsFree ? "0,00 $" : $"{Subtotal:0.00} $";
    public string FormattedTPS => IsFree ? "0,00 $" : $"{TPS:0.00} $";
    public string FormattedTVQ => IsFree ? "0,00 $" : $"{TVQ:0.00} $";
    public string FormattedTotal => IsFree ? "Gratuit" : $"{Total:0.00} $";

    public ICommand IncrementCommand { get; }
    public ICommand DecrementCommand { get; }
    public ICommand ConfirmCommand { get; }

    private async void LoadAsync(Guid id)
    {
        Event = await _eventAdapter.GetByIdAsync(id);
    }

    private void RefreshPricing()
    {
        OnPropertyChanged(nameof(Subtotal));
        OnPropertyChanged(nameof(TPS));
        OnPropertyChanged(nameof(TVQ));
        OnPropertyChanged(nameof(Total));
        OnPropertyChanged(nameof(FormattedSubtotal));
        OnPropertyChanged(nameof(FormattedTPS));
        OnPropertyChanged(nameof(FormattedTVQ));
        OnPropertyChanged(nameof(FormattedTotal));
    }

    private async void OnConfirm()
    {
        if (_event is null)
        {
            return;
        }

        var ticket = new Ticket
        {
            Id = Guid.NewGuid(),
            UserId = _authState.CurrentUser?.Id ?? AuthStateService.GuestId,
            EventId = _event.Id,
            EventTitle = _event.Title,
            EventImageSource = _event.ImageSource,
            EventVenue = _event.Venue,
            EventAddress = _event.Address,
            EventDate = _event.Date,
            Quantity = Quantity,
            UnitPrice = _event.Price,
            PurchasedAt = DateTime.UtcNow
        };

        await _ticketRepository.AddAsync(ticket);
        await Shell.Current.GoToAsync("//tickets");
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
