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
    private string playerName;

    private void Awake()
    {
        Instance = this;
        playerName = "duy" + UnityEngine.Random.Range(10, 99);
        Debug.Log("Player Name: " + playerName);
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
    }


    public async void CreateLobby(string lobbyName, int maxPlayer, Action<Lobby> HostLobby)
    {
        try
        {
            Debug.Log("Start Create Lobby");

            CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions
            {
                IsPrivate = false,
                Player = GetPlayer()
            };


            hostLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayer, createLobbyOptions);
            Debug.Log("Create Lobby " + hostLobby.Name + " " + hostLobby.MaxPlayers);

            OnLobbyCreated?.Invoke(this, EventArgs.Empty);
            HostLobby(hostLobby);
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

    public async void JoinLobby(string id, Action<Lobby> UpdateLobby)
    {
        try
        {
            QueryResponse queryResponse = await LobbyService.Instance.QueryLobbiesAsync();

            JoinLobbyByIdOptions joinLobbyByIdOptions = new JoinLobbyByIdOptions
            {
                Player = GetPlayer()
            };
            Lobby joinedLobby = await LobbyService.Instance.JoinLobbyByIdAsync(id, joinLobbyByIdOptions);
            UpdateLobby(joinedLobby);
        }
        catch (LobbyServiceException e)
        {
            Debug.LogException(e);
        }
    }

    public async void LeaveLobby()
    {
        try
        {
            QueryResponse queryResponse = await LobbyService.Instance.QueryLobbiesAsync();
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

    public Player GetPlayer()
    {
        return new Player
        {
            Data = new Dictionary<string, PlayerDataObject>
            {
                {"PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, playerName) }
            }
        };
    }

    public void SetPlayerName(string playerName)
    {
        this.playerName = playerName;
    }
}
