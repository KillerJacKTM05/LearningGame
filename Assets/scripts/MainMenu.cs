using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    [Range(0f, 5f)] public float DelayTime = 2f;
    public GameObject settingsCanvas;
    void Start()
    {
        settingsCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
