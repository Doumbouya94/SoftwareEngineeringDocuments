# Cahier des charges (SRS léger) — <Nom du projet>
**Équipe :** <Pierre-Sylvestre Cypré, Aboubacar Doumbouya Sidiki>

**Date :** <2026-01-23>  
**Version :** <v0.1 / v1.0>

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
> Forme recommandée : “Le système doit…”
- **FR-1 :** Le système doit permettre aux utilisateurs de créer un compte et se connecter.
- **FR-2 :** Le système doit permettre aux utilisateurs de lister, rechercher et filtrer des événements par localisation, rayon, prix, date et catégorie.
- **FR-3 :** Le système doit utiliser la géolocalisation pour afficher les événements autour de l'utilisateur.
- **FR-4 :** Le système doit permettre aux organisateurs d'ajouter, modifier et supprimer des événements.
- **FR-5 :** Le système doit permettre aux utilisateurs d'ajouter des événements à leurs favoris.
- **FR-6 :** Le système doit envoyer des notifications aux utilisateurs pour les événements pertinents.
- **FR-7 :** Le système doit permettre aux administrateurs de modérer le contenu et consulter les statistiques.

---

## 5. Exigences non fonctionnelles (NFR)
> Performance / sécurité / disponibilité / UX / maintenabilité…
- **NFR-1 (Performance) :** L'authentification est requise pour accéder aux fonctionnalités de favoris et de gestion d'événements. Les mots de passe doivent être chiffrés.
- **NFR-2 (Sécurité) :** L'authentification est requise pour accéder aux fonctionnalités de favoris et de gestion d'événements. Les mots de passe doivent être chiffrés.
- **NFR-3 (UX) :** Le parcours pour trouver un événement doit nécessiter 3 clics ou moins.
- **NFR-4 (Qualité) :** Couverture minimale de tests unitaires à définir selon les standards de l'équipe.

---

## 6. Contraintes
- **C-1 (Technologie) :** C# MAUI.NET + SQLite
- **C-2 (Plateforme) :** Mobile
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
- D-1 : API Ticketmaster, Eventbrite etc...
- D-2 : Service de notifications push (locales et cloud) .NET
- D-3 : API pour la localisation

---

## 9. Critères d’acceptation globaux (Definition of Done – mini)
- [ ] Fonctionnalités livrées et testées
- [ ] Tests unitaires présents
- [ ] Gestion d’erreurs minimale
- [ ] Documentation à jour (UML + ADR si requis)
