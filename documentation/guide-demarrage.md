
# Guide de démarrage - EventGo

## Prérequis

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- Visual Studio 2022 ou Visual Studio Code avec l'extension MAUI
- Windows 10 version 19041 ou supérieure (pour le déploiement Windows)

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

| Paquet | Version | Utilisation |
|--------|---------|-------------|
| Microsoft.Maui.Controls | 9.0.51 | Interface MAUI |
| sqlite-net-pcl | 1.9.172 | Base de données locale |
| BCrypt.Net-Next | 4.0.3 | Hachage des mots de passe |

> Aucune installation de Docker ou de base de données externe n'est requise.
> L'application utilise SQLite, une base de données locale créée automatiquement au premier lancement.

---

## 3. Compiler et lancer l'application

### Via Visual Studio

Ouvrir `EventGo.sln`, sélectionner le projet `EventGoApp`, choisir la cible **Windows Machine** et appuyer sur **F5**.

### Via terminal

```bash
cd EventGoApp
dotnet build -f net9.0-windows10.0.19041.0
dotnet run -f net9.0-windows10.0.19041.0
```

---

## 4. Premier lancement

Au premier lancement, l'application effectue automatiquement les opérations suivantes :

1. Création du fichier de base de données `eventgo.db` dans le répertoire de données de l'application
2. Création des tables `Users` et `Events`
3. Insertion d'un utilisateur de démonstration
4. Insertion de 20 événements de démonstration

L'emplacement du fichier de base de données sur Windows :
```
%LOCALAPPDATA%\EventGoApp\eventgo.db
```

---

## 5. Tester l'application

### Parcours utilisateur complet

1. La page de bienvenue s'affiche au lancement
2. Cliquer sur **Se connecter** et utiliser les identifiants de démonstration :
   - Courriel : `demo@eventgo.ca`
   - Mot de passe : `Demo1234`
3. Cliquer sur **Créer un compte** pour tester l'inscription avec un nouveau courriel
4. Cliquer sur **Continuer en tant qu'invité** pour accéder sans compte
5. Compléter les 4 étapes d'onboarding (ville, catégories, mode social, budget)
6. Naviguer sur la page d'accueil et utiliser les filtres par catégorie

### Vérification de la base de données

Ouvrir le fichier `eventgo.db` avec [DB Browser for SQLite](https://sqlitebrowser.org/) pour inspecter les tables et vérifier que les mots de passe sont bien hachés avec BCrypt.

---

## Structure du projet

```
code/
├── EventGo.sln
├── EventGoAPI/                       <- backend ASP.NET Core (non requis pour MAUI)
└── EventGoApp/                       <- application MAUI autonome
    ├── Models/
    │   ├── Event.cs                  <- modèle événement (table SQLite)
    │   ├── EventCategory.cs          <- énumération des catégories
    │   └── User.cs                   <- modèle utilisateur (table SQLite)
    ├── Services/
    │   ├── SqliteService.cs          <- façade SQLite (connexion + schéma)
    │   ├── PasswordService.cs        <- hachage BCrypt
    │   ├── LocalAuthService.cs       <- authentification via SQLite
    │   ├── IEventAdapter.cs          <- interface adaptateur événements
    │   ├── SqliteEventAdapter.cs     <- adaptateur SQLite pour les événements
    │   ├── IAuthState.cs             <- interface état d'authentification
    │   ├── AuthStateService.cs       <- gestion de l'état d'authentification
    │   └── OnboardingStateService.cs <- gestion de l'onboarding
    ├── ViewModels/
    │   ├── EventViewModel.cs         <- patron Observateur (INotifyPropertyChanged)
    │   └── HomeViewModel.cs          <- logique de la page d'accueil
    ├── Views/
    │   ├── WelcomePage.xaml
    │   ├── LoginPage.xaml
    │   ├── RegisterPage.xaml
    │   ├── OnboardingPage.xaml
    │   └── HomePage.xaml
    └── MauiProgram.cs                <- configuration de l'injection de dépendances
```

---

## Patrons de conception implémentés

| Patron | Classe | Rôle |
|--------|--------|------|
| Observateur | `EventViewModel` | Notification automatique de l'interface |
| État | `AuthStateService`, `OnboardingStateService` | Gestion des transitions d'état |
| Adaptateur | `SqliteEventAdapter` | Abstraction de la source de données |
| Façade | `SqliteService` | Simplification de l'accès SQLite |
