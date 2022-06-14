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

    public GameObject playerModel;

    [SerializeField] private QuestionHandler myHandler;


    public static event Action ClientOnNameUpdated;

    public void SetHandler(QuestionHandler handler)
    {
        myHandler = handler;
    }

    public QuestionHandler GetHandler()
    {
        return myHandler;
    }


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
        Debug.Log(newName);

        ClientOnNameUpdated?.Invoke();
    }
}
