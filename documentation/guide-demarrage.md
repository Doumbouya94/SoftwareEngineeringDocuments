# Guide de démarrage — EventGo

## Prérequis

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- Un IDE (Visual Studio 2022+ recommande)

---

## 1. Cloner le projet

```bash
git clone <url-du-repo>
cd SoftwareEngineeringDocuments/code
```

---

## 2. Démarrer la base de données (MySQL via Docker)

```bash
cd EventGoAPI
docker-compose up -d
```

Vérifier que le conteneur est actif :

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

---

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

Ou ouvrir `EventGo.sln` dans Visual Studio et lancer le projet `EventGoApp`.

> **Note Android** : L'émulateur Android utilise `10.0.2.2` au lieu de `localhost` pour accéder à la machine hôte. Ce changement est déjà géré automatiquement dans le code.

---

## 6. Tester

1. Ouvrir l'app MAUI — la page de bienvenue s'affiche
2. Cliquer sur **Créer un compte** — remplir le formulaire
3. Vérifier dans la base de données :
   ```bash
   docker exec -it eventgo-mysql mysql -u root -p eventgo
   ```
   ```sql
   SELECT * FROM users;
   ```  
4. Se déconnecter — se reconnecter avec le compte créé

---

## Structure du projet

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
