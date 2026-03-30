# Guide de démarrage - EventGo

## Prérequis

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- Visual Studio 2022 ou Visual Studio Code avec l'extension MAUI
- Windows 10 version 19041 ou supérieur (pour le déploiement Windows)

---

## 1. Cloner le projet

```bash
git clone <url-du-repo>
cd SoftwareEngineeringDocuments/code
```

---

## 2. Restaurer les dépendances

```bash
cd EventGoApp
dotnet restore
```

Les paquets NuGet suivants seront téléchargés automatiquement :

- `sqlite-net-pcl` -- accès SQLite asynchrone
- `BCrypt.Net-Next` -- hachage sécurisé des mots de passe
- `Microsoft.Maui.Controls` -- framework MAUI

> Aucune base de données externe n'est requise. La base de données SQLite est créée automatiquement au premier lancement dans le répertoire de données de l'application.

---

## 3. Lancer l'application MAUI

```bash
cd EventGoApp
dotnet build
dotnet run -f net9.0-windows10.0.19041.0
```

Ou ouvrir `EventGo.sln` dans Visual Studio et lancer le projet `EventGoApp` avec le profil Windows.

---

## 4. Compte de démonstration

Un utilisateur de démonstration est créé automatiquement au premier lancement :

| Champ | Valeur |
|-------|--------|
| Courriel | demo@eventgo.ca |
| Mot de passe | Demo1234 |

---

## 5. Vérifier le fonctionnement

1. Ouvrir l'application -- la page de bienvenue s'affiche
2. Cliquer sur **Se connecter** et utiliser le compte de démonstration
3. La page d'accueil affiche 20 événements avec filtres par catégorie
4. Cliquer sur **S'inscrire** pour créer un nouveau compte
5. Fermer et relancer l'application -- les données persistent grâce à SQLite

---

## 6. Backend (API)

Le backend ASP.NET Core est dans `EventGoAPI/`. Il est indépendant de l'application MAUI et n'est pas nécessaire pour faire fonctionner l'application mobile.

```bash
cd EventGoAPI/EventGoAPI
dotnet run
```

L'API sera accessible sur `http://localhost:5121`. Swagger est disponible à `http://localhost:5121/swagger` en mode développement.

---

## Structure du projet

```
code/
|-- EventGo.sln                  <- solution unique pour les deux projets
|-- EventGoAPI/
|   |-- EventGoAPI/              <- projet ASP.NET Core (backend)
|       |-- Controllers/
|       |-- Services/
|       |-- Models/
|       |-- DTOs/
|       |-- Data/
|       `-- Program.cs
`-- EventGoApp/                  <- projet .NET 9 MAUI (frontend)
    |-- Models/                  <- Event, User, EventCategory
    |-- Services/                <- SqliteService, LocalAuthService, IEventAdapter...
    |-- ViewModels/              <- HomeViewModel, EventViewModel
    |-- Views/                   <- WelcomePage, LoginPage, RegisterPage, OnboardingPage, HomePage
    `-- MauiProgram.cs
```

---

## Notes de développement

- La base de données SQLite est stockée dans `FileSystem.AppDataDirectory` (dossier de données de l'application)
- Les mots de passe sont hachés avec BCrypt avant d'être enregistrés
- Pour réinitialiser la base de données, appeler `SqliteService.DropAndRecreateAsync()` en développement
- Le mode invité permet de naviguer sans créer de compte
