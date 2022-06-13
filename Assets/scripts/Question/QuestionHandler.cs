using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class QuestionHandler : NetworkBehaviour
{
    [SerializeField] QuestionUIv2 questionUI = null;

    [SerializeField] public bool isMyTurn = false;

    #region Server

    public void Start()
    {
        Debug.Log(hasAuthority);
        if (!hasAuthority)
        {
            return;
        }
        Debug.Log("has auth");
        questionUI = GameObject.FindObjectOfType<QuestionUIv2>();
        questionUI.SetPlayer(this);
    }

    [Command]
    public void CmdMove()
    {
        this.transform.Translate(Vector3.forward * 5);
    }

    #endregion


    #region Client

    public void Move()
    {
        if (!hasAuthority)
        {
            return;
        }
        Debug.Log("Trying To Move");
        CmdMove();
    }

    #endregion

}
