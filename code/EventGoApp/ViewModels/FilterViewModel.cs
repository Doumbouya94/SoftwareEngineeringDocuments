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
    private string? _selectedDateFilter = "Toutes les dates";
    private bool _isFreeOnly;

    public FilterViewModel(FilterStateService filterState)
    {
        _filterState = filterState;

        // Charger les filtres actifs existants
        _selectedCity = _filterState.SelectedCity;
        _maxPrice = (double?)_filterState.MaxPrice ?? 100;
        _selectedDateFilter = _filterState.SelectedDateFilter ?? "Toutes les dates";
        _isFreeOnly = _filterState.IsFreeOnly;

        ApplyCommand = new Command(async () =>
        {
            // Sauvegarder les filtres dans le service partagé
            _filterState.SelectedCity = _selectedCity;
            _filterState.MaxPrice = _maxPrice >= 100 ? null : (decimal?)_maxPrice;
            _filterState.SelectedDateFilter = _selectedDateFilter;
            _filterState.IsFreeOnly = _isFreeOnly;

            await Shell.Current.GoToAsync("..");
        });

        BackCommand = new Command(async () =>
            await Shell.Current.GoToAsync(".."));

        ResetCommand = new Command(() =>
        {
            SelectedCity = null;
            MaxPrice = 100;
            SelectedDateFilter = "Toutes les dates";
            IsFreeOnly = false;
            _filterState.Reset();
        });
    }

    public List<string> Cities { get; } = new()
    {
        "Montréal", "Québec", "Laval", "Gatineau", "Sherbrooke", "Chambly", "Trois-Rivières"
    };

    public List<string> DateFilters { get; } = new()
    {
        "Toutes les dates", "Aujourd'hui", "Demain", "Ce week-end", "Cette semaine", "Ce mois-ci"
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

    public string? SelectedDateFilter
    {
        get => _selectedDateFilter;
        set { _selectedDateFilter = value; OnPropertyChanged(); }
    }

    public bool IsFreeOnly
    {
        get => _isFreeOnly;
        set { _isFreeOnly = value; OnPropertyChanged(); }
    }

    public ICommand ApplyCommand { get; }
    public ICommand ResetCommand { get; }
    public ICommand BackCommand { get; }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}