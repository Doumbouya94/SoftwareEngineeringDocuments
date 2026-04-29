# Rapport Technique Final — EventGo

**Cours :** Développement d'applications mobiles  
**Équipe :** Pierre-Sylvestre Cypré, Aboubacar Sidiki Doumbouya  
**Date :** Avril 2026  

---

## Table des matières

1. [Présentation du projet](#1-présentation-du-projet)
2. [Choix techniques](#2-choix-techniques)
3. [Description de l'architecture](#3-description-de-larchitecture)
4. [Patrons de conception](#4-patrons-de-conception)
5. [Tests unitaires](#5-tests-unitaires)
6. [Répartition des tâches](#6-répartition-des-tâches)
7. [Auto-évaluation et réflexion sur l'avenir](#7-auto-évaluation-et-réflexion-sur-lavenir)

---

## 1. Présentation du projet

EventGo est une application mobile de découverte d'événements développée avec .NET MAUI 9 et C#. Elle permet aux utilisateurs de parcourir, rechercher et gérer des événements culturels, sportifs et sociaux dans plusieurs villes du Québec. L'application couvre l'ensemble du cycle de vie de l'utilisateur : inscription, personnalisation, découverte, gestion des favoris, achat de billets et création d'événements.

### Objectifs

- Offrir une expérience de découverte d'événements locaux simple et personnalisée
- Permettre la gestion complète des favoris avec possibilité d'annulation
- Permettre l'achat de billets avec calcul automatique des taxes (TPS et TVQ)
- Permettre aux organisateurs de créer, modifier et supprimer leurs propres événements

### Plateformes cibles

L'application cible quatre plateformes à partir d'un seul projet de code source : Android, iOS, macOS et Windows.

### Données de démonstration

L'application est livrée avec 21 événements répartis sur 8 catégories et 7 villes québécoises (Montréal, Québec, Laval, Gatineau, Sherbrooke, Chambly, Trois-Rivières), ainsi qu'un compte de démonstration accessible via `demo@eventgo.ca` avec le mot de passe `Demo1234`.

---

## 2. Choix techniques

### .NET MAUI 9

.NET MAUI (Multi-platform App UI) a été choisi pour sa capacité à cibler Android, iOS, macOS et Windows à partir d'un seul projet C#. Cela évite la duplication de code et permet une maintenance centralisée. Le framework offre une intégration native avec les API de la plateforme, notamment la géolocalisation utilisée dans `CityPickerViewModel`.

### SQLite via sqlite-net-pcl

SQLite a été retenu comme solution de persistance locale pour plusieurs raisons : absence de serveur requis, intégration simple via l'ORM `sqlite-net-pcl`, et compatibilité cross-platform. Les tables sont créées automatiquement au démarrage via `SqliteService`, ce qui élimine le besoin de migrations. Le fichier de base de données est stocké à `FileSystem.AppDataDirectory/eventgo.db`.

### BCrypt pour le hachage des mots de passe

La bibliothèque `BCrypt.Net-Next` est utilisée pour le hachage des mots de passe. BCrypt est un algorithme de hachage adaptatif conçu spécifiquement pour les mots de passe. Il intègre un sel aléatoire et un facteur de coût configurable, ce qui le rend résistant aux attaques par force brute.

### Architecture MVVM

Le patron MVVM (Model-View-ViewModel) a été adopté pour séparer clairement la logique de présentation de l'interface utilisateur. Les vues XAML ne contiennent aucune logique métier. Toute la logique de présentation réside dans les ViewModels, qui exposent leurs données via `INotifyPropertyChanged`.

### Authentification locale

L'authentification est entièrement locale, sans serveur distant. Ce choix simplifie l'architecture pour un projet académique tout en démontrant les principes de sécurité de base (hachage BCrypt, gestion de session via `AuthStateService`).

### Packages NuGet

| Package | Version | Utilisation |
|---|---|---|
| Microsoft.Maui.Controls | 9.0.51 | Framework MAUI |
| sqlite-net-pcl | 1.9.172 | ORM SQLite |
| BCrypt.Net-Next | 4.1.0 | Hachage des mots de passe |
| Microsoft.Extensions.Logging.Debug | 9.0.0 | Journalisation en mode débogage |

---

## 3. Description de l'architecture

### Structure du projet

```
code/
  EventGo.sln
  EventGoApp/
    MauiProgram.cs          (conteneur DI)
    App.xaml / AppShell.xaml
    Models/                 (entités de données)
    Services/               (logique métier et accès aux données)
    ViewModels/             (12 ViewModels)
    Views/                  (15 pages XAML)
    Factories/              (8 fabriques d'événements)
    Converters/             (convertisseurs XAML)
    Platforms/              (Android, iOS, MacCatalyst, Windows)
```

### Couche Modèle

Les modèles représentent les entités de données persistées en SQLite :

| Modèle | Champs principaux |
|---|---|
| `Event` | Id, Title, Description, Date, City, Address, Venue, Price, Category, IsFeatured, OrganizerId |
| `User` | Id, Email, Username, FullName, PasswordHash, City, PreferredCategories |
| `Favorite` | Id, UserId, EventId, AddedAt |
| `Ticket` | Id, UserId, EventId, EventTitle, Quantity, UnitPrice, PurchasedAt |
| `EventCategory` | Concerts, Festivals, Sports, Parties, Food, Arts, Outdoor, Networking |

Les catégories préférées de l'utilisateur sont stockées sous forme de chaîne séparée par des points-virgules dans la colonne `PreferredCategoriesRaw`, et converties en `List<EventCategory>` via une propriété calculée.

### Couche ViewModel

Les 12 ViewModels implémentent `INotifyPropertyChanged` et exposent les données aux vues via des liaisons XAML. Aucune logique ne réside dans les fichiers code-behind des pages. Les ViewModels reçoivent leurs dépendances par injection via le constructeur.

| ViewModel | Responsabilité |
|---|---|
| `HomeViewModel` | Liste des événements, filtres par pilule, recherche en temps réel |
| `EventViewModel` | Enveloppe un `Event` avec les propriétés formatées (date, prix, couleur de catégorie) |
| `EventDetailViewModel` | Chargement d'un événement par ID, bascule des favoris |
| `FilterViewModel` | Ville, prix maximum, filtre de date, gratuit uniquement |
| `FavoritesViewModel` | Liste des favoris avec ajout/retrait annulable via l'invocateur de commandes |
| `ProfileViewModel` | Informations utilisateur, catégories préférées, déconnexion |
| `CreateEventViewModel` | Formulaire de création avec validation |
| `EditEventViewModel` | Formulaire de modification pré-rempli |
| `MyEventsViewModel` | Événements créés par l'organisateur, modification et suppression |
| `CityPickerViewModel` | Sélection de ville avec géolocalisation et distance de Haversine |
| `TicketsViewModel` | Liste des billets achetés |
| `TicketCheckoutViewModel` | Sélecteur de quantité (1 à 10), calcul TPS (5%) et TVQ (9,975%) |

### Couche Vue

Les 15 pages XAML définissent uniquement la structure visuelle et les liaisons de données. La navigation est gérée par le système Shell de MAUI avec des routes enregistrées dans `AppShell.xaml.cs`.

### Flux de navigation

```
WelcomePage
  LoginPage / RegisterPage
    OnboardingPage
      HomePage (onglets : Explorer, Favoris, Tickets, Compte)
        EventDetailPage
        FilterPage
        CityPickerPage
        CreateEventPage
        MyEventsPage
          EditEventPage
        TicketsPage
          TicketCheckoutPage
        ProfilePage
```

### Injection de dépendances

Le conteneur DI est configuré dans `MauiProgram.cs`. Les services partagés sont enregistrés en tant que singletons. Les ViewModels et pages sont enregistrés en tant que transients, à l'exception de `FavoritesViewModel`, `TicketsViewModel` et `TicketsPage` qui sont des singletons afin de conserver leur état entre les navigations.

L'enregistrement de `IEventAdapter` illustre le patron Décorateur appliqué directement dans la configuration DI :

```csharp
builder.Services.AddSingleton<SqliteEventAdapter>();
builder.Services.AddSingleton<IEventAdapter>(sp =>
    new CachingEventAdapter(sp.GetRequiredService<SqliteEventAdapter>()));
```

### Services

| Service | Interface | Rôle |
|---|---|---|
| `SqliteService` | aucune | Initialisation DB et création des tables |
| `LocalAuthService` | aucune | Connexion, inscription, validation |
| `PasswordService` | aucune | Hachage et vérification BCrypt |
| `AuthStateService` | `IAuthState` | État de session (Guest, LoggedIn, LoggedOut) |
| `SqliteEventAdapter` | `IEventAdapter` | CRUD événements et requêtes filtrées |
| `CachingEventAdapter` | `IEventAdapter` | Cache en mémoire autour de SqliteEventAdapter |
| `EventSeeder` | `IEventSeeder` | Insertion des 21 événements de démonstration |
| `SqliteFavoriteRepository` | `IFavoriteRepository` | Gestion des favoris par utilisateur |
| `SqliteTicketRepository` | `ITicketRepository` | Gestion des billets par utilisateur |
| `AddFavoriteCommand` | `IFavoriteCommand` | Commande d'ajout aux favoris avec annulation |
| `RemoveFavoriteCommand` | `IFavoriteCommand` | Commande de retrait des favoris avec annulation |
| `FavoriteCommandInvoker` | aucune | Pile LIFO pour l'annulation des commandes |
| `OnboardingStateService` | aucune | État de l'onboarding en 4 étapes |
| `FilterStateService` | aucune | État actif des filtres de recherche |
| `CityStateService` | aucune | Ville sélectionnée avec notification de changement |

---

## 4. Patrons de conception

### 4.1 Décorateur — CachingEventAdapter

**Problème résolu :** Chaque navigation vers la page d'accueil déclenche un rechargement des événements. Sans optimisation, chaque appel effectue une requête SQLite complète, ce qui est inutile lorsque les données n'ont pas changé.

**Implémentation :** `CachingEventAdapter` implémente `IEventAdapter` et encapsule une instance concrète via le champ `_inner`. Chaque méthode de lecture vérifie un dictionnaire `Dictionary<string, object>` avant de déléguer. Les clés de cache intègrent tous les paramètres de la méthode pour garantir l'isolation des résultats :

```csharp
var key = $"filtered_{category}_{city}_{maxPrice}_{dateFilter}_{isFreeOnly}";
if (_cache.TryGetValue(key, out var cached))
    return (IReadOnlyList<Event>)cached;

var result = await _inner.GetFilteredAsync(category, city, maxPrice, dateFilter, isFreeOnly);
_cache[key] = result;
return result;
```

Toute opération d'écriture invalide l'intégralité du cache après avoir délégué à `_inner` :

```csharp
public async Task AddAsync(Event newEvent)
{
    await _inner.AddAsync(newEvent);
    _cache.Clear();
}
```

**Respect du patron :** Les trois conditions du Décorateur sont satisfaites : `CachingEventAdapter` implémente la même interface que l'objet enveloppé, détient une référence à cet objet via `_inner`, et délègue tous les appels à celui-ci en ajoutant un comportement supplémentaire.

**Principes SOLID respectés :** OCP (SqliteEventAdapter n'est pas modifié), SRP (la logique de cache est isolée), LSP (le décorateur remplace IEventAdapter sans changer le comportement observable).

### 4.2 Fabrique — EventFactoryRegistry

**Problème résolu :** L'application doit insérer 21 événements de démonstration au démarrage, organisés par catégorie, sans coupler `EventSeeder` aux détails de chaque type d'événement.

**Implémentation :** L'interface `IEventFactory` définit un seul contrat :

```csharp
public interface IEventFactory
{
    IEnumerable<Event> CreateEvents();
}
```

Huit classes concrètes l'implémentent, une par catégorie. La classe statique `EventFactoryRegistry` centralise leur instanciation :

```csharp
public static IReadOnlyList<IEventFactory> All() =>
[
    new ConcertEventFactory(),
    new FestivalEventFactory(),
    new SportsEventFactory(),
    new PartyEventFactory(),
    new FoodEventFactory(),
    new ArtsEventFactory(),
    new OutdoorEventFactory(),
    new NetworkingEventFactory()
];
```

`EventSeeder` itère sur toutes les fabriques sans connaître leurs implémentations concrètes. L'ajout d'une nouvelle catégorie ne nécessite que la création d'une nouvelle fabrique et son enregistrement dans le registre.

### 4.3 Commande — FavoriteCommandInvoker

**Problème résolu :** L'utilisateur doit pouvoir annuler la dernière action effectuée sur ses favoris (ajout ou retrait).

**Implémentation :** L'interface `IFavoriteCommand` expose deux méthodes : `Execute()` et `Undo()`.

`AddFavoriteCommand` :
```csharp
public async Task Execute() => await _repo.AddAsync(_event);
public async Task Undo()   => await _repo.RemoveAsync(_event.Id);
```

`RemoveFavoriteCommand` :
```csharp
public async Task Execute() => await _repo.RemoveAsync(_event.Id);
public async Task Undo()   => await _repo.AddAsync(_event);
```

`FavoriteCommandInvoker` maintient une pile LIFO `Stack<IFavoriteCommand>` :

```csharp
public async Task ExecuteAsync(IFavoriteCommand command)
{
    await command.Execute();
    _history.Push(command);
}

public async Task UndoLastAsync()
{
    if (_history.TryPop(out var last))
        await last.Undo();
}

public bool CanUndo => _history.Count > 0;
```

`FavoritesViewModel` délègue toutes les opérations à l'invocateur, sans connaître les détails des commandes concrètes.

### 4.4 Adaptateur — SqliteEventAdapter

**Problème résolu :** Les ViewModels ne doivent pas dépendre directement de `SQLiteAsyncConnection`. L'interface `IEventAdapter` définit un contrat indépendant de la source de données.

**Implémentation :** `SqliteEventAdapter` traduit chaque appel de `IEventAdapter` en requête `SQLiteAsyncConnection`. Les filtres complexes (ville, date, gratuité) sont appliqués en mémoire via LINQ après un premier filtre SQLite par catégorie, car le moteur SQLite-net-pcl ne supporte pas toutes les expressions lambda.

### 4.5 Facade — SqliteService

**Problème résolu :** L'initialisation de la connexion SQLite et la création des quatre tables (`Event`, `User`, `Favorite`, `Ticket`) doivent être centralisées et cachées derrière une interface simple.

**Implémentation :** `SqliteService` expose `InitializeAsync()` pour créer les tables et `GetConnection()` pour retourner la connexion active. Tous les services qui accèdent à SQLite reçoivent `SqliteService` par injection et appellent `GetConnection()`.

### 4.6 État — AuthStateService et OnboardingStateService

**Problème résolu :** L'état de session utilisateur et l'étape d'onboarding doivent être accessibles et cohérents depuis plusieurs ViewModels simultanément.

**Implémentation :** `AuthStateService` maintient l'état de connexion via l'énumération `AuthMode` (Guest, LoggedIn, LoggedOut) et expose l'utilisateur courant. `OnboardingStateService` gère un processus en 4 étapes avec les méthodes `NextStep()`, `PreviousStep()` et `Reset()`.

### 4.7 Observateur — INotifyPropertyChanged

**Problème résolu :** Les vues XAML doivent se mettre à jour automatiquement lorsque les propriétés des ViewModels changent, sans couplage direct entre les couches.

**Implémentation :** Tous les ViewModels implémentent `INotifyPropertyChanged`. Le moteur de liaison de données MAUI s'abonne à `PropertyChanged` et met à jour les contrôles visuels. Dans `EventViewModel`, la modification de `IsFavorite` déclenche une notification supplémentaire pour `FavoriteIcon` :

```csharp
set
{
    if (_isFavorite == value) return;
    _isFavorite = value;
    OnPropertyChanged();
    OnPropertyChanged(nameof(FavoriteIcon));
}
```

### 4.8 Référentiel — SqliteFavoriteRepository et SqliteTicketRepository

**Problème résolu :** Les opérations de persistance sur les favoris et les billets doivent être isolées derrière des interfaces pour faciliter les tests et le remplacement de la source de données.

**Implémentation :** `SqliteFavoriteRepository` implémente `IFavoriteRepository` avec `AddAsync`, `RemoveAsync`, `GetAllAsync` et `IsFavoriteAsync`. `SqliteTicketRepository` implémente `ITicketRepository` avec `AddAsync` et `GetByUserAsync`.

### Résumé

| Patron | Classe principale | Categorie GOF |
|---|---|---|
| Décorateur | CachingEventAdapter | Structurel |
| Fabrique | EventFactoryRegistry | Création |
| Commande | FavoriteCommandInvoker | Comportemental |
| Adaptateur | SqliteEventAdapter | Structurel |
| Facade | SqliteService | Structurel |
| État | AuthStateService | Comportemental |
| Observateur | INotifyPropertyChanged | Comportemental |
| Référentiel | SqliteFavoriteRepository | Architectural |

---

## 5. Tests unitaires

L'application compte 27 tests unitaires répartis en trois suites, tous exécutés en environ 200 millisecondes. Les tests utilisent xUnit comme framework de test et NSubstitute pour simuler les dépendances, sans aucun accès réel à SQLite.

### 5.1 CachingEventAdapterTests (4 tests)

Cette suite valide le comportement du patron Décorateur.

| Test | Ce qui est vérifié |
|---|---|
| `GetAllAsync_ShouldCall_Inner_OnCacheMiss` | Un premier appel délègue à l'adaptateur interne |
| `GetAllAsync_ShouldNotCall_Inner_OnCacheHit` | Un second appel identique ne contacte pas l'interne |
| `GetAllAsync_ShouldCall_Inner_AfterWrite` | Une écriture invalide le cache, forçant un nouvel appel |
| `GetByCategoryAsync_ShouldCache_PerCategory` | Les clés de cache sont distinctes par paramètre |

### 5.2 EventFactoryTests (12 tests)

Cette suite valide le patron Fabrique. Le test paramétré `[Theory]` génère 8 cas de test, un par fabrique.

| Test | Ce qui est vérifié |
|---|---|
| `Registry_ShouldReturn_EightFactories` | Le registre retourne exactement 8 fabriques |
| `Registry_AllFactories_ShouldProduce_AtLeastOneEvent` | Aucune fabrique ne retourne une liste vide |
| `ConcertEventFactory_ShouldProduce_OnlyConcertEvents` | La fabrique produit uniquement sa propre catégorie |
| `ConcertEventFactory_ShouldProduce_EventsWithUniqueIds` | Chaque événement a un Guid unique |
| `EachFactory_ShouldProduce_CorrectCategory` [Theory x8] | Toutes les fabriques respectent leur categorie |

### 5.3 FavoriteCommandTests (10 tests)

Cette suite valide le patron Commande avec un `IFavoriteRepository` mocké via NSubstitute.

| Test | Ce qui est vérifié |
|---|---|
| `AddFavoriteCommand_Execute_ShouldCall_AddAsync` | Execute appelle repo.AddAsync avec le bon événement |
| `AddFavoriteCommand_Undo_ShouldCall_RemoveAsync` | Undo appelle repo.RemoveAsync avec le bon Id |
| `RemoveFavoriteCommand_Execute_ShouldCall_RemoveAsync` | Execute appelle repo.RemoveAsync avec le bon Id |
| `RemoveFavoriteCommand_Undo_ShouldCall_AddAsync` | Undo appelle repo.AddAsync avec le bon événement |
| `Invoker_ExecuteAsync_ShouldCall_Execute` | L'invocateur délègue à command.Execute |
| `Invoker_UndoLastAsync_ShouldCall_Undo` | L'invocateur délègue à command.Undo |
| `Invoker_UndoLastAsync_ShouldRespect_LIFOOrder` | La dernière commande exécutée est la première annulée |
| `Invoker_UndoLastAsync_ShouldNotThrow_WhenEmpty` | Aucune exception si l'historique est vide |
| `Invoker_CanUndo_ShouldBeFalse_WhenEmpty` | CanUndo est false au démarrage |
| `Invoker_CanUndo_ShouldBeTrue_AfterExecute` | CanUndo est true après une exécution |

---

## 6. Répartition des tâches

| Composant | Pierre-Sylvestre Cypré | Aboubacar Sidiki Doumbouya |
|---|---|---|
| IEventFactory + 8 fabriques concrètes | X | |
| EventFactoryRegistry | X | |
| CachingEventAdapter (Décorateur) | X | |
| MauiProgram.cs (configuration DI) | X | |
| CityPickerViewModel (géolocalisation) | X | |
| EditEventViewModel | X | |
| Tests unitaires : Décorateur et Fabrique | X | |
| IEventAdapter + SqliteEventAdapter | | X |
| IFavoriteCommand, AddFavoriteCommand, RemoveFavoriteCommand | | X |
| FavoriteCommandInvoker | | X |
| FavoritesViewModel | | X |
| Tests unitaires : Commande | | X |

Les autres composants (modèles, pages XAML, ViewModels restants, services d'authentification) ont été développés en collaboration.

---

## 7. Auto-évaluation et réflexion sur l'avenir

### Ce qui a bien fonctionné

L'adoption du patron MVVM dès le début du projet a structuré le développement de façon claire. La séparation des responsabilités entre les couches a facilité le travail en parallèle : un coéquipier pouvait travailler sur les services pendant que l'autre travaillait sur les ViewModels correspondants.

Le patron Décorateur s'est révélé particulièrement adapté au contexte : ajouter le cache n'a nécessité aucune modification des classes existantes, ce qui illustre concrètement le principe ouvert/fermé. De la même façon, le patron Commande a rendu l'annulation des favoris simple à implémenter et à tester, en isolant chaque opération dans une classe dédiée.

Le système d'injection de dépendances natif de MAUI a simplifié la gestion des instances et rendu les dépendances explicites et testables.

### Difficultés rencontrées

La gestion de l'état de navigation entre les pages a représenté un défi, notamment pour transmettre des identifiants d'événements entre les pages via `QueryProperty`. La synchronisation de l'état des favoris entre `HomePage` et `FavoritesPage` a également nécessité une attention particulière, car les deux pages maintiennent leurs propres collections.

La compatibilité cross-platform de certaines fonctionnalités, notamment la géolocalisation dans `CityPickerViewModel`, a exigé des ajustements spécifiques à chaque plateforme et des tests sur plusieurs environnements.

### Pistes d'amélioration

Plusieurs améliorations pourraient être apportées dans une version future :

- Ajouter un délai d'expiration (TTL) au cache du `CachingEventAdapter` pour les scénarios avec une source de données distante, où la garantie que toutes les écritures passent par le même décorateur ne tient plus
- Remplacer SQLite par une API REST avec base de données distante pour permettre le partage d'événements entre utilisateurs
- Implémenter un système de notifications push pour alerter les utilisateurs des événements à venir
- Ajouter une authentification via des fournisseurs tiers (Google, Apple)
- Augmenter la couverture de tests en ajoutant des tests pour les ViewModels et des tests d'intégration couvrant le flux complet de données

### Réflexion générale

Ce projet a permis d'appliquer concrètement plusieurs patrons de conception GOF dans un contexte applicatif réel. La mise en pratique de ces patrons a mis en évidence leur valeur réelle : non pas comme exercices théoriques, mais comme outils qui simplifient l'évolution et la maintenance du code. La combinaison du patron Décorateur pour le cache, du patron Commande pour l'annulation et du patron Fabrique pour la génération de données démontre comment des patrons distincts peuvent coexister dans une même application sans interférer, chacun répondant à un problème précis.
