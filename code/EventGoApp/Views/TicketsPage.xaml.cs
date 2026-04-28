using EventGoApp.ViewModels;

namespace EventGoApp.Views;

public partial class TicketsPage : ContentPage
{
    private readonly TicketsViewModel _vm;

    public TicketsPage(TicketsViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _vm.LoadAsync();
    }
}
