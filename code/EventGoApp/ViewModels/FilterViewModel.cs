using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using EventGoApp.Models;
using EventGoApp.Services;

namespace EventGoApp.ViewModels;

/// <summary>
/// Vue-modèle de la page de filtres.
/// Applique les filtres via FilterStateService partagé avec HomeViewModel.
/// </summary>
/// <remarks>
/// Auteur : Aboubacar Sidiki Doumbouya
/// </remarks>
public class FilterViewModel : INotifyPropertyChanged
{
    private readonly FilterStateService _filterState;
    private string? _selectedCity;
    private double _maxPrice = 100;
    private EventCategory? _selectedCategory;

    public FilterViewModel(FilterStateService filterState)
    {
        _filterState = filterState;

        // Charger les filtres actifs existants
        _selectedCity = _filterState.SelectedCity;
        _maxPrice = (double?)_filterState.MaxPrice ?? 100;
        _selectedCategory = _filterState.SelectedCategory;

        ApplyCommand = new Command(async () =>
        {
            // Sauvegarder les filtres dans le service partagé
            _filterState.SelectedCity = _selectedCity;
            _filterState.MaxPrice = _maxPrice >= 100 ? null : (decimal?)_maxPrice;
            _filterState.SelectedCategory = _selectedCategory;

            await Shell.Current.GoToAsync("..");
        });

        BackCommand = new Command(async () =>
            await Shell.Current.GoToAsync(".."));

        ResetCommand = new Command(() =>
        {
            SelectedCity = null;
            MaxPrice = 100;
            SelectedCategory = null;
            _filterState.Reset();
        });
    }

    public List<string> Cities { get; } = new()
    {
        "Montréal", "Québec", "Laval", "Gatineau", "Sherbrooke", "Chambly", "Trois-Rivières"
    };

    public string? SelectedCity
    {
        get => _selectedCity;
        set { _selectedCity = value; OnPropertyChanged(); }
    }

    public double MaxPrice
    {
        get => _maxPrice;
        set
        {
            _maxPrice = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(FormattedMaxPrice));
        }
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

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}