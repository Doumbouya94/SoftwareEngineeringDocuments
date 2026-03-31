using EventGoApp.Models;

namespace EventGoApp.Services;

/// <summary>
/// Mode social sélectionné par l'utilisateur lors de l'onboarding.
/// </summary>
public enum SocialMode
{
    Solo,
    WithFriends,
    WithPartner,
    WithFamily
}

/// <summary>
/// Tranche de budget sélectionnée par l'utilisateur lors de l'onboarding.
/// </summary>
public enum BudgetTier
{
    FreeOnly,
    Under20,
    Between20And50,
    Above50
}

/// <summary>
/// Service de gestion de l'état des 4 étapes d'onboarding.
/// </summary>
/// <remarks>
/// Auteur : Pierre
/// Patron de conception : State — suit la progression et les sélections de l'utilisateur à travers les étapes.
/// UserStories : US1.1 (inscription avec préférences).
/// Épic : Authentification et gestion des utilisateurs.
/// </remarks>
public class OnboardingStateService
{
    /// <summary>Étape courante (0 à 3).</summary>
    public int CurrentStep { get; private set; } = 0;

    /// <summary>Nombre total d'étapes : 0=Ville, 1=Catégories, 2=Mode social, 3=Budget.</summary>
    public const int TotalSteps = 4;

    /// <summary>Ville sélectionnée par l'utilisateur.</summary>
    public string SelectedCity { get; set; } = "Montréal";

    /// <summary>Catégories d'événements sélectionnées par l'utilisateur.</summary>
    public HashSet<EventCategory> SelectedCategories { get; } = new();

    /// <summary>Mode social sélectionné par l'utilisateur.</summary>
    public SocialMode? SelectedSocialMode { get; set; }

    /// <summary>Tranche de budget sélectionnée par l'utilisateur.</summary>
    public BudgetTier? SelectedBudget { get; set; }

    /// <summary>Vrai si toutes les étapes ont été complétées.</summary>
    public bool IsComplete => CurrentStep >= TotalSteps;

    /// <summary>Avance à l'étape suivante si possible.</summary>
    public void NextStep()
    {
        if (CurrentStep < TotalSteps)
        {
            CurrentStep++;
        }
    }

    /// <summary>Revient à l'étape précédente si possible.</summary>
    public void PreviousStep()
    {
        if (CurrentStep > 0)
        {
            CurrentStep--;
        }
    }

    /// <summary>Réinitialise toutes les sélections et revient à l'étape 0.</summary>
    public void Reset()
    {
        CurrentStep = 0;
        SelectedCity = "Montréal";
        SelectedCategories.Clear();
        SelectedSocialMode = null;
        SelectedBudget = null;
    }
}
