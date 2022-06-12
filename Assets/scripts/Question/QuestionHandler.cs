using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class QuestionHandler : NetworkBehaviour
{
    public static event Action<QuestionHandler> ServerOnClientStart;
    public static event Action<QuestionHandler> ServerOnClientStop;

    public static event Action<QuestionHandler> AuthOnClientStart;
    public static event Action<QuestionHandler> AuthOnClientStop;


    [Command]
    public void CmdMove()
    {
        this.transform.Translate(Vector3.forward* 5);
    }

    [ClientCallback]
    public void Move()
    {
        if (!hasAuthority)
        {
            return;
        }
        CmdMove();
    }
}
