# Cahier des charges (SRS léger) — EventGo
**Équipe :** Pierre-Sylvestre Cypré, Aboubacar Doumbouya Sidiki

**Date :** 2026-01-23  
**Version :** Pre-Alpha

---

## 1. Contexte & objectif
- **Contexte :** Les jeunes adultes, touristes et nouveaux immigrants veulent sortir et découvrir des activités, mais les informations sont dispersées sur plusieurs plateformes (Instagram, TikTok, Ticketmaster, etc.), rendant difficile la découverte complète et la planification de sorties.
- **Objectif principal :** Centraliser la découverte d'événements locaux sur une plateforme unique et accessible, permettant de trouver facilement des événements publics autour de soi grâce à la géolocalisation, des filtres et des recommandations personnalisées.
- **Parties prenantes :** Utilisateurs normaux (cherchant des événements), organisateurs d'événements, administrateurs (modération et statistiques)
---

## 2. Portée (Scope)
### 2.1 Inclus (IN)
- IN-1 : Inscription et connexion des utilisateurs
- IN-2 : Liste, recherche et filtrage d'événements par rayon/localisation, prix, date et catégorie
- IN-3 : Géolocalisation pour trouver des événements autour de l'utilisateur
- IN-4 : Système de favoris pour sauvegarder des événements
- IN-5 : Notifications pour les événements
- IN-6 : Ajout, modification et suppression d'événements par les organisateurs
- IN-7 : Modération de contenu et consultation de statistiques par les administrateurs

### 2.2 Exclu (OUT)
- OUT-1 : Achat de billets directement dans l'application (phase 1)
- OUT-2 : Intégration de paiement en ligne
- OUT-3 : Messagerie entre utilisateurs
- OUT-4 : Fonctionnalités de réseau social avancées (partage, commentaires publics)
- OUT-5 : Utilisation d'API (Ticketmaster, Eventbrite)

---

## 3. Acteurs / profils utilisateurs
- **Utilisateur normal:** Cherche et découvre des événements locaux, utilise les filtres et la géolocalisation, ajoute des événements à ses favoris, reçoit des notifications.
- **Organisateur :** Crée, modifie et supprime des événements qu'il organise. Possède un compte avec des permissions spécifiques.
- **Administrateur :** Modère le contenu des événements, gère les utilisateurs et organisateurs, consulte les statistiques d'utilisation de la plateforme.

---

## 4. Exigences fonctionnelles (FR)
- **FR-1 :** Le système doit permettre aux utilisateurs de créer un compte (nom, courriel, mot de passe ≥ 8 caractères avec au moins 1 majuscule et 1 chiffre) et de se connecter via JWT. Un courriel unique est requis ; toute tentative de doublon retourne une erreur 409 Conflict.
- **FR-2 :** Le système doit permettre de lister, rechercher et filtrer des événements selon au moins 5 critères combinables : localisation (ville ou coordonnées GPS), rayon (ex. 5, 10, 25 km), fourchette de prix, plage de dates (date début / date fin), et catégorie. Les résultats doivent être triables par pertinence, date ou distance.
- **FR-3 :** Le système doit utiliser les coordonnées GPS de l'utilisateur (avec consentement explicite) pour afficher les événements dans un rayon configurable (défaut : 10 km). Si la permission est refusée, l'utilisateur doit pouvoir saisir manuellement une adresse.
- **FR-4 :** Le système doit permettre aux utilisateurs avec le rôle Organisateur de créer un événement (titre, description, catégorie, date, lieu, prix), de le modifier, et de le supprimer (avec confirmation). Un événement ne peut être modifié que par son créateur ou un administrateur.
- **FR-5 :** Le système doit permettre aux utilisateurs authentifiés d'ajouter ou retirer un événement de leurs favoris (toggle). La liste des favoris doit être persistante et accessible hors ligne (cache local).
- **FR-6 :** Le système doit envoyer une notification push aux utilisateurs abonnés à une catégorie ou un rayon géographique lorsqu'un nouvel événement correspondant est publié, dans un délai maximum de 5 minutes après la publication.
- **FR-7 :** Le système doit permettre aux utilisateurs avec le rôle Admin de signaler/masquer un événement, de bannir un compte, et de consulter un tableau de bord affichant au minimum : le nombre d'événements actifs, d'utilisateurs inscrits, et d'événements signalés.

---

## 5. Exigences non fonctionnelles (NFR)
- **NFR-1 (Performance) :** La liste des événements doit se charger en moins de 2 secondes pour une requête avec filtres sur un réseau 4G. Les appels API de recherche doivent retourner une réponse en moins de 500 ms.
- **NFR-2 (Sécurité) :** L'accès aux endpoints de favoris et de gestion d'événements est restreint aux utilisateurs authentifiés (token JWT requis). Les mots de passe doivent être hachés avec bcrypt. Les tokens doivent expirer après 24h.
- **NFR-3 (UX) :** L'utilisateur doit pouvoir trouver et consulter un événement en 3 interactions ou moins depuis l'écran d'accueil. Le temps de réponse perçu lors de la navigation doit être inférieur à 300 ms.
- **NFR-4 (Qualité) :** La couverture minimale de tests unitaires est fixée à 80 % sur les services et contrôleurs critiques (Auth, Events, Favorites). Chaque endpoint exposé via Swagger doit avoir au moins un test d'intégration couvrant le cas nominal et un cas d'erreur.

---

## 6. Contraintes
- **C-1 (Technologies) :** ASP.NET Core + MAUI.NET + MySQL
- **C-2 (Plateforme) :** Mobile/Web
- **C-3 (Délai) :** 25 janvier 2026 (Phase 1), 22 fevrier 2026 (Phase 2), 22 mars 2026 (Phase 3), 23 au 19 avril (Phase 4)
- **C-4 (Outils) :** Visual Studio, Git, Notion (Asana si nécessaire)

---

## 7. Données & règles métier (si applicable)
- **Entités principales :** User (Utilisateur, Organisateur, Administrateur), Event (Événement), Favorite (Favori), Notification, Category (Catégorie).
- **Règles métier :** Un organisateur ne peut modifier que ses propres événements. Les événements doivent avoir une date future. Les administrateurs peuvent modérer tout le contenu. La géolocalisation est basée sur les coordonnées GPS de l'utilisateur. L'application doit demander l'autorisation d'accéder à la localisation de l'utilisateur lors de la première utilisation des fonctionnalités de géolocalisation.

---

## 8. Hypothèses & dépendances
### 8.1 Hypothèses
- H-1 : Les utilisateurs ont un smartphone avec accès à Internet et GPS activé.
- H-2 : Les organisateurs fournissent des informations exactes et complètes sur les événements.
- H-3 : Les utilisateurs acceptent de partager leur localisation pour utiliser les fonctionnalités de géolocalisation.

### 8.2 Dépendances
- D-1 : Service de géolocalisation (Comme Geolocation dans MAUI)
- D-2 : Service de notifications push (Azure Notification Hubs)
- D-3 : Service d'autentification (ASP.NET Core authentication)

---

## 9. Critères d’acceptation globaux (Definition of Done – mini)
- [ ] Fonctionnalités livrées et testées
- [ ] Tests unitaires présents
- [ ] Gestion d’erreurs minimale
- [ ] Documentation à jour (UML + ADR si requis)
