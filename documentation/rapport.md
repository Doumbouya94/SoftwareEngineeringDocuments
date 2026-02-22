# Résumé des changements — SRS v1 → v2

## Métadonnées & identification
- Les placeholders (`À compléter`) ont été remplacés par les vrais noms de l'équipe : **Pierre-Sylvestre Cypré, Aboubacar Doumbouya Sidiki**
- La version est passée de `v0.1` à `Pre-Alpha`  *(corrige : placeholders non finalisés)*

## Portée (Scope)
- Ajout de **OUT-5** : exclusion explicite des APIs externes (Ticketmaster, Eventbrite)  *(corrige : incohérence OUT-5 vs dépendances)*

## Exigences fonctionnelles (FR)
Les FR sont passées de 7 à **15 exigences**, avec des critères précis et testables :
- **FR-1** : validation du mot de passe (≥ 8 caractères, 1 majuscule, 1 chiffre), JWT, erreur 409 sur doublon 
- **FR-2** : 5 critères de filtre combinables (localisation, rayon, prix, dates, catégorie), tri des résultats *(corrige : filtres trop généraux)*
- **FR-3** : si géolocalisation refusée → saisie manuelle d'adresse *(corrige : comportement si refus géolocalisation)*
- **FR-4** : format d'événement défini (titre, description, catégorie, date, lieu, prix) + règles de modification *(corrige : format et validation d'un événement)*
- **FR-5 à FR-15** : nouveaux flux, billetterie, avis, onboarding, thème, mode invité, recherche avec suggestions, événements internes/externes

## Exigences non fonctionnelles (NFR)
Passées de vagues à **précises et mesurables** :
- **Performance** : chargement < 2 s sur 4G, pagination 20 résultats, cache TTL 5 min, lazy loading
- **Sécurité** : HTTPS/TLS 1.2, JWT expirant après 24h, bcrypt ≥ 12, rate limiting
- **Qualité** : couverture de tests unitaires fixée à **80 %** sur les services critiques
 *(corrige : FR/NFR trop généraux, manque de critères mesurables)*

## Contraintes
- Technologie précisée : **ASP.NET Core + MAUI.NET + MySQL** (remplace Java/Spring Boot)
- Dates de phases ajoutées explicitement

## Dépendances
- Mises à jour vers les outils réels : **Azure Notification Hubs**, **MAUI Geolocation**, **ASP.NET Core Authentication**
- Ticketmaster/Eventbrite retirés des dépendances (cohérent avec OUT-5) 
