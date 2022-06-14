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
    public Toggle fullscreenToggle;
    public Dropdown qualityDropdown;
    public Dropdown resolutionDropdown;

    Resolution[] resolutions;
    List<Resolution> validResolutions;
    void Start()
    {
        resolutions = Screen.resolutions;
        settingsCanvas.SetActive(false);
        lobbyCanvas.SetActive(false);
        GetResolutions();
        GetQuality();
        LoadSettings();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnEnable()
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
            resolutionDropdown.RefreshShownValue();
            qualityDropdown.RefreshShownValue();
            volumeSlider.value = SoundManager.instance.GetSource().volume;
            volumeSlider.onValueChanged.AddListener(delegate { changeVolume(volumeSlider.value); });
            settingsCanvas.SetActive(true);
        }
        else
        {
            volumeSlider.onValueChanged.RemoveListener(delegate { changeVolume(volumeSlider.value); });
            settingsCanvas.SetActive(false);
        }
    }
    public void changeVolume(float sliderValue)
    {
        SoundManager.instance.GetSource().volume = sliderValue;
    }
    public void SetFullscreen()
    {
        Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, fullscreenToggle.isOn);
    }
    public void GetResolutions()
    {
        string prevOption = "";
        List<string> options = new List<string>();
        validResolutions = new List<Resolution>();

        for(int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            //Debug.Log("option:" + option);

            if(prevOption != option)
            {
                options.Add(option);
                validResolutions.Add(resolutions[i]);
                prevOption = option;
            }
            
        }
        resolutionDropdown.AddOptions(options);
    }
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = validResolutions[resolutionIndex];
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
