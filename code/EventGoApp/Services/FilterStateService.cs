using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventGoApp.Models;

namespace EventGoApp.Services;

/// <summary>
/// Service partagé qui conserve l'état des filtres actifs.
/// Permet la communication entre FilterViewModel et HomeViewModel.
/// </summary>
/// <remarks>
/// Auteur : Aboubacar Sidiki Doumbouya
/// </remarks>
public class FilterStateService
{
    /// <summary>Ville sélectionnée. Null = toutes les villes.</summary>
    public string? SelectedCity { get; set; }

    /// <summary>Prix maximum sélectionné. Null = tous les prix.</summary>
    public decimal? MaxPrice { get; set; }

    /// <summary>Filtre de date sélectionné (ex: Aujourd'hui, Ce week-end).</summary>
    public string? SelectedDateFilter { get; set; }

    /// <summary>Indique si on n'affiche que les événements gratuits.</summary>
    public bool IsFreeOnly { get; set; }

    /// <summary>Indique si des filtres sont actifs.</summary>
    public bool HasActiveFilters =>
        SelectedCity != null || MaxPrice != null || SelectedDateFilter != null || IsFreeOnly;

    /// <summary>Réinitialise tous les filtres.</summary>
    public void Reset()
    {
        SelectedCity = null;
        MaxPrice = null;
        SelectedDateFilter = null;
        IsFreeOnly = false;
    }
}