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
        Debug.Log("Player Connected" + ((MyNetworkManager)NetworkManager.singleton).players.Count);


        for (int i = 0; i < NetworkManager.singleton.numPlayers; i++)
        {
            playerDisplays[i].text = "Ready";
        }
        for (int i = NetworkManager.singleton.numPlayers; i < 2; i++)
        {
            playerDisplays[i].text = "Waiting For Players....";
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
        else if(NetworkClient.isConnected)
        {
            NetworkManager.singleton.StopClient();
        }
        SceneManager.LoadScene(0);
    }
}
