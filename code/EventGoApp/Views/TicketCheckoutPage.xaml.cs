using EventGoApp.ViewModels;

namespace EventGoApp.Views;

public partial class TicketCheckoutPage : ContentPage
{
    public TicketCheckoutPage(TicketCheckoutViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
