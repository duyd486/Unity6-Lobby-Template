using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbyUI : MonoBehaviour
{
    public static LobbyUI Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI lobbyName;

    private Lobby lobby;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Hide();
    }



    public void UpdateLobby(Lobby lobby)
    {
        this.lobby = lobby;
        lobbyName.text = lobby.Name;

        foreach (Player player in lobby.Players)
        {
            Debug.Log(player.Data["PlayerName"].Value);
        }

        Show();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
