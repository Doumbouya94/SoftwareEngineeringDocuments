### Description

EventGo est une application orientée mobile (avec une API backend) qui permet aux utilisateurs de **découvrir des événements publics autour d’eux** grâce à la **géolocalisation**, avec des filtres et des recommandations personnalisées.

### Ce qui est déjà implémenté

- **Squelette d’API backend** en [ASP.NET](http://ASP.NET) Core Web API.
- **Authentification** : c’est le principal focus en ce moment.
    - Modèle utilisateur basé sur Identity, mappé sur la table `users`.
    - Génération d’un JWT lors du **register** et du **login**.
    - Swagger activé en environnement de développement pour tester.
- **Couche base de données** branchée avec Entity Framework Core (MySQL).
    - Migrations initiales et workflow `dotnet ef database update` en place.

### Endpoints disponibles

- `POST /api/auth/register`
- `POST /api/auth/login`

### Ce qui n’est pas encore fait

- **Intégration du client .NET MAUI** (UI, appels à l’API, stockage du JWT, ajout du JWT aux requêtes).
- Endpoints protégés pour les fonctionnalités (Discovery, Geolocation, etc.) au-delà du socle d’auth.
- Modèles métier côté événements (événements, catégories, favoris, recommandations) et persistance associée.

### Exécution en local (workflow actuel)

1. Configurer `ConnectionStrings:Default` et les paramètres `Jwt` dans `appsettings.json`.
2. Exécuter les migrations :
    - `dotnet ef migrations add <Name>` (si nécessaire)
    - `dotnet ef database update`
3. Démarrer l’API :
    - `dotnet run`
4. Tester dans Swagger ou via cURL avec les endpoints d’auth.

**Progrès actuel :** endpoints d’authentification fonctionnels de bout en bout côté API (register + login + JWT).
