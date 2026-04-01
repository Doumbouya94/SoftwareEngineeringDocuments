# Rapport de Phase 2 - Patrons de conception
**Projet EventGo** | Pierre-Sylvestre Cypré, Aboubacar Sidiki Doumbouya

---

## Corrections apportées suite aux commentaires

| Commentaire | Correction |
|-------------|------------|
| Diagramme de classes resemblait à un ERD | Remplacé par un vrai diagramme UML orienté objet avec méthodes, visibilité, et relations |
| Absence du frontend MAUI | Application MAUI complète : 5 pages fonctionnelles + SQLite |
| Patrons non observables dans le code | 3 patrons visibles dans le code avec documentation XML |
| Paiement dans UC mais hors portée SRS | Retiré du diagramme de cas d'utilisation |

---

## 1. Patron Observer - `EventViewModel` + Authentification

Permet à l'interface de se mettre à jour automatiquement quand les données changent, sans appels manuels de rafraîchissement.

**`EventViewModel`** implémente `INotifyPropertyChanged` :
```csharp
public bool IsSelected {
    set { _isSelected = value; OnPropertyChanged(); } // notifie le CollectionView
}
public event PropertyChangedEventHandler? PropertyChanged;
```

**`AuthStateService`** maintient l'état partagé observé par toutes les pages :
```csharp
public void SetLoggedIn(Models.User user) {
    CurrentMode = AuthMode.LoggedIn;
    CurrentUser = user;
}
```
`HomePage` lit `CurrentUser.Username` à chaque navigation — si l'état a changé, l'affichage est à jour.

---

## 2. Patron Adapter - `SqliteEventAdapter`

Découple `HomeViewModel` de SQLite. La source de données peut changer (API, JSON) sans toucher à l'interface.

```
«interface»           «adaptateur»              «adapté»
IEventAdapter ◀─── SqliteEventAdapter ──▶ SQLiteAsyncConnection
GetAllAsync()         GetAllAsync()              Table<Event>()
GetFilteredAsync()    GetFilteredAsync()         Where() / ToListAsync()
```

```csharp
// HomeViewModel ne connaît que l'interface
public HomeViewModel(IEventAdapter eventAdapter) { ... }
await _eventAdapter.GetAllAsync(); // pas de SQLite direct
```

Pour passer à une API REST : changer **une seule ligne** dans `MauiProgram.cs` :
```csharp
builder.Services.AddSingleton<IEventAdapter, ApiEventAdapter>();
```

---

## 3. Patron Facade - `SqliteService`

Cache la complexité de l'initialisation SQLite derrière deux méthodes simples.

```csharp
public async Task InitializeAsync() {
    string dbPath = Path.Combine(FileSystem.AppDataDirectory, "eventgo.db");
    _db = new SQLiteAsyncConnection(dbPath);
    await _db.CreateTableAsync<User>();
    await _db.CreateTableAsync<Event>();
}
public SQLiteAsyncConnection GetConnection() => _db ?? throw new InvalidOperationException(...);
```

Les services (`LocalAuthService`, `SqliteEventAdapter`) ont juste besoin d'appeller `GetConnection()` - ils ne savent pas où est le fichier ni comment les tables sont créées.

---

## Résumé

| # | Patron | Classe |
|---|--------|--------|
| 1 | Observer | `EventViewModel`, `AuthStateService` | 
| 2 | Adapter | `SqliteEventAdapter` / `IEventAdapter` |
| 3 | Facade | `SqliteService` | Aboubacar |
