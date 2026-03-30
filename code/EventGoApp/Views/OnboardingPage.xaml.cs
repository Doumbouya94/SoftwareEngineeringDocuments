using EventGoApp.Models;
using EventGoApp.Services;

namespace EventGoApp.Views;

public partial class OnboardingPage : ContentPage
{
    private readonly OnboardingStateService _onboarding;

    private static readonly string[] Cities = new[]
    {
        "Montréal", "Québec", "Laval", "Gatineau", "Longueuil",
        "Sherbrooke", "Saguenay", "Lévis", "Trois-Rivières", "Terrebonne"
    };

    private static readonly string[] StepTitles = new[]
    {
        "Où êtes-vous situé?",
        "Quels types d'événements vous intéressent?",
        "Vous sortez habituellement...",
        "Quel est votre budget?"
    };

    
    private readonly Dictionary<string, EventCategory> _categoryBorders;
    private readonly Dictionary<string, SocialMode> _socialBorders;
    private readonly Dictionary<string, BudgetTier> _budgetBorders;

    public OnboardingPage(OnboardingStateService onboarding)
    {
        InitializeComponent();
        _onboarding = onboarding;
        _onboarding.Reset();

        _categoryBorders = new()
        {
            { "Cat_Concerts",   EventCategory.Concerts },
            { "Cat_Festivals",  EventCategory.Festivals },
            { "Cat_Sports",     EventCategory.Sports },
            { "Cat_Parties",    EventCategory.Parties },
            { "Cat_Food",       EventCategory.Food },
            { "Cat_Arts",       EventCategory.Arts },
            { "Cat_Outdoor",    EventCategory.Outdoor },
            { "Cat_Networking", EventCategory.Networking },
        };

        _socialBorders = new()
        {
            { "Social_Solo",    SocialMode.Solo },
            { "Social_Friends", SocialMode.WithFriends },
            { "Social_Partner", SocialMode.WithPartner },
            { "Social_Family",  SocialMode.WithFamily },
        };

        _budgetBorders = new()
        {
            { "Budget_Free",    BudgetTier.FreeOnly },
            { "Budget_Under20", BudgetTier.Under20 },
            { "Budget_20to50",  BudgetTier.Between20And50 },
            { "Budget_Above50", BudgetTier.Above50 },
        };


        foreach (var city in Cities)
        {
            CityPicker.Items.Add(city);
        }

        CityPicker.SelectedIndex = 0; // par défaut Montréal

        RefreshUI();
    }

    // ── Navigation ───────────────────────────────────────────

    private async void OnNextClicked(object sender, EventArgs e)
    {
        if (_onboarding.CurrentStep == Cities.Length - 1 || _onboarding.CurrentStep >= 3)
        {
            await Shell.Current.GoToAsync("//home");
            return;
        }
        _onboarding.NextStep();
        RefreshUI();
    }

    private void OnPrevClicked(object sender, EventArgs e)
    {
        _onboarding.PreviousStep();
        RefreshUI();
    }

    private async void OnSkipTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("//home");
    }

    // ── Étape 1 : Catégorie de sélection ───────────────────────────

    private void OnCategoryTapped(object sender, TappedEventArgs e)
    {
        if (sender is not Element el) return;
        var param = e.Parameter?.ToString();
        if (param == null) return;

        if (!Enum.TryParse<EventCategory>(param, out var cat)) return;

        if (_onboarding.SelectedCategories.Contains(cat))
            _onboarding.SelectedCategories.Remove(cat);
        else
            _onboarding.SelectedCategories.Add(cat);

        // Update visual state for all category borders
        foreach (var kv in _categoryBorders)
        {
            var border = (Border)FindByName(kv.Key);
            if (border == null) continue;
            bool selected = _onboarding.SelectedCategories.Contains(kv.Value);
            border.BackgroundColor = selected ? Color.FromArgb("#2C0070") : Colors.White;
            border.Stroke = selected ? Color.FromArgb("#6200EE") : Color.FromArgb("#D1C4E9");
            
            // Also update label color for better contrast when selected
            if (border.Content is Label label)
                label.TextColor = selected ? Colors.White : Color.FromArgb("#2D1B4E");
        }
    }

    // ── Step 2: Mode social ──────────────────────────────────

    private void OnSocialTapped(object sender, TappedEventArgs e)
    {
        var param = e.Parameter?.ToString();
        if (param == null) return;

        if (!Enum.TryParse<SocialMode>(param, out var mode))
        {
            return;
        }

        _onboarding.SelectedSocialMode = mode;

        foreach (var kv in _socialBorders)
        {
            var border = (Border)FindByName(kv.Key);
            if (border == null) continue;
            bool selected = _onboarding.SelectedSocialMode == kv.Value;
            border.BackgroundColor = selected ? Color.FromArgb("#2C0070") : Colors.White;
            border.Stroke = selected ? Color.FromArgb("#6200EE") : Color.FromArgb("#D1C4E9");

            if (border.Content is Label label)
                label.TextColor = selected ? Colors.White : Color.FromArgb("#2D1B4E");
        }
    }

    // ── Step 3: Budget ───────────────────────────────────────

    private void OnBudgetTapped(object sender, TappedEventArgs e)
    {
        var param = e.Parameter?.ToString();
        if (param == null) return;

        if (!Enum.TryParse<BudgetTier>(param, out var tier)) return;
        _onboarding.SelectedBudget = tier;

        foreach (var kv in _budgetBorders)
        {
            var border = (Border)FindByName(kv.Key);
            if (border == null) continue;
            bool selected = _onboarding.SelectedBudget == kv.Value;
            border.BackgroundColor = selected ? Color.FromArgb("#2C0070") : Colors.White;
            border.Stroke = selected ? Color.FromArgb("#6200EE") : Color.FromArgb("#D1C4E9");

            if (border.Content is Label label)
                label.TextColor = selected ? Colors.White : Color.FromArgb("#2D1B4E");
        }
    }

    // ── Rafraichissement du UI ───────────────────────────────────────────

    private void RefreshUI()
    {
        int step = _onboarding.CurrentStep;

        StepLabel.Text = $"Étape {step + 1} sur 4";
        StepTitle.Text = StepTitles[step];
        StepProgress.Progress = (step + 1) / 4.0;

        Step0Content.IsVisible = step == 0;
        Step1Content.IsVisible = step == 1;
        Step2Content.IsVisible = step == 2;
        Step3Content.IsVisible = step == 3;

        PrevButton.IsVisible = step > 0;

        if (step == 0)
        {
            Grid.SetColumnSpan(NextButton, 2);
            Grid.SetColumn(NextButton, 0);
        }
        else
        {
            Grid.SetColumnSpan(NextButton, 1);
            Grid.SetColumn(NextButton, 1);
        }

        NextButton.Text = step == 3 ? "Terminer" : "Suivant";
    }
}
