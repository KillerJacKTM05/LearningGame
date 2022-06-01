using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    [Range(0f, 5f)] public float DelayTime = 2f;
    public GameObject settingsCanvas;
    public GameObject lobbyCanvas;
    public GameObject serverCanvas;
    public Slider volumeSlider;
    public Dropdown qualityDropdown;
    public Dropdown resolutionDropdown;

    Resolution[] resolutions;
    void Start()
    {
        resolutions = Screen.resolutions;
        settingsCanvas.SetActive(false);
        lobbyCanvas.SetActive(false);
        volumeSlider.value = SoundManager.instance.GetSource().volume;
        GetResolutions();
        GetQuality();
        resolutionDropdown.RefreshShownValue();
        qualityDropdown.RefreshShownValue();
        LoadSettings();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnEnable()
    {
        volumeSlider.onValueChanged.AddListener(delegate { changeVolume(volumeSlider.value); });
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
    public void changeVolume(float sliderValue)
    {
        SoundManager.instance.GetSource().volume = sliderValue;
    }
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
    public void GetResolutions()
    {
        List<string> options = new List<string>();
        for(int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            //Debug.Log("option:" + option);
            options.Add(option);
        }
        resolutionDropdown.AddOptions(options);
    }
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        resolutionDropdown.value = resolutionIndex;
    }
    public void GetQuality()
    {
        List<string> options = new List<string>();
        for(int i = 0; i < QualitySettings.names.Length; i++)
        {
            options.Add(QualitySettings.names[i]);
        }
        qualityDropdown.AddOptions(options);
    }
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        qualityDropdown.value = qualityIndex;
    }
    public void SaveSettings()
    {
        PlayerPrefs.SetInt("QualitySettingPreference", qualityDropdown.value);
        PlayerPrefs.SetInt("ResolutionPreference", resolutionDropdown.value);
        PlayerPrefs.SetFloat("VolumePreference", volumeSlider.value);
        PlayerPrefs.SetInt("FullscreenPreference", System.Convert.ToInt32(Screen.fullScreen));
    }
    public void LoadSettings()
    { 
        qualityDropdown.value = PlayerPrefs.GetInt("QualitySettingPreference");
        resolutionDropdown.value = PlayerPrefs.GetInt("ResolutionPreference");
        volumeSlider.value = PlayerPrefs.GetFloat("VolumePreference");
        Screen.fullScreen = System.Convert.ToBoolean(PlayerPrefs.GetInt("FullscreenPreference"));
    }
}
