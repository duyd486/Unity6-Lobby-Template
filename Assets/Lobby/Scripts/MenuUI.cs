using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    [SerializeField] private Button createLobbyBtn;
    [SerializeField] private Button listLobbyBtn;

    private void Start()
    {
        createLobbyBtn.onClick.AddListener(() => {
            LobbyManager.Instance.CreateLobby("My Lobby", 4);
        });
        listLobbyBtn.onClick.AddListener(() => {
            Hide();
        });
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
