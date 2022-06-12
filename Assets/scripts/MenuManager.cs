using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class MenuManager : NetworkBehaviour
{

    [SerializeField] QuestionHandler questionHandler = null;

    #region Server
    public override void OnStartServer()
    {
        base.OnStartServer();

        QuestionHandler.ServerOnClientStart += ClientHandleSpawner;
        QuestionHandler.ServerOnClientStop += ClientHandleDespawner;
    }

    public override void OnStopServer()
    {
        base.OnStopServer();

        QuestionHandler.ServerOnClientStart -= ClientHandleSpawner;
        QuestionHandler.ServerOnClientStop -= ClientHandleDespawner;
    }

    public override void OnStartClient()
    {

        base.OnStartClient();
        if (!isClientOnly)
        {
            return;
        }



        QuestionHandler.AuthOnClientStart += ClientHandleSpawner;
        QuestionHandler.AuthOnClientStop += ClientHandleDespawner;
    }

    public override void OnStopClient()
    {
        base.OnStopClient();
        if (!isClientOnly)
        {
            return;
        }
        QuestionHandler.AuthOnClientStart -= ClientHandleSpawner;
        QuestionHandler.AuthOnClientStop -= ClientHandleDespawner;
    }


    private void ClientHandleSpawner(QuestionHandler ques)
    {
        Debug.Log("Client Check");
        if (!ques.hasAuthority)
        {
            return;
        }
        questionHandler = ques;
    }

    private void ClientHandleDespawner(QuestionHandler ques)
    {
        Debug.Log("Client Check");
        if (!ques.hasAuthority)
        {
            return;
        }
        questionHandler = ques;
    }

    #endregion

    #region Non-server
    public static MenuManager instance { get; private set; }
    private void Awake()
    {
        if (instance != null && instance != this)
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

    public List<QuestionUI> questionPanel;                                      //0 index is for multiple choice, 1 index for input question
    [Range(0f, 5f)] public float questionDisplayDelay = 3f;
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
    public void openCloseMenu(int index)                    //Menu tab
    {
        if (index == 0)
        {
            volumeSlider.value = SoundManager.instance.GetSource().volume;
            menuCanvas.SetActive(true);
            inGameCanvas.SetActive(false);
            profilePanel.SetActive(false);
            settingsPanel.SetActive(false);
            infoPanel.SetActive(false);
        }
        else if (index == 1)
        {
            menuCanvas.SetActive(false);
            inGameCanvas.SetActive(true);
            profilePanel.SetActive(false);
            settingsPanel.SetActive(false);
            infoPanel.SetActive(false);
        }
    }
    public void openCloseSettings(int index)                //Settings tab
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
    public void openCloseProfile(int index)                 //Profile tab
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
    public void openCloseInfo(int index)                    //Info tab
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
    public void DisplayLocalTime()                          //This function displays the system time on the ingame menu.
    {
        string time;
        if (System.DateTime.Now.Minute < 10)
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
    public void QuestionTimer(int counter, GameObject display)
    {
        StartCoroutine(Counter(counter, display));
    }
    public IEnumerator Counter(int count, GameObject display)              //this is a simple counter that will be used during displaying the questions.
    {
        float currentTime = 0;
        Text counterDisplay = display.GetComponent<Text>();
        while (currentTime < count)
        {
            currentTime += Time.deltaTime;
            var t = (int)(count - currentTime);
            counterDisplay.text = t.ToString();
            yield return new WaitForEndOfFrame();
        }
    }

    public IEnumerator QuestionDisplayDelay()
    {
        yield return new WaitForSeconds(questionDisplayDelay);
        GameManager.instance.questionDisplayed = false;
    }
    public void GetTopicChoices()
    {
        Button[] buttons = topicsPanel.GetComponentsInChildren<Button>();
        foreach (Button button in buttons)
        {
            topicButtons.Add(button.transform.parent.gameObject);
        }
    }
    public void SetTopicChoices(int index)                  //this function reads the topics defined in pool and displays on the topic selection UI.
    {
        if (index < QuestionPool.instance.Topic.Count)
        {
            topicButtons[index].GetComponentInChildren<Text>().text = QuestionPool.instance.Topic[index].topicName;
            SetTopicChoices(index + 1);
        }
    }
    public GameObject getTopicsPanel()
    {
        return topicsPanel;
    }
    public void ChooseTopic(int index)      //player topic choosing
    {
        GameManager.instance.SetPlayerTopicName(1, QuestionPool.instance.Topic[index].topicName); //1 or 2, 1 for player1; 2 for player2
        topicsPanel.SetActive(false);
        GameManager.instance.topicsChosed = true;
    }
    public void Answer(int index)           //check answers here, correct or false. according to value, please edit player.
    {
        if (GameManager.instance.displayedQuestion.GetComponent<QuestionStructure>().isQuestionMultipleChoice)
        {
            if (index == GameManager.instance.displayedQuestion.GetComponent<QuestionStructure>().correctChoiceIndex)
            {
                questionHandler.Move();
            }
            else
            {
                Debug.Log("False");
            }
        }
        else
        {
            if (questionPanel[1].getFieldText() == GameManager.instance.displayedQuestion.GetComponent<QuestionStructure>().questionAnswerIfNotMultipleChoice)
            {
                questionHandler.Move();
            }
            else
            {
                Debug.Log("False");
            }
        }

        for (int i = 0; i < questionPanel.Count; i++)
        {
            questionPanel[i].gameObject.SetActive(false);
        }
        StartCoroutine(QuestionDisplayDelay());
    }
    #endregion

}
