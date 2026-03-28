using EventGoApp.Models;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace EventGoApp.Services;

/// <summary>
/// Classe de service pour gérer l'authentification des utilisateurs en communiquant avec une API d'authentification.
/// </summary>
/// <remarks>
/// Cette classe utilise HttpClient pour envoyer des requêtes HTTP à une API d'authentification.
/// </remarks>
public class AuthService
{
    private readonly HttpClient _httpClient;

#if ANDROID
    private const string BaseUrl = "http://10.0.2.2:5121/api/auth";
#else
    private const string BaseUrl = "http://localhost:5121/api/auth";
#endif

    public AuthService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// Méthode asynchrone pour connecter un utilisateur en envoyant une requête POST à 
    /// l'API d'authentification avec l'e-mail et le mot de passe, et en retournant une réponse 
    /// d'authentification contenant un token JWT si la connexion est réussie.
    /// </summary>
    /// <param name="email">L'adresse e-mail de l'utilisateur.</param>
    /// <param name="password">Le mot de passe de l'utilisateur.</param>
    /// <returns>Une réponse d'authentification contenant un token JWT si la connexion est réussie, sinon null.</returns>
    public async Task<AuthResponse?> LoginAsync(string email, string password)
    {
        var payload = new { Email = email, Password = password };
        var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/login", payload);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        return await response.Content.ReadFromJsonAsync<AuthResponse>();
    }

    /// <summary>
    /// Méthode asynchrone pour enregistrer un nouvel utilisateur en envoyant une requête POST à l'API d'authentification.
    /// </summary>
    /// <param name="email">L'adresse e-mail de l'utilisateur.</param>
    /// <param name="username">Le nom d'utilisateur de l'utilisateur.</param>
    /// <param name="password">Le mot de passe de l'utilisateur.</param>
    /// <param name="fullName">Le nom complet de l'utilisateur (optionnel).</param>
    /// <returns>Une réponse d'authentification contenant un token JWT si l'enregistrement est réussi, sinon null.</returns>
    public async Task<AuthResponse?> RegisterAsync(string email, string username, string password, string? fullName = null)
    {
        var payload = new { Email = email, Username = username, Password = password, FullName = fullName };
        var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/register", payload);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }
        return await response.Content.ReadFromJsonAsync<AuthResponse>();
    }
}
