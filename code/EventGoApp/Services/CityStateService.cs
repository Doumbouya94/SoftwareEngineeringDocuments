using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EventGoApp.Services;

/// <summary>
/// Service partagé qui conserve la ville active sélectionnée par l'utilisateur.
/// Permet la synchronisation entre HomePage, CityPickerPage et ProfilePage.
/// </summary>
/// <remarks>
/// Auteur : Aboubacar Sidiki Doumbouya
/// </remarks>
public class CityStateService : INotifyPropertyChanged
{
    private string _selectedCity = "Montréal";

    /// <summary>Ville actuellement sélectionnée.</summary>
    public string SelectedCity
    {
        get => _selectedCity;
        set
        {
            if (_selectedCity == value)
            {
                return;
            }

            _selectedCity = value;
            OnPropertyChanged();
        }
    }

    /// <summary>Liste des villes disponibles dans l'application.</summary>
    public List<string> AvailableCities { get; } = new()
    {
        "Montréal", "Québec", "Laval", "Gatineau",
        "Sherbrooke", "Chambly", "Trois-Rivières"
    };

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}