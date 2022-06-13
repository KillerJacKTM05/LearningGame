using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MyNetworkManager : NetworkManager
{
    public static event Action ClientOnConnected;
    public static event Action ClientOnDisconnected;

    private bool isGameInProgress = false;

    [SerializeField] public List<MyNetworkPlayer> players = new List<MyNetworkPlayer>();
    

    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        if (!isGameInProgress)
        {
            return;
        }
        conn.Disconnect();
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);

        MyNetworkPlayer player = conn.identity.GetComponent<MyNetworkPlayer>();
        
        players.Add(player);

        player.SetDisplayName("Player" + numPlayers);
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        MyNetworkPlayer player = conn.identity.GetComponent<MyNetworkPlayer>();

        players.Remove(player);

        base.OnServerDisconnect(conn);

    }

    public override void OnStopServer()
    {
        players.Clear();

        isGameInProgress = false;

        base.OnStopServer();
    }

    public void StartGame()
    {
        if(numPlayers != 2)
        {
            return;
        }
        isGameInProgress = true;

        ServerChangeScene("SampleScene");
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();

        ClientOnConnected?.Invoke();
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();

        ClientOnDisconnected?.Invoke();
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        base.OnServerSceneChanged(sceneName);

        if (sceneName == SceneManager.GetActiveScene().name)
        {
            List<GameObject> spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint").ToList<GameObject>();
            int i = 0;
            foreach (var player in players)
            {
                GameObject avatar = Instantiate(player.playerModel, spawnPoints[i].transform.position, spawnPoints[i].transform.rotation);

                NetworkServer.Spawn(avatar, player.connectionToClient);
                player.SetHandler(avatar.GetComponent<QuestionHandler>());
                i++;
            }
        }
    }

}
