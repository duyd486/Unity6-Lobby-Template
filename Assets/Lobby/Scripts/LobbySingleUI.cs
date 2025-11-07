using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbySingleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI gameModeText;
    [SerializeField] private TextMeshProUGUI playerCountText;

    private Lobby lobby;


    public void UpdateLobby(Lobby lobby)
    {
        this.lobby = lobby;
        nameText.text = lobby.Name;
    }
}
