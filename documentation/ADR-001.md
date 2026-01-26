# ADR-001 — Choix du stack technologique mobile (MAUI.NET + SQLite)

**Statut :** Accepted  
**Date :** 2026-01-23  
**Décideurs :** Pierre-Sylvestre Cypré, Aboubacar Doumbouya Sidiki  
**Contexte projet :** EventGo — Application mobile de découverte d'événements locaux  

---

## 1. Contexte

- **Problème / besoin :** Développer une application mobile cross-platform pour iOS et Android permettant la découverte d'événements locaux avec géolocalisation, filtres, favoris et notifications. L'application doit être livrée en 4 phases entre janvier et avril 2026.
- **Contraintes :**
	- **Temps :** Délais serrés avec 4 phases (Phase 1: 25 janvier 2026, Phase 2: 22 février 2026, Phase 3: 22 mars 2026, Phase 4: 23-19 avril 2026)
	- **Équipe :** Équipe de 2 développeurs avec expérience en C# et .NET
	- **Plateforme :** Mobile uniquement (iOS et Android)
	- **Outils :** Visual Studio, Git, Notion
	- **Académique :** Projet dans le cadre du cours "Implémentation d'un système d'information"
- **Forces en présence :**
	- Besoin de développement rapide avec base de code unique
	- Performance native requise pour la géolocalisation en temps réel
	- Simplicité de déploiement et de maintenance
	- Stockage local pour les favoris et cache des données
	- Courbe d'apprentissage de l'équipe

---

## 2. Décision

> Nous choisissons **C# avec .NET MAUI + SQLite** comme stack technologique pour le développement de l'application mobile EventGo.

- Nous choisissons : .NET MAUI (Multi-platform App UI) en C# avec SQLite comme base de données locale
- Pour : Développer une application mobile cross-platform performante avec une base de code unique, tirant parti de notre expertise existante en C# et .NET

---

## 3. Alternatives considérées

### Option A — Flutter + Dart

- **Avantages :**
	- Excellent support cross-platform avec une seule base de code
	- UI hautement personnalisable avec Material Design et Cupertino
	- Hot reload pour développement rapide
	- Grande communauté et écosystème de packages riche
	- Performance native via compilation AOT
- **Inconvénients :**
	- Nouveau langage (Dart) à apprendre pour l'équipe
	- Courbe d'apprentissage supplémentaire
	- Moins d'alignement avec le parcours académique actuel en .NET

### Option B — React Native + JavaScript/TypeScript

- **Avantages :**
	- Large communauté et écosystème mature
	- Utilise JavaScript/TypeScript (langage web familier)
	- Bon support pour les bibliothèques tierces
	- Hot reload disponible
- **Inconvénients :**
	- Performance parfois inférieure aux solutions natives
	- Bridge JavaScript peut causer des problèmes de performance
	- Nécessite apprentissage de React et de l'écosystème JavaScript
	- Moins d'alignement avec les compétences actuelles de l'équipe

### Option C — Développement natif (Swift + Kotlin)

- **Avantages :**
	- Performance maximale et accès complet aux APIs natives
	- Meilleures pratiques spécifiques à chaque plateforme
	- Expérience utilisateur optimale par plateforme
- **Inconvénients :**
	- Deux bases de code distinctes à maintenir
	- Temps de développement doublé
	- Nécessite expertise en Swift ET Kotlin
	- Impossible dans les délais du projet

---

## 4. Justification (Pourquoi cette décision ?)

- **Expertise existante :** Moi et Aboubacar nous possédons déjà des compétences en C# et .NET, donc ça élimine la courbe d'apprentissage d'un nouveau langage et nous permet de se concentrer sur les fonctionnalités métier
- **Alignement académique :** Le cours "Développement mobile multi-plateforme" couvre spécifiquement .NET MAUI, permettant de tirer parti des apprentissages en cours
- **Cross-platform efficace :** MAUI permet de partager 90%+ du code entre iOS et Android avec une seule base de code en C#
- **Performance native :** MAUI compile en natif pour chaque plateforme, garantissant de bonnes performances pour la géolocalisation et les interactions utilisateur
- **SQLite intégré :** Support natif et mature de SQLite dans .NET pour le stockage local (favoris, cache)
- **Outillage mature :** Visual Studio offre un excellent support pour MAUI avec débogage, profiling et UI designer
- **Délais serrés :** Pas de temps pour apprendre un nouveau langage (Dart, TypeScript) ou framework
- **Écosystème .NET :** Accès à NuGet et toutes les bibliothèques .NET pour services de notifications, géolocalisation, etc.

---

## 5. Conséquences

### Positives

- Développement rapide grâce à l'expertise existante en C#
- Base de code unique réduit les coûts de maintenance
- Support natif de la géolocalisation, notifications push et SQLite
- Intégration facile avec Visual Studio et Git
- Réutilisation possible de code pour futurs projets .NET
- Performance native sur iOS et Android

### Négatives / Risques

- Écosystème de packages moins mature que Flutter ou React Native
- Communauté plus petite que Flutter/React Native pour résolution de problèmes
- MAUI est relativement récent (moins mature que Xamarin)
- Possibles bugs ou limitations dans MAUI comparé à des frameworks plus établis
- UI peut nécessiter plus de travail pour un look natif parfait sur chaque plateforme

### Impact sur l'architecture / le code

- **Couche de données :** Implémentation de Entity Framework Core Lite ou SQLite-net pour SQLite
- **Architecture MVVM :** Utilisation du pattern MVVM (Model-View-ViewModel) standard dans MAUI
- **Services :** Création de services pour géolocalisation (Geolocator), notifications locales, et gestion des favoris
- **Navigation :** Shell navigation de MAUI pour la structure de l'app
- **Modules touchés :** Tous les modules (Auth, Events, Favorites, Notifications, Profile)

---

## 6. Plan d'implémentation (court)

- [x] **Phase 1 (jusqu'au 25 janvier 2026) :** Setup projet MAUI + SQLite, structure de base, authentification
- [ ] **Phase 2 (jusqu'au 22 février 2026) :** Implémentation de la liste d'événements, filtres, géolocalisation
- [ ] **Phase 3 (jusqu'au 22 mars 2026) :** Système de favoris, notifications, gestion organisateur
- [ ] **Phase 4 (23 mars - 19 avril 2026) :** Fonctionnalités admin, tests complets, optimisations, déploiement

---

## 7. Validation

- **Comment vérifier que c'est bon ?**
- L'application compile et s'exécute sur iOS et Android
- Les fonctionnalités de géolocalisation fonctionnent en temps réel sur les deux plateformes
- Le temps de réponse pour la recherche d'événements est < 2 secondes (NFR-1)
- Le parcours utilisateur nécessite ≤ 3 clics pour trouver un événement (NFR-3)
- Les favoris sont persistés correctement dans SQLite
- Les notifications s'affichent correctement
- Tests unitaires couvrent les services critiques (Auth, Events, Favorites)
- Pas de régression de performance par rapport aux benchmarks natifs

---

## Notes additionnelles

**Décision prise en considération de :**
- Contraintes temporelles du projet (4 phases en 3 mois)
- Background académique et professionnel de l'équipe en C#/.NET
- Exigences fonctionnelles (géolocalisation, notifications, favoris locaux)
- Exigences non-fonctionnelles (performance, UX fluide)
- Exclusion des APIs externes (Ticketmaster, Eventbrite) en Phase 1

**Points de révision potentiels :**
Si en Phase 2-3 nous rencontrons des limitations significatives de MAUI (ex: problèmes de performance, bugs bloquants, manque de bibliothèques critiques), nous pourrons reconsidérer Flutter avec une migration progressive. Cependant, cette décision devra être documentée dans un nouvel ADR (ADR-002).
