using EventGoApp.ViewModels;

namespace EventGoApp.Views;

/// <summary>
/// Page sheet de sélection de ville ou de détection automatique de position.
/// </summary>
/// <remarks>
/// Auteur : Aboubacar Sidiki Doumbouya
/// UserStory : US3.2 (Géolocalisation)
/// </remarks>
public partial class CityPickerPage : ContentPage
{
    public CityPickerPage(CityPickerViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}