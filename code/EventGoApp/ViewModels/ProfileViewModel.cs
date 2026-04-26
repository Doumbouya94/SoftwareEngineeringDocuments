using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using EventGoApp.Models;
using EventGoApp.Services;

namespace EventGoApp.ViewModels;

public class SelectedToColorConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
    {
        bool isSelected = value is bool b && b;
        string param = parameter?.ToString() ?? string.Empty;

        if (param == "Background")
            return isSelected ? Color.FromArgb("#6200EE") : Colors.White;
        if (param == "Stroke")
            return isSelected ? Color.FromArgb("#6200EE") : Colors.Black;
        if (param == "Text")
            return isSelected ? Colors.White : Colors.Black;

        return Colors.Transparent;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
        => throw new NotImplementedException();
}

public class CategoryOption : INotifyPropertyChanged
{
    private bool _isSelected;
    public EventCategory Category { get; set; }
    public string Name => CategoryToLabel(Category);

    private string CategoryToLabel(EventCategory cat) => cat switch
    {
        EventCategory.Concerts => "Concerts",
        EventCategory.Festivals => "Festivals",
        EventCategory.Sports => "Sports",
        EventCategory.Parties => "Soirées",
        EventCategory.Food => "Gastronomie",
        EventCategory.Arts => "Arts & Culture",
        EventCategory.Outdoor => "Plein air",
        EventCategory.Networking => "Réseautage",
        _ => cat.ToString()
    };

    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            _isSelected = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}

public class ProfileViewModel : INotifyPropertyChanged
{
    private readonly AuthStateService _authState;
    private readonly SqliteService _sqliteService;
    private string _fullName = string.Empty;
    private string _city = string.Empty;
    private string _initials = string.Empty;

    public ProfileViewModel(AuthStateService authState, SqliteService sqliteService)
    {
        _authState = authState;
        _sqliteService = sqliteService;
        Categories = new ObservableCollection<CategoryOption>();
        LogoutCommand = new Command(OnLogout);
        SaveCommand = new Command(OnSave);
        ToggleCategoryCommand = new Command<CategoryOption>(OnToggleCategory);
        GoToMyEventsCommand = new Command(async () => await Shell.Current.GoToAsync("myevents"));

        LoadUserData();
    }

    public string FullName
    {
        get => _fullName;
        set { _fullName = value; OnPropertyChanged(); }
    }

    public string City
    {
        get => _city;
        set { _city = value; OnPropertyChanged(); }
    }

    public string Initials
    {
        get => _initials;
        set { _initials = value; OnPropertyChanged(); }
    }

    public ObservableCollection<CategoryOption> Categories { get; }

    public ICommand LogoutCommand { get; }
    public ICommand SaveCommand { get; }
    public ICommand ToggleCategoryCommand { get; }
    public ICommand GoToMyEventsCommand { get; }

    public void LoadUserData()
    {
        var user = _authState.CurrentUser;
        if (user != null)
        {
            FullName = user.FullName;
            City = user.City;
            Initials = string.IsNullOrWhiteSpace(user.FullName) ? "?" : user.FullName[0].ToString().ToUpper();

            var preferred = user.PreferredCategories;
            Categories.Clear();
            foreach (EventCategory cat in Enum.GetValues(typeof(EventCategory)))
            {
                Categories.Add(new CategoryOption
                {
                    Category = cat,
                    IsSelected = preferred.Contains(cat)
                });
            }
        }
        else
        {
            FullName = "Invité";
            City = "Mode découverte";
            Initials = "G";
            
            Categories.Clear();
            foreach (EventCategory cat in Enum.GetValues(typeof(EventCategory)))
            {
                Categories.Add(new CategoryOption
                {
                    Category = cat,
                    IsSelected = false
                });
            }
        }
    }

    private void OnToggleCategory(CategoryOption option)
    {
        if (option == null)
        {
            return;
        }

        option.IsSelected = !option.IsSelected;
    }

    private async void OnSave()
    {
        if (_authState.CurrentUser == null)
        {
            await Shell.Current.DisplayAlert("Information", "Veuillez vous connecter pour enregistrer vos préférences.", "OK");
            return;
        }

        // Update preferences in the user object
        var user = _authState.CurrentUser;
        var preferred = Categories.Where(c => c.IsSelected).Select(c => c.Category).ToList();
        user.PreferredCategories = preferred;

        // Persist to DB
        await _sqliteService.GetConnection().UpdateAsync(user);

        // Show feedback
        await Shell.Current.DisplayAlert("Succès", "Vos préférences ont été enregistrées.", "OK");
    }

    private async void OnLogout()
    {
        bool answer = await Shell.Current.DisplayAlert("Déconnexion", "Êtes-vous sûr de vouloir vous déconnecter ?", "Oui", "Non");
        if (answer)
        {
            _authState.SetLoggedOut();
            await Shell.Current.GoToAsync("//welcome");
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
