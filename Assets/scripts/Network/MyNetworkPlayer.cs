using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class MyNetworkPlayer : NetworkBehaviour
{
    [SerializeField][SyncVar(hook = nameof(HandleClientNameUpdated))]
    string displayName = "No Name";


    public static event Action ClientOnNameUpdated;

    public string GetDisplayName()
    {
        return displayName;
    }

    [Server]
    public void SetDisplayName(string name)
    {
        displayName = name;
    }

    public override void OnStartClient()
    {
        Text nameTag = GameObject.FindGameObjectWithTag("NameTag").GetComponent<Text>();
        displayName = nameTag.text;
        DontDestroyOnLoad(gameObject);
        base.OnStartClient();
    }

    public override void OnStartServer()
    {
        DontDestroyOnLoad(gameObject);
        base.OnStartServer();
    }

    public void HandleClientNameUpdated(string oldName, string newName)
    {
        ClientOnNameUpdated?.Invoke();
    }

    [Command]
    public void CmdChangeMyName(string name)
    {
        displayName = name;
    }
}
