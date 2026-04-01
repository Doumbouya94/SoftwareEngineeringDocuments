# Guide de démarrage - EventGo

## Prérequis

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- Visual Studio 2022 avec la charge de travail **.NET MAUI**

---

## 1. Cloner le dépôt

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

Les paquets NuGet (sqlite-net-pcl, BCrypt.Net-Next, Microsoft.Maui.Controls) seront téléchargés automatiquement.

---

## 3. Lancer l'application

**Via Visual Studio :**
Ouvrir `EventGo.sln`, sélectionner la plateforme cible (Windows, Android, iOS), puis appuyer sur **F5**.

**Via le terminal (Windows) :**
```bash
dotnet build -f net9.0-windows10.0.19041.0
dotnet run -f net9.0-windows10.0.19041.0
```

**Via le terminal (Android) :**
```bash
dotnet build -f net9.0-android
dotnet run -f net9.0-android
```

> La base de données SQLite est créée automatiquement au premier lancement.

---

## 4. Premiers pas

1. La page de bienvenue s'affiche au lancement.
2. Créer un compte via **S'inscrire**, ou se connecter avec un compte existant.
3. Compléter l'onboarding (ville, catégories, budget).
4. Explorer les événements depuis la page d'accueil.
