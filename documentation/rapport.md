# Résumé des changements — SRS v1 → v2

## Métadonnées & identification
- Les placeholders (`À compléter`) ont été remplacés par les vrais noms de l'équipe : **Pierre-Sylvestre Cypré, Aboubacar Doumbouya Sidiki**
- La version est passée de `v0.1` à `Pre-Alpha` ✅ *(corrige : placeholders non finalisés)*

## Portée (Scope)
- Ajout de **OUT-5** : exclusion explicite des APIs externes (Ticketmaster, Eventbrite) ✅ *(corrige : incohérence OUT-5 vs dépendances)*

## Exigences fonctionnelles (FR)
Les FR sont passées de 7 à **15 exigences**, avec des critères précis et testables :
- **FR-1** : validation du mot de passe (≥ 8 caractères, 1 majuscule, 1 chiffre), JWT, erreur 409 sur doublon ✅
- **FR-2** : 5 critères de filtre combinables (localisation, rayon, prix, dates, catégorie), tri des résultats ✅ *(corrige : filtres trop généraux)*
- **FR-3** : si géolocalisation refusée → saisie manuelle d'adresse ✅ *(corrige : comportement si refus géolocalisation)*
- **FR-4** : format d'événement défini (titre, description, catégorie, date, lieu, prix) + règles de modification ✅ *(corrige : format et validation d'un événement)*
- **FR-5 à FR-15** : nouveaux flux — billetterie, avis, onboarding, thème, mode invité, recherche avec suggestions, événements internes/externes

## Exigences non fonctionnelles (NFR)
Passées de vagues à **précises et mesurables** :
- **Performance** : chargement < 2 s sur 4G, pagination 20 résultats, cache TTL 5 min, lazy loading
- **Sécurité** : HTTPS/TLS 1.2, JWT expirant après 24h, bcrypt ≥ 12, rate limiting
- **Qualité** : couverture de tests unitaires fixée à **80 %** sur les services critiques
✅ *(corrige : FR/NFR trop généraux, manque de critères mesurables)*

## Contraintes
- Technologie précisée : **ASP.NET Core + MAUI.NET + MySQL** (remplace Java/Spring Boot)
- Dates de phases ajoutées explicitement

## Dépendances
- Mises à jour vers les outils réels : **Azure Notification Hubs**, **MAUI Geolocation**, **ASP.NET Core Authentication**
- Ticketmaster/Eventbrite retirés des dépendances (cohérent avec OUT-5) ✅
Voilà les explications rapides des trois diagrammes pour EventGo :

## Diagramme de cas d'utilisation (Use Case Diagram)
Ce diagramme montre qui fait quoi dans l'app. On a deux acteurs : l'Utilisateur et l'Admin.  
L'utilisateur peut :  
- Explorer et rechercher des événements autour de lui (géoloc + filtres)  
- Voir les détails d'un événement
- Booker un billet — et là, ça inclut automatiquement le paiement (la flèche «include» veut dire que c'est obligatoire)
- Voir ses billets, sauvegarder des favoris, gérer son profil
- L'admin, lui, peut :
- Gérer les événements (modération)
- Voir les statistiques de la plateforme  

## Diagramme de composants (Component Diagram)
Ce diagramme montre comment l'app est structurée. C'est l'architecture technique complète :  
- À gauche : le client (.NET MAUI mobile/web) qui envoie des requêtes HTTP/REST avec un token JWT
- Il passe par un Load Balancer / API Gateway
- Ensuite les Controllers (ASP.NET Core) reçoivent les requêtes : Auth, Tickets, Events, Favorites, Notifications…
- Chaque controller parle à un Service (business logic) correspondant
- Les services accèdent à la base de données via Entity Framework + MySQL (Users, Events, Tickets, Categories, etc.)
- Un Cache Redis stocke les événements temporairement pour éviter d'aller chercher les mêmes données tout le temps (TTL 10-30 min)
- Les APIs externes (Ticketmaster, Eventbrite) sont chargées en lazy loading seulement quand nécessaire
- Pour les notifications, ça passe par Azure Notification Hubs  

## Diagramme de séquence (Sequence Diagram)
Ce diagramme montre les étapes dans le temps pour les principales actions d'un utilisateur. On voit l'interaction entre : l'utilisateur, l'app mobile, l'API backend, la base de données, le service de géolocalisation et les notifications.  
Les grands scénarios couverts :  
1. Inscription — l'utilisateur entre ses infos, le backend vérifie que le courriel est unique, crée le compte et retourne un token JWT. Si le courriel existe déjà → erreur affichée.  
2. Connexion — entrée courriel/mdp, vérification des identifiants, retour des tokens d'accès et de rafraîchissement.  
3. Explorer des événements — l'app récupère les coordonnées GPS, le backend filtre les événements par coordonnées et retourne une liste paginée (20/page).  
4. Recherche — requête avec debounce de 300ms, recherche insensible à la casse, suggestions en temps réel.  
5. Favoris — ajout d'un événement aux favoris, confirmation en base de données.  
6. Obtenir un billet — création d'un billet avec numéro unique + QR code.  
7. Notifications push — quand un nouvel événement est publié, Azure envoie une notif aux utilisateurs abonnés.

## Composant implémenté
Le module d'authentification (AuthService) est un composant fonctionnel complet, couvrant l'inscription (RegisterAsync), la connexion (LoginAsync) et la génération de tokens JWT (GenerateToken), exposés via des endpoints REST dans AuthController.

## Patron de conception
Trois patrons de conception sont présents dans l'implémentation. Le patron Adaptateur est le plus visible : la classe ApplicationUser, qui hérite de IdentityUser<Guid>, adapte le modèle d'Identity d'SASP.NET aux besoins spécifiques du schéma users de EventGo, en ajoutant des champs comme FullName, IsGuest et CreatedAt, et en remappant les colonnes dans OnModelCreating. Le patron Factory est illustré par la méthode GenerateToken, qui centralise la création d'un objet AuthResponse (incluant le token JWT) à partir d'un utilisateur. Enfin, le patron Singleton est présent de façon indirecte via IConfiguration, fourni par le framework comme instance unique partagée à travers l'application et injecté dans AuthService.

## Principes de développement
L'architecture respecte la séparation des responsabilités : AuthController gère uniquement la couche HTTP, AuthService contient la logique métier, AppDbContext gère la persistance, et les DTOs (RegisterRequest, LoginRequest, AuthResponse) isolent le contrat de l'API du modèle de domaine. La structure en dossiers (Controllers/, Services/, Models/, DTOs/, Data/) renforce la clarté et la lisibilité du projet.
