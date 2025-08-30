using System;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    public static MenuUI Instance { get; private set; }

    [SerializeField] private Button createLobbyBtn;
    [SerializeField] private Button listLobbyBtn;

    public event EventHandler OnListLobbyClick;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        createLobbyBtn.onClick.AddListener(() => {
            LobbyManager.Instance.CreateLobby("My Lobby", 4);
        });
        listLobbyBtn.onClick.AddListener(() => {
            OnListLobbyClick?.Invoke(this, EventArgs.Empty);
            //Hide();
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
