
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

```bash
docker ps
```

---

## 3. Appliquer les migrations

```bash
cd EventGoAPI
dotnet ef database update
```

> Si `dotnet ef` n'est pas installe :
> ```bash
> dotnet tool install --global dotnet-ef
> ```

### Via terminal

## 4. Lancer le backend (API)

```bash
cd EventGoAPI
dotnet run
```

L'API sera disponible sur `http://localhost:5121`. Swagger est accessible à `http://localhost:5121/swagger` en mode développement.

---

## 5. Lancer le frontend (MAUI)

Dans un **nouveau terminal** :

```bash
cd EventGoApp
dotnet build
dotnet run
```
%LOCALAPPDATA%\EventGoApp\eventgo.db
```

Ou ouvrir `EventGo.sln` dans Visual Studio et lancer le projet `EventGoApp`.

> **Note Android** : L'émulateur Android utilise `10.0.2.2` au lieu de `localhost` pour accéder à la machine hôte. Ce changement est déjà géré automatiquement dans le code.

### Parcours utilisateur complet

1. La page de bienvenue s'affiche au lancement
2. Cliquer sur **Se connecter** et utiliser les identifiants de démonstration :
   - Courriel : `demo@eventgo.ca`
   - Mot de passe : `Demo1234`
3. Cliquer sur **Créer un compte** pour tester l'inscription avec un nouveau courriel
4. Cliquer sur **Continuer en tant qu'invité** pour accéder sans compte
5. Compléter les 4 étapes d'onboarding (ville, catégories, mode social, budget)
6. Naviguer sur la page d'accueil et utiliser les filtres par catégorie

## 6. Tester

1. Ouvrir l'app MAUI - la page de bienvenue s'affiche
2. Cliquer sur **Créer un compte** - remplir le formulaire
3. Vérifier dans la base de données :
   ```bash
   docker exec -it eventgo-mysql mysql -u root -p eventgo
   ```
   ```sql
   SELECT * FROM users;
   ```  
4. Se déconnecter - se reconnecter avec le compte créé

---

## Notes de développement

```
code/
├── EventGo.sln              <- solution unique pour les deux projets
├── EventGoAPI/
│   ├── docker-compose.yml   <- configuration MySQL
│   └── EventGoAPI/          <- projet ASP.NET Core (backend)
│       ├── Controllers/
│       ├── Services/
│       ├── Models/
│       ├── DTOs/
│       ├── Data/
│       └── Program.cs
└── EventGoApp/              <- projet .NET MAUI (frontend)
    ├── Views/
    ├── Services/
    ├── Models/
    └── MauiProgram.cs
```
