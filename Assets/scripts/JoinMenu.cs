using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class JoinMenu : MonoBehaviour
{
    [SerializeField] private InputField input;
    [SerializeField] private Button joinButton;
    [SerializeField] private GameObject serverCanvas;
    private void OnEnable()
    {
        MyNetworkManager.ClientOnConnected += HandleClientConnected;
        MyNetworkManager.ClientOnDisconnected += HandleClientDisconnected;
    }

    private void OnDisable()
    {
        MyNetworkManager.ClientOnConnected -= HandleClientConnected;
        MyNetworkManager.ClientOnDisconnected -= HandleClientDisconnected;
    }

    public void Join()
    {
        string adress = input.text;
        if (adress == string.Empty)
        {
            adress = "localhost";
        }

        Debug.Log("Joining Server: " + adress);

        NetworkManager.singleton.networkAddress = adress;
        NetworkManager.singleton.StartClient();

        joinButton.interactable = false;
    }


    void HandleClientConnected()
    {
        joinButton.interactable = true;


        serverCanvas.SetActive(true);
        gameObject.SetActive(false);
    }

    void HandleClientDisconnected()
    {
        joinButton.interactable = true;
    }
}
