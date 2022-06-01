using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance { get; private set; }
    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    [SerializeField] private GameObject inGameCanvas;
    [SerializeField] private GameObject menuCanvas;
    [SerializeField] private Text worldTimer;
    [SerializeField] private GameObject profilePanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private GameObject topicsPanel;
    [SerializeField] private List<GameObject> topicButtons;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DisplayLocalTime();
    }
    private void OnEnable()
    {
        volumeSlider.onValueChanged.AddListener(delegate { changeVolume(volumeSlider.value); });
    }
    /* MENU THINGS */
    public void openCloseMenu(int index)
    {
        if(index == 0)
        {
            volumeSlider.value = SoundManager.instance.GetSource().volume;
            menuCanvas.SetActive(true);
            inGameCanvas.SetActive(false);
            profilePanel.SetActive(false);
            settingsPanel.SetActive(false);
            infoPanel.SetActive(false);
        }
        else if(index == 1)
        {
            menuCanvas.SetActive(false);
            inGameCanvas.SetActive(true);
            profilePanel.SetActive(false);
            settingsPanel.SetActive(false);
            infoPanel.SetActive(false);
        }
    }
    public void openCloseSettings(int index)
    {
        if (index == 0)
        {
            settingsPanel.SetActive(true);
        }
        else if (index == 1)
        {
            settingsPanel.SetActive(false);
        }
    }
    public void openCloseProfile(int index)
    {
        if (index == 0)
        {
            profilePanel.SetActive(true);
        }
        else if (index == 1)
        {
            profilePanel.SetActive(false);
        }
    }
    public void openCloseInfo(int index)
    {
        if (index == 0)
        {
            infoPanel.SetActive(true);
        }
        else if (index == 1)
        {
            infoPanel.SetActive(false);
        }
    }
    public void DisplayLocalTime()
    {
        string time;
        if(System.DateTime.Now.Minute < 10)
        {
            time = System.DateTime.Now.Hour + ":0" + System.DateTime.Now.Minute;
        }
        else
        {
            time = System.DateTime.Now.Hour + ":" + System.DateTime.Now.Minute;
        }
        worldTimer.text = time;
    }
    public void changeVolume(float sliderValue)
    {
        SoundManager.instance.GetSource().volume = sliderValue;
    }

    /* GAME THINGS */
    public void QuestionTimer(int counter)              //use this for updating the timer of each question.
    {
        StartCoroutine(Counter(counter));
    }
    private IEnumerator Counter(int count)
    {
        float currentTime = 0;
        int counterDisplayer = count;
        while(currentTime < count)
        {
            currentTime += Time.deltaTime;
            counterDisplayer = (int)(count - currentTime);
            yield return new WaitForEndOfFrame();
        }
    }
    public void GetTopicChoices()
    {
        Button[] buttons = topicsPanel.GetComponentsInChildren<Button>();
        foreach(Button button in buttons)
        {
            topicButtons.Add(button.transform.parent.gameObject);
        }
    }
    public void SetTopicChoices(int index)
    {
        if(index < QuestionPool.instance.Topic.Count)
        {
            topicButtons[index].GetComponentInChildren<Text>().text = QuestionPool.instance.Topic[index].topicName;
            SetTopicChoices(index + 1);
        }
    }
    public GameObject getTopicsPanel()
    {
        return topicsPanel;
    }
    public void ChooseTopic(int index)
    {
        GameManager.instance.SetPlayerTopicName(1, QuestionPool.instance.Topic[index].topicName); //1 or 2, 1 for player1; 2 for player2
        topicsPanel.SetActive(false);
    }
}
