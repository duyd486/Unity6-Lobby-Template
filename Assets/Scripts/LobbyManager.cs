using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{

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
        if(Input.GetKeyDown(KeyCode.Space))
        {
            CreateLobby();
        }
    }


    public async void CreateLobby()
    {
        try
        {
            string lobbyName = "MyLobby";
            int maxPlayer = 4;
            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayer);
            Debug.Log("Create Lobby " + lobby.Name + " " + lobby.MaxPlayers);
        } catch (LobbyServiceException e)
        {
            Debug.LogException(e);
        }
    }

    public async void ListLobbies()
    {
        try
        {
            QueryLobbiesOptions options = new QueryLobbiesOptions();

            QueryResponse queryResponse = await LobbyService.Instance.QueryLobbiesAsync();
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

}
