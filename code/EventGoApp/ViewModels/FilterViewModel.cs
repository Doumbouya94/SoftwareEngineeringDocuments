using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using EventGoApp.Models;

namespace EventGoApp.ViewModels;

public class FilterViewModel : INotifyPropertyChanged
{
    private string _selectedCity = "Montréal";
    private double _maxPrice = 100;
    private EventCategory? _selectedCategory;

    public List<string> Cities { get; } = new()
    {
        "Montréal", "Québec", "Laval", "Gatineau", "Sherbrooke", "Trois-Rivières"
    };

    public string SelectedCity
    {
        get => _selectedCity;
        set { _selectedCity = value; OnPropertyChanged(); }
    }

    public double MaxPrice
    {
        get => _maxPrice;
        set { _maxPrice = value; OnPropertyChanged(); OnPropertyChanged(nameof(FormattedMaxPrice)); }
    }

    public string FormattedMaxPrice => _maxPrice >= 100 ? "Tous les prix" : $"{_maxPrice:0} $ et moins";

    public EventCategory? SelectedCategory
    {
        get => _selectedCategory;
        set { _selectedCategory = value; OnPropertyChanged(); }
    }

    public ICommand ApplyCommand { get; }
    public ICommand ResetCommand { get; }
    public ICommand BackCommand { get; }

    public FilterViewModel()
    {
        ApplyCommand = new Command(async () => await Shell.Current.GoToAsync(".."));
        BackCommand = new Command(async () => await Shell.Current.GoToAsync(".."));
        ResetCommand = new Command(() =>
        {
            SelectedCity = "Montréal";
            MaxPrice = 100;
            SelectedCategory = null;
        });
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
