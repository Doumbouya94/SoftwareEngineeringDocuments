using EventGoApp.ViewModels;

namespace EventGoApp.Views;

public partial class EditEventPage : ContentPage
{
    public EditEventPage(EditEventViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
