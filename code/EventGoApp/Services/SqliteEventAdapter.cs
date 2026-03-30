using SQLite;
using EventGoApp.Models;

namespace EventGoApp.Services;

/// <summary>
/// Adaptateur qui rend SqliteService compatible avec IEventAdapter.
/// Traduit les appels de l'interface cible (IEventAdapter) en appels
/// vers la connexion SQLite (SQLiteAsyncConnection).
/// </summary>
/// <remarks>
/// Auteur : Aboubacar Sidiki Doumbouya
/// Patron : Adaptateur 
/// </remarks>

public class SqliteEventAdapter : IEventAdapter
{
    // Connexion SQLite récupérée depuis SqliteService (l'adapté)
    private readonly SQLiteAsyncConnection _db;

    /// <summary>
    /// Constructeur — reçoit le SqliteService et récupère directement
    /// la connexion active via GetConnection().
    /// </summary>
    public SqliteEventAdapter(SqliteService sqliteService)
    {
        _db = sqliteService.GetConnection();
    }

    /// <summary>
    /// Récupère tous les événements de la base de données,
    /// triés par date croissante.
    /// Adapte : GetAllAsync() → SQLite Table<Event>().ToListAsync()
    /// </summary>
    public async Task<IReadOnlyList<Event>> GetAllAsync()
    {
        var events = await _db.Table<Event>().ToListAsync();
        return events.OrderBy(e => e.Date).ToList();
    }

    /// <summary>
    /// Récupère les événements filtrés par catégorie,
    /// triés par date croissante.
    /// Adapte : GetByCategoryAsync(category) → SQLite Where(e => e.Category == category)
    /// </summary>
    public async Task<IReadOnlyList<Event>> GetByCategoryAsync(EventCategory category)
    {
        var events = await _db.Table<Event>()
            .Where(e => e.Category == category)
            .ToListAsync();
        return events.OrderBy(e => e.Date).ToList();
    }

    /// <summary>
    /// Récupère les événements filtrés par catégorie, ville et/ou prix maximum.
    /// Les filtres sont optionnels (nullable) et combinables.
    /// Note : le filtre category est appliqué côté SQLite,
    /// les filtres city et maxPrice sont appliqués en mémoire
    /// car SQLite-net ne supporte pas StringComparison ni le cast decimal directement.
    /// Adapte : GetFilteredAsync(...) → SQLite + filtres LINQ en mémoire
    /// </summary>
    public async Task<IReadOnlyList<Event>> GetFilteredAsync(
        EventCategory? category, string? city, decimal? maxPrice)
    {
        // Requête de base sur la table Event
        var query = _db.Table<Event>();

        // Filtre par catégorie appliqué directement dans SQLite
        if (category.HasValue)
        {
            query = query.Where(e => e.Category == category.Value);
        }

        // Chargement en mémoire pour les filtres suivants
        var events = await query.ToListAsync();

        IEnumerable<Event> result = events;

        // Filtre par ville appliqué en mémoire
        if (!string.IsNullOrEmpty(city))
        {
            result = result.Where(e =>
                e.City.Equals(city, StringComparison.OrdinalIgnoreCase));
        }

        // Filtre par prix maximum appliqué en mémoire
        if (maxPrice.HasValue)
        {
            result = result.Where(e => (decimal)e.Price <= maxPrice.Value);
        }

        return result.OrderBy(e => e.Date).ToList();
    }

    /// <summary>
    /// Insère les données de test dans la base de données si elle est vide.
    /// Contient 20 événements répartis dans plusieurs villes québécoises
    /// (Montréal, Québec, Laval, Gatineau, Sherbrooke, Trois-Rivières).
    /// </summary>
    public async Task SeedEventsAsync()
    {
        // Vérifier si la table contient déjà des données
        var count = await _db.Table<Event>().CountAsync();
        if (count > 0)
        {
            return;
        }

        var now = DateTime.UtcNow;

        // Liste des 20 événements de test
        var events = new List<Event>
        {
            new() { Id = Guid.NewGuid(), Title = "Festival de Jazz de Montréal",
                Description = "Le plus grand festival de jazz en Amérique du Nord vous accueille pour 11 jours de concerts gratuits et payants.",
                Date = new DateTime(2026, 6, 27, 19, 0, 0), City = "Montréal", Venue = "Quartier des spectacles",
                Category = EventCategory.Concerts, Price = 0, ImagePlaceholderColor = "#1A237E",
                IsFeatured = true, CreatedAt = now, UpdatedAt = now },

            new() { Id = Guid.NewGuid(), Title = "Osheaga — Scène Principale",
                Description = "Trois jours de musique avec les plus grandes têtes d'affiche internationales au parc Jean-Drapeau.",
                Date = new DateTime(2026, 8, 1, 14, 0, 0), City = "Montréal", Venue = "Parc Jean-Drapeau",
                Category = EventCategory.Festivals, Price = 85, ImagePlaceholderColor = "#4A148C",
                IsFeatured = true, CreatedAt = now, UpdatedAt = now },

            new() { Id = Guid.NewGuid(), Title = "Match des Canadiens de Montréal",
                Description = "Venez encourager le Tricolore dans ce match crucial contre les Maple Leafs de Toronto.",
                Date = new DateTime(2026, 4, 14, 19, 30, 0), City = "Montréal", Venue = "Centre Bell",
                Category = EventCategory.Sports, Price = 45, ImageSource = "dotnet_bot.png", ImagePlaceholderColor = "#B71C1C",
                IsFeatured = false, CreatedAt = now, UpdatedAt = now },

            new() { Id = Guid.NewGuid(), Title = "Soirée Électro au Stereo Bar",
                Description = "Une nuit de musique électronique avec des DJs locaux et internationaux dans l'une des meilleures salles de Montréal.",
                Date = new DateTime(2026, 4, 19, 22, 0, 0), City = "Montréal", Venue = "Stereo Bar",
                Category = EventCategory.Parties, Price = 15, ImageSource = "dotnet_bot.png", ImagePlaceholderColor = "#880E4F",
                IsFeatured = false, CreatedAt = now, UpdatedAt = now },

            new() { Id = Guid.NewGuid(), Title = "Brunch Gastronomique au Marché Jean-Talon",
                Description = "Découvrez les saveurs locales avec des chefs étoilés qui préparent des créations culinaires uniques.",
                Date = new DateTime(2026, 5, 10, 10, 0, 0), City = "Montréal", Venue = "Marché Jean-Talon",
                Category = EventCategory.Food, Price = 35, ImagePlaceholderColor = "#F57F17",
                IsFeatured = false, CreatedAt = now, UpdatedAt = now },

            new() { Id = Guid.NewGuid(), Title = "Exposition Banksy au MAC",
                Description = "Une rétrospective immersive de l'artiste anonyme Banksy présentant ses œuvres les plus emblématiques.",
                Date = new DateTime(2026, 7, 5, 11, 0, 0), City = "Montréal", Venue = "Musée d'art contemporain",
                Category = EventCategory.Arts, Price = 20, ImagePlaceholderColor = "#006064",
                IsFeatured = false, CreatedAt = now, UpdatedAt = now },

            new() { Id = Guid.NewGuid(), Title = "Randonnée au Parc du Mont-Royal",
                Description = "Rejoignez un groupe de randonneurs pour explorer les sentiers du mont Royal et profiter de la vue sur Montréal.",
                Date = new DateTime(2026, 5, 24, 9, 0, 0), City = "Montréal", Venue = "Parc du Mont-Royal",
                Category = EventCategory.Outdoor, Price = 0, ImagePlaceholderColor = "#1B5E20",
                IsFeatured = false, CreatedAt = now, UpdatedAt = now },

            new() { Id = Guid.NewGuid(), Title = "Conférence Tech MTL 2026",
                Description = "La grande conférence annuelle dédiée aux nouvelles technologies, à l'IA et à l'innovation technologique.",
                Date = new DateTime(2026, 9, 15, 9, 0, 0), City = "Montréal", Venue = "Palais des congrès",
                Category = EventCategory.Networking, Price = 50, ImagePlaceholderColor = "#37474F",
                IsFeatured = false, CreatedAt = now, UpdatedAt = now },

            new() { Id = Guid.NewGuid(), Title = "Festival d'Été de Québec",
                Description = "Le plus grand festival de musique en plein air au monde avec plus de 200 spectacles gratuits dans les rues de Québec.",
                Date = new DateTime(2026, 7, 9, 17, 0, 0), City = "Québec", Venue = "Plaines d'Abraham",
                Category = EventCategory.Festivals, Price = 0, ImagePlaceholderColor = "#0D47A1",
                IsFeatured = true, CreatedAt = now, UpdatedAt = now },

            new() { Id = Guid.NewGuid(), Title = "Match des Remparts de Québec",
                Description = "Un match de hockey junior excitant mettant en vedette les jeunes talents des Remparts.",
                Date = new DateTime(2026, 4, 28, 19, 0, 0), City = "Québec", Venue = "Videotron Centre",
                Category = EventCategory.Sports, Price = 22, ImagePlaceholderColor = "#B71C1C",
                IsFeatured = false, CreatedAt = now, UpdatedAt = now },

            new() { Id = Guid.NewGuid(), Title = "Soirée Salsa au Bar L'Auberge",
                Description = "Apprenez la salsa avec des instructeurs professionnels et dansez jusqu'au bout de la nuit.",
                Date = new DateTime(2026, 4, 4, 20, 0, 0), City = "Québec", Venue = "Bar L'Auberge",
                Category = EventCategory.Parties, Price = 10, ImagePlaceholderColor = "#880E4F",
                IsFeatured = false, CreatedAt = now, UpdatedAt = now },

            new() { Id = Guid.NewGuid(), Title = "Visite guidée de la Vieille-Capitale",
                Description = "Découvrez l'histoire fascinante de Québec lors d'une visite guidée à pied dans les rues du Vieux-Québec.",
                Date = new DateTime(2026, 6, 14, 10, 0, 0), City = "Québec", Venue = "Vieux-Québec",
                Category = EventCategory.Arts, Price = 18, ImagePlaceholderColor = "#006064",
                IsFeatured = false, CreatedAt = now, UpdatedAt = now },

            new() { Id = Guid.NewGuid(), Title = "Festival Bières et Saveurs",
                Description = "Une célébration de la gastronomie québécoise avec plus de 150 bières artisanales et des délices locaux.",
                Date = new DateTime(2026, 9, 5, 11, 0, 0), City = "Chambly", Venue = "Fort Chambly",
                Category = EventCategory.Food, Price = 25, ImagePlaceholderColor = "#F57F17",
                IsFeatured = false, CreatedAt = now, UpdatedAt = now },

            new() { Id = Guid.NewGuid(), Title = "Concert hip-hop au Centre Laval",
                Description = "Une soirée explosive avec les meilleurs artistes hip-hop québécois et canadiens.",
                Date = new DateTime(2026, 5, 30, 20, 0, 0), City = "Laval", Venue = "Place Bell",
                Category = EventCategory.Concerts, Price = 40, ImagePlaceholderColor = "#1A237E",
                IsFeatured = false, CreatedAt = now, UpdatedAt = now },

            new() { Id = Guid.NewGuid(), Title = "Tournoi de Soccer Amateur",
                Description = "Rejoignez notre tournoi communautaire de soccer en plein air ouvert à tous les niveaux de jeu.",
                Date = new DateTime(2026, 6, 6, 8, 0, 0), City = "Laval",
                Venue = "Parc de la Rivière-des-Mille-Îles",
                Category = EventCategory.Sports, Price = 0, ImagePlaceholderColor = "#1B5E20",
                IsFeatured = false, CreatedAt = now, UpdatedAt = now },

            new() { Id = Guid.NewGuid(), Title = "Salon Carrières et Emploi",
                Description = "Rencontrez plus de 100 employeurs de la région lors de cette grande journée de réseautage professionnel.",
                Date = new DateTime(2026, 4, 21, 9, 0, 0), City = "Gatineau",
                Venue = "Palais des congrès de Gatineau",
                Category = EventCategory.Networking, Price = 0, ImagePlaceholderColor = "#37474F",
                IsFeatured = false, CreatedAt = now, UpdatedAt = now },

            new() { Id = Guid.NewGuid(), Title = "Festival des Montgolfières",
                Description = "Émerveillez-vous devant des dizaines de montgolfières colorées lors de ce festival unique en son genre.",
                Date = new DateTime(2026, 9, 3, 10, 0, 0), City = "Gatineau", Venue = "Parc du Lac Leamy",
                Category = EventCategory.Festivals, Price = 12, ImagePlaceholderColor = "#4A148C",
                IsFeatured = false, CreatedAt = now, UpdatedAt = now },

            new() { Id = Guid.NewGuid(), Title = "Balade à vélo sur la Piste de la Capitale",
                Description = "Une promenade de 20 km en groupe sur la magnifique piste cyclable longeant la rivière des Outaouais.",
                Date = new DateTime(2026, 5, 17, 9, 0, 0), City = "Gatineau", Venue = "Piste de la Capitale",
                Category = EventCategory.Outdoor, Price = 0, ImagePlaceholderColor = "#33691E",
                IsFeatured = false, CreatedAt = now, UpdatedAt = now },

            new() { Id = Guid.NewGuid(), Title = "Festival des Arts de la Rue",
                Description = "Jongleurs, acrobates, musiciens et artistes visuels investissent le centre-ville de Sherbrooke pour un week-end festif.",
                Date = new DateTime(2026, 7, 18, 11, 0, 0), City = "Sherbrooke",
                Venue = "Centre-ville de Sherbrooke",
                Category = EventCategory.Arts, Price = 0, ImagePlaceholderColor = "#006064",
                IsFeatured = false, CreatedAt = now, UpdatedAt = now },

            new() { Id = Guid.NewGuid(), Title = "Concert classique à la Salle Maurice-O'Bready",
                Description = "L'Orchestre symphonique de Trois-Rivières présente un programme de grandes œuvres romantiques.",
                Date = new DateTime(2026, 10, 3, 20, 0, 0), City = "Trois-Rivières",
                Venue = "Salle Maurice-O'Bready",
                Category = EventCategory.Concerts, Price = 30, ImagePlaceholderColor = "#1A237E",
                IsFeatured = false, CreatedAt = now, UpdatedAt = now },
        };

        // Insérer tous les événements en une seule opération
        await _db.InsertAllAsync(events);
    }
}