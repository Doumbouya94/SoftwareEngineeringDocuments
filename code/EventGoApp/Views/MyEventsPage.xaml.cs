using EventGoApp.ViewModels;

namespace EventGoApp.Views;

public partial class MyEventsPage : ContentPage
{
    private readonly MyEventsViewModel _vm;

    public MyEventsPage(MyEventsViewModel vm)
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
