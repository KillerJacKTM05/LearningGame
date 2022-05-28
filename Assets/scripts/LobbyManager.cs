using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Mirror;

public class LobbyManager : MonoBehaviour
{
    [SerializeField] GameObject lobbyUI = null;
    [SerializeField] Text[] playerDisplays = new Text[2];
    private void OnEnable()
    {
        MyNetworkManager.ClientOnConnected += HandleClientConnected;
        MyNetworkPlayer.ClientOnNameUpdated += HandleClientInfo;
    }

    private void OnDisable()
    {
        MyNetworkManager.ClientOnConnected -= HandleClientConnected;
        MyNetworkPlayer.ClientOnNameUpdated -= HandleClientInfo;
    }

    private void HandleClientInfo()
    {
        List<MyNetworkPlayer> players = ((MyNetworkManager)NetworkManager.singleton).players;

        for (int i = 0; i < players.Count; i++)
        {
            Debug.Log("Hello");
            playerDisplays[i].text = players[i].GetDisplayName();
        }
    }

    private void HandleClientConnected()
    {
        lobbyUI.SetActive(true);
    }

    public void LeaveLobby()
    {
        if (NetworkServer.active && NetworkClient.isConnected)
        {
            NetworkManager.singleton.StopHost();
        }
        else
        {
            NetworkManager.singleton.StopClient();
        }
        SceneManager.LoadScene(0);
    }
}
