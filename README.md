# EventGo

Application mobile de découverte d'événements construite avec **.NET MAUI 9 / C#**, ciblant Android, iOS, macOS et Windows.

## Aperçu

EventGo permet aux utilisateurs de découvrir et gérer des événements locaux dans plusieurs villes du Québec. De l'inscription jusqu'à l'achat de billets, l'application offre une expérience complète de découverte d'événements culturels, sportifs et sociaux.

## Fonctionnalités

- 🔐 **Authentification** : Inscription, connexion et mode invité
- 🎯 **Onboarding** : Personnalisation par ville, catégories et budget
- 🔍 **Découverte** : Parcourir, rechercher et filtrer des événements
- ❤️ **Favoris** : Sauvegarder des événements avec possibilité d'annulation
- 🎟️ **Billets** : Achat de billets avec calcul des taxes (TPS + TVQ)
- 🛠️ **Gestion** : Créer, modifier et supprimer ses propres événements
- 👤 **Profil** : Modifier ses préférences et catégories favorites

## Technologies

| Technologie | Rôle |
|---|---|
| .NET MAUI 9 | Framework cross-platform |
| SQLite | Base de données locale |
| BCrypt | Hachage des mots de passe |
| xUnit + NSubstitute | Tests unitaires |

## Démarrage

### Prérequis

- [Visual Studio 2022](https://visualstudio.microsoft.com/) avec la charge de travail **.NET MAUI** installée
- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- Un émulateur Android / iOS ou un appareil physique connecté

### Installation

```bash
# Cloner le dépôt
git clone https://github.com/votre-utilisateur/eventgo.git
cd eventgo
```

### Lancer l'application

**Via Visual Studio :**

1. Ouvrir `code/EventGo.sln`
2. Sélectionner la plateforme cible (Android, iOS, Windows) dans la barre d'outils
3. Choisir un émulateur ou un appareil connecté
4. Appuyer sur `F5` pour démarrer

**Via ligne de commande :**

```bash
cd code/EventGoApp

# Android
dotnet build -f net9.0-android
dotnet run -f net9.0-android

# Windows
dotnet build -f net9.0-windows10.0.19041.0
dotnet run -f net9.0-windows10.0.19041.0
```

## Compte de démonstration

Une fois l'application lancée, utiliser les identifiants suivants pour se connecter directement :

```
Email    : demo@eventgo.ca
Mot de passe : Demo1234
```

## Données de démonstration

- **21 événements** répartis sur 8 catégories et 7 villes québécoises
- **Compte démo** : `demo@eventgo.ca` / `Demo1234`

## Plateformes cibles

Android · iOS · macOS · Windows
