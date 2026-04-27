using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using EventGoApp.Models;
using EventGoApp.Services;

namespace EventGoApp.ViewModels;

[QueryProperty(nameof(EventId), "id")]
public class EditEventViewModel : INotifyPropertyChanged
{
    private readonly IEventAdapter _eventAdapter;
    private Event? _originalEvent;

    private string _eventId = string.Empty;
    private string _title = string.Empty;
    private string _description = string.Empty;
    private DateTime _date = DateTime.Now.Date;
    private TimeSpan _time = DateTime.Now.TimeOfDay;
    private string _venue = string.Empty;
    private string _address = string.Empty;
    private decimal _price;
    private string _imageSource = string.Empty;
    private EventCategory _category = EventCategory.Concerts;

    public EditEventViewModel(IEventAdapter eventAdapter)
    {
        _eventAdapter = eventAdapter;
        SaveCommand = new Command(OnSave, CanSave);
        CancelCommand = new Command(OnCancel);
        Categories = Enum.GetValues(typeof(EventCategory)).Cast<EventCategory>().ToList();
    }

    public string EventId
    {
        get => _eventId;
        set
        {
            _eventId = value;
            if (Guid.TryParse(value, out var guid))
                LoadEventAsync(guid);
        }
    }

    public string Title
    {
        get => _title;
        set { _title = value; OnPropertyChanged(); ((Command)SaveCommand).ChangeCanExecute(); }
    }

    public string Description
    {
        get => _description;
        set { _description = value; OnPropertyChanged(); }
    }

    public DateTime Date
    {
        get => _date;
        set { _date = value; OnPropertyChanged(); }
    }

    public TimeSpan Time
    {
        get => _time;
        set { _time = value; OnPropertyChanged(); }
    }

    public string Venue
    {
        get => _venue;
        set { _venue = value; OnPropertyChanged(); ((Command)SaveCommand).ChangeCanExecute(); }
    }

    public string Address
    {
        get => _address;
        set { _address = value; OnPropertyChanged(); }
    }

    public decimal Price
    {
        get => _price;
        set { _price = value; OnPropertyChanged(); }
    }

    public string ImageSource
    {
        get => _imageSource;
        set { _imageSource = value; OnPropertyChanged(); }
    }

    public EventCategory Category
    {
        get => _category;
        set { _category = value; OnPropertyChanged(); }
    }

    public List<EventCategory> Categories { get; }

    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    private bool CanSave() => !string.IsNullOrWhiteSpace(Title) && !string.IsNullOrWhiteSpace(Venue);

    private async void LoadEventAsync(Guid id)
    {
        _originalEvent = await _eventAdapter.GetByIdAsync(id);
        if (_originalEvent is null) return;

        Title = _originalEvent.Title;
        Description = _originalEvent.Description;
        Date = _originalEvent.Date.Date;
        Time = _originalEvent.Date.TimeOfDay;
        Venue = _originalEvent.Venue;
        Address = _originalEvent.Address;
        Price = (decimal)_originalEvent.Price;
        ImageSource = _originalEvent.ImageSource;
        Category = _originalEvent.Category;
    }

    private async void OnSave()
    {
        if (_originalEvent is null) return;

        _originalEvent.Title = Title;
        _originalEvent.Description = Description;
        _originalEvent.Date = Date.Date + Time;
        _originalEvent.Venue = Venue;
        _originalEvent.Address = Address;
        _originalEvent.Price = (double)Price;
        _originalEvent.ImageSource = string.IsNullOrWhiteSpace(ImageSource)
            ? "https://images.unsplash.com/photo-1501281668745-f7f57925c3b4?q=80&w=1000"
            : ImageSource;
        _originalEvent.Category = Category;

        await _eventAdapter.UpdateAsync(_originalEvent);
        await Shell.Current.GoToAsync("..");
    }

    private async void OnCancel()
    {
        await Shell.Current.GoToAsync("..");
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
