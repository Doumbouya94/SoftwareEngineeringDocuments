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
- **FR-8 :** Le système doit permettre à un utilisateur non authentifié (mode invité) d'accéder à la page Explore et de consulter les détails d'un événement, après avoir uniquement sélectionné une localisation. Toute action nécessitant un compte (ajouter aux favoris, obtenir un billet, accéder aux onglets Tickets et Profil) doit rediriger vers l'écran de connexion/inscription avec un message explicite, sans modifier l'état de l'application.
- **FR-9 :** Le système doit guider tout nouvel utilisateur inscrit à travers un flux d'intégration de 5 étapes (nom/âge, localisation, intérêts, notifications, objectifs) avant d'accéder à l'application. La sélection d'au moins une catégorie d'intérêts est obligatoire pour passer à l'étape suivante. La progression est vérifiable via un LinearProgressIndicator évoluant de 20 % à 100 % entre l'étape 1 et l'étape 5. Une fois complété, le flux ne doit plus s'afficher au relancement de l'application.
- **FR-10 :** Le système doit permettre à un utilisateur authentifié d'obtenir un billet pour un événement interne depuis la page de détail. Le billet généré doit apparaître dans l'onglet "Upcoming" de la page Tickets et comporter : un numéro unique, le nom du détenteur, la date/heure et le lieu de l'événement, et le type de billet. Le comportement de billetterie selon la source de l'événement (interne vs. externe) est défini en FR-15.
- **FR-11 :** Le système doit classer automatiquement les billets entre les onglets "Upcoming" (date de l'événement > date courante) et "Past" (date de l'événement ≤ date courante). Les billets "Past" doivent être affichés avec une opacité réduite (≤ 0.6) et proposer un bouton "Leave Review" à la place de "Get Directions".
- **FR-12 :** Le système doit permettre à un utilisateur possédant un billet dans l'onglet "Past" de soumettre un avis composé d'une note (1 à 5 étoiles, obligatoire) et d'un commentaire textuel (optionnel, ≤ 500 caractères). Un utilisateur ne peut soumettre qu'un seul avis par événement ; toute tentative supplémentaire affiche le message "Vous avez déjà noté cet événement". Les avis soumis sont visibles sur la page de détail de l'événement concerné.
- **FR-13 :** Le système doit afficher des suggestions d'événements dans la barre de recherche à partir de 2 caractères saisis, avec un debounce de 300 ms maximum. Les suggestions doivent inclure le titre et la localisation de l'événement. La recherche doit être insensible à la casse et supporter la correspondance partielle (ex. : "tech" retourne les événements contenant "tech", "Tech" ou "TECH" dans le titre ou la catégorie).
- **FR-14 :** Le système doit permettre à l'utilisateur de sélectionner un thème d'affichage parmi Clair, Sombre et Système depuis Profil > Préférences. Le thème doit être appliqué immédiatement à tous les écrans sans redémarrage et persisté entre les sessions. La préférence d'unité de distance (km / miles) doit se répercuter sur tous les affichages de distance de l'application (filtres, cartes d'événements, rayon de recherche) sans délai.
- **FR-15 :**  Le système doit distinguer deux types d'événements via un champ source dans le modèle de données : les événements internes (créés par un Organisateur) et les événements externes (importés via les API Ticketmaster, Eventbrite, etc.). Pour un événement externe, le bouton "Get Tickets" doit ouvrir le lien de billetterie officiel de la plateforme d'origine dans le navigateur natif (url_launcher), sans générer de billet interne. Pour un événement interne, le flux d'achat in-app génère un billet avec QR code (via QrImageView) accessible dans l'onglet Tickets. Ce comportement est vérifiable : appuyer sur "Get Tickets" d'un événement externe ouvre un lien externe et n'ajoute aucun billet à la page Tickets, tandis que la même action sur un événement interne crée un billet avec QR code dans le bottom sheet "View Ticket".

---

## 5. Exigences non fonctionnelles (NFR)
- **NFR-1 (Performance) :** La liste des événements doit se charger en moins de 2 secondes sur un réseau 4G, avec une pagination limitée à 20 résultats par page. Les appels API de recherche doivent retourner une réponse en moins de 500 ms. Au-delà de 3 secondes, une erreur de timeout doit être déclenchée. Les listes d'événements doivent être mises en cache côté client avec un TTL de 5 minutes afin de réduire les appels réseau redondants. Les images doivent être chargées en lazy loading et afficher un placeholder animé (ex. shimmer) pendant leur chargement.
- **NFR-2 (Sécurité) :** Tous les échanges entre le client et le serveur doivent transiter via HTTPS (TLS 1.2 minimum). L'accès aux endpoints protégés (favoris, gestion d'événements, profil) est restreint aux requêtes portant un token JWT valide. Les access tokens expirent après 24h ; un refresh token d'une validité de 7 jours permet leur renouvellement sans reconnexion. Les mots de passe doivent être hachés avec bcrypt (facteur de coût ≥ 12). Les entrées utilisateur doivent être validées côté serveur afin de prévenir les injections SQL et XSS. Un rate limiting de 100 requêtes/minute par utilisateur authentifié et de 20 requêtes/minute par IP non authentifiée doit être appliqué.
- **NFR-3 (UX) :** L'utilisateur doit pouvoir trouver et consulter un événement en 3 interactions ou moins depuis l'écran d'accueil. Le temps de réponse perçu lors de la navigation entre écrans doit être inférieur à 300 ms. Tout contenu chargé de manière asynchrone (listes, images, profil) doit afficher un état de chargement skeleton (Shimmer) pendant la récupération des données. Chaque écran susceptible de retourner une liste vide doit afficher un état vide explicite (icône + titre + description + action). Les messages d'erreur doivent être rédigés en langage clair, indiquer la cause probable et proposer une action corrective (ex. : "Impossible de charger les événements — Réessayer"). En mode hors ligne, l'application doit afficher une bannière indiquant l'absence de connexion et continuer à afficher les données mises en cache.
- **NFR-4 (Qualité) :** La couverture minimale de tests unitaires est fixée à 80 % sur les services et contrôleurs critiques (Auth, Events, Favorites, Tickets). Chaque endpoint exposé via Swagger doit avoir au moins un test d'intégration couvrant le cas nominal et un cas d'erreur.
- **NFR-5 (Disponibilité & Résilience) :** Le backend doit garantir un taux de disponibilité de 99,5 % (hors fenêtres de maintenance planifiée, annoncées 24h à l'avance). L'indisponibilité d'une API externe (Ticketmaster, Eventbrite) ne doit pas provoquer de crash de l'application : le système doit basculer vers les données en cache ou afficher un message d'indisponibilité partielle sans bloquer la navigation. Tout appel vers une API externe doit être soumis à un timeout de 3 secondes et à un mécanisme de retry limité à 2 tentatives.
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
