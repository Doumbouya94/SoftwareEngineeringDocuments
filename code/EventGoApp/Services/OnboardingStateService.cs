using EventGoApp.Models;

namespace EventGoApp.Services;

/// <summary>
/// Énumération représentant les différents modes sociaux que l'utilisateur peut sélectionner lors de l'onboarding.
/// </summary>
/// <remarks>
/// Auteur : Pierre
/// Patron de conception : State Pattern — permet de gérer l'état du mode social sélectionné par l'utilisateur lors de l'onboarding.
/// Epic : "Personnalisation de l'expérience utilisateur lors de l'onboarding" — le mode social sélectionné par l'utilisateur peut influencer les recommandations d'événements et les fonctionnalités de l'application, en adaptant l'expérience en fonction des préférences sociales de l'utilisateur.
/// </remarks>
public enum SocialMode
{
    Solo,
    WithFriends,
    WithPartner,
    WithFamily
}

/// <summary>
/// Énumération représentant les différentes tranches de budget que l'utilisateur peut sélectionner lors de l'onboarding.
/// </summary>
public enum BudgetTier
{
    FreeOnly,
    Under20,
    Between20And50,
    Above50
}

/// <summary>
/// Service pour gérer l'état de l'onboarding, y compris les étapes, la ville sélectionnée,
/// les catégories d'événements, le mode social et le budget.
/// </summary>
public class OnboardingStateService
{
    public int CurrentStep { get; private set; } = 0;
    public const int TotalSteps = 4; // 0: Ville, 1: Catégories, 2: Mode social, 3: Budget

    public string SelectedCity { get; set; } = "Montréal";
    public HashSet<EventCategory> SelectedCategories { get; } = new();
    public SocialMode? SelectedSocialMode { get; set; }
    public BudgetTier? SelectedBudget { get; set; }

    public bool IsComplete => CurrentStep >= TotalSteps;


    // Permet de passer à l'étape suivante de l'onboarding
    public void NextStep()
    {
        if (CurrentStep < TotalSteps)
        {
            CurrentStep++;
        }
    }

    // Permet de revenir à l'étape précédente de l'onboarding
    public void PreviousStep()
    {
        if (CurrentStep > 0)
        {
            CurrentStep--;
        }
    }

    // Réinitialise l'état de l'onboarding à ses valeurs par défaut,
    // permettant à l'utilisateur de recommencer le processus d'onboarding depuis le début.
    public void Reset()
    {
        CurrentStep = 0;
        SelectedCity = "Montréal";
        SelectedCategories.Clear();
        SelectedSocialMode = null;
        SelectedBudget = null;
    }
}
