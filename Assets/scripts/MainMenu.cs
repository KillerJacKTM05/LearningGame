using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    [Range(0f, 5f)] public float DelayTime = 2f;
    public GameObject settingsCanvas;
    public GameObject lobbyCanvas;
    public GameObject serverCanvas;
    void Start()
    {
        settingsCanvas.SetActive(false);
        lobbyCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartGame()
    {
        lobbyCanvas.SetActive(true);
    }
    public void HostGame(GameObject startButton)
    {
        serverCanvas.SetActive(true);

        startButton.SetActive(true);

        MyNetworkManager.singleton.StartHost();
    }

    public void JoinGame(GameObject AdressMenu)
    {
        AdressMenu.SetActive(true);
    }
    public void QuitGame()
    {
        StartCoroutine(Quiter());
    }
    private IEnumerator Quiter()
    {
        float currentTime = 0f;
        while(currentTime < DelayTime)
        {
            currentTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        Application.Quit();
    }
    public void OpenCloseSettings(int index)
    {
        if(index == 0)
        {
            settingsCanvas.SetActive(true);
        }
        else
        {
            settingsCanvas.SetActive(false);
        }
    }
}