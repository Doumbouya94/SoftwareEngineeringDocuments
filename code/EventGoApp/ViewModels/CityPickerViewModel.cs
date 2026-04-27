using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using EventGoApp.Services;

namespace EventGoApp.ViewModels;

/// <summary>
/// Vue-modèle de la page sheet de sélection de ville.
/// Gère la sélection manuelle et la détection automatique via GPS.
/// </summary>
/// <remarks>
/// Auteur : Aboubacar Sidiki Doumbouya
/// UserStory : US3.2 (Géolocalisation)
/// </remarks>
public class CityPickerViewModel : INotifyPropertyChanged
{
    private readonly CityStateService _cityState;
    private string _selectedCity;
    private bool _isDetecting;
    private string _detectionStatus = string.Empty;

    public CityPickerViewModel(CityStateService cityState)
    {
        _cityState = cityState;
        _selectedCity = cityState.SelectedCity;

        SelectCityCommand = new Command<string>(async city =>
        {
            _cityState.SelectedCity = city;
            await Shell.Current.GoToAsync("..");
        });

        DetectLocationCommand = new Command(async () => await DetectLocationAsync());
    }

    /// <summary>Liste des villes disponibles.</summary>
    public List<string> Cities => _cityState.AvailableCities;

    /// <summary>Ville actuellement sélectionnée.</summary>
    public string SelectedCity
    {
        get => _selectedCity;
        set
        {
            if (_selectedCity == value) return;
            _selectedCity = value;
            OnPropertyChanged();
        }
    }

    /// <summary>Indique si la détection GPS est en cours.</summary>
    public bool IsDetecting
    {
        get => _isDetecting;
        set
        {
            _isDetecting = value;
            OnPropertyChanged();
        }
    }

    /// <summary>Message de statut de la détection GPS.</summary>
    public string DetectionStatus
    {
        get => _detectionStatus;
        set
        {
            _detectionStatus = value;
            OnPropertyChanged();
        }
    }

    public ICommand SelectCityCommand { get; }
    public ICommand DetectLocationCommand { get; }

    /// <summary>
    /// Détecte automatiquement la position de l'utilisateur via GPS
    /// et sélectionne la ville la plus proche dans la liste.
    /// </summary>
    private async Task DetectLocationAsync()
    {
        IsDetecting = true;
        DetectionStatus = "Détection en cours...";

        try
        {
            // Demander la permission à l'exécution
            var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

            if (status != PermissionStatus.Granted)
            {
                DetectionStatus = "Permission refusée. Sélectionnez une ville manuellement.";
                IsDetecting = false;
                return;
            }

            // Obtenir la position GPS
            var location = await Geolocation.GetLocationAsync(new GeolocationRequest
            {
                DesiredAccuracy = GeolocationAccuracy.Medium,
                Timeout = TimeSpan.FromSeconds(10)
            });

            if (location == null)
            {
                DetectionStatus = "Position introuvable. Réessayez.";
                IsDetecting = false;
                return;
            }

            // Trouver la ville la plus proche
            var detectedCity = GetNearestCity(location.Latitude, location.Longitude);
            _cityState.SelectedCity = detectedCity;
            DetectionStatus = $"Ville détectée : {detectedCity}";

            await Task.Delay(1000);
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            DetectionStatus = $"Erreur : {ex.Message}";
        }
        finally
        {
            IsDetecting = false;
        }
    }

    /// <summary>
    /// Retourne la ville disponible la plus proche des coordonnées GPS données.
    /// </summary>
    private string GetNearestCity(double lat, double lon)
    {
        // Coordonnées approximatives des villes disponibles
        var cityCoordinates = new Dictionary<string, (double Lat, double Lon)>
        {
            { "Montréal",      (45.5017, -73.5673) },
            { "Québec",        (46.8139, -71.2080) },
            { "Laval",         (45.6066, -73.7124) },
            { "Gatineau",      (45.4765, -75.7013) },
            { "Sherbrooke",    (45.4042, -71.8929) },
            { "Chambly",       (45.4423, -73.2876) },
            { "Trois-Rivières",(46.3432, -72.5418) }
        };

        string nearest = "Montréal";
        double minDistance = double.MaxValue;

        foreach (var city in cityCoordinates)
        {
            double distance = Math.Sqrt(
                Math.Pow(city.Value.Lat - lat, 2) +
                Math.Pow(city.Value.Lon - lon, 2));

            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = city.Key;
            }
        }

        return nearest;
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}