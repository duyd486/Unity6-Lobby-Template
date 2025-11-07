using System;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager Instance { get; private set; }

    public event EventHandler OnLobbyCreated;

    private Lobby hostLobby;
    private List<Lobby> currentLobbies;
    private float heartbeatTimer;

    private void Awake()
    {
        Instance = this;
    }

    private async void Start()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Sign In " + AuthenticationService.Instance.PlayerId);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    private void Update()
    {
        HandleLobbyHeartbeat();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CreateLobby("MyLobby", 4);
        }
    }


    public async void CreateLobby(string lobbyName, int maxPlayer)
    {
        try
        {
            Debug.Log("Start Create Lobby");
            hostLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayer);
            Debug.Log("Create Lobby " + hostLobby.Name + " " + hostLobby.MaxPlayers);

            OnLobbyCreated?.Invoke(this, EventArgs.Empty);

        }
        catch (LobbyServiceException e)
        {
            Debug.LogException(e);
        }
    }

    public async void ListLobbies(Action ShowLobbies)
    {
        try
        {
            QueryLobbiesOptions options = new QueryLobbiesOptions();

            QueryResponse queryResponse = await LobbyService.Instance.QueryLobbiesAsync();

            currentLobbies = queryResponse.Results;

            ShowLobbies();

            Debug.Log("Found " + queryResponse.Results.Count + " lobby");
            foreach (Lobby lobby in queryResponse.Results)
            {
                Debug.Log("Lobby found " + lobby.Name + " " + lobby.MaxPlayers);
            }
        }
        catch (LobbyServiceException e)
        {
            Debug.LogException(e);
        }
    }

    public async void JoinLobby()
    {
        try
        {
            QueryResponse queryResponse = await LobbyService.Instance.QueryLobbiesAsync();

            await LobbyService.Instance.JoinLobbyByIdAsync(queryResponse.Results[0].Id);
        }
        catch (LobbyServiceException e)
        {
            Debug.LogException(e);
        }
    }

    private async void HandleLobbyHeartbeat()
    {
        if (hostLobby != null)
        {
            heartbeatTimer -= Time.deltaTime;
            if (heartbeatTimer < 0)
            {
                heartbeatTimer = 15;

                await LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);
            }
        }
    }

    public List<Lobby> GetCurrentLobbies()
    {
        return currentLobbies;
    }

}
