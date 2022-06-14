using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class TurnManager : NetworkBehaviour
{
    [SerializeField] private QuestionUIv2 questionUI;


    private void Start()
    {
        questionUI = GetComponent<QuestionUIv2>();
    }

    public void SetTurn()
    {
        questionUI.CmdSetTurn();
    }
}
