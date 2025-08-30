using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class ListLobbyUI : MonoBehaviour
{
    [SerializeField] private Button backButton;
    [SerializeField] private GameObject lobbyInfoTemplate;
    [SerializeField] private GameObject container;

    private void Start()
    {
        MenuUI.Instance.OnListLobbyClick += MenuUI_OnListLobbyClick;


        Hide();
        backButton.onClick.AddListener(() =>
        {
            Hide();
        });
    }

    private void MenuUI_OnListLobbyClick(object sender, System.EventArgs e)
    {
        Show();
        UpdateListLobby();
    }

    public void UpdateListLobby()
    {
        for(int i = 0; i < 10;  i++)
        {
            GameObject lobbyInfoOb = Instantiate(lobbyInfoTemplate, container.transform);
            lobbyInfoOb.SetActive(true);
        }
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
