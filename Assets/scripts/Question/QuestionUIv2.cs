using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Mirror;

public class QuestionUIv2 : NetworkBehaviour
{
    [SerializeField] private int debugQuestion = 0;
    [SerializeField] private QuestionHandler handler = null;
    [SerializeField] private bool IsQuestionable = false;
    [SerializeField] private int topicID = 99;
    [SerializeField] private List<QuestionStructure> questionPool = new List<QuestionStructure>();
    [SerializeField] private List<QuestionStructure> topicPool = new List<QuestionStructure>();
    [SerializeField] private QuestionStructure currQuestion = null;

    [Header("UI")]
    [SerializeField] private GameObject canvas;
    [SerializeField] private Text questionText;
    [SerializeField] private GameObject menuCanvas;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject profilePanel;
    [SerializeField] private Text profile;
    [SerializeField] private Dropdown qualityDropdown;
    [SerializeField] private Dropdown resolutionDropdown;

    [Header("Topic")]
    [SerializeField] private GameObject topicsPanel;
    [SerializeField] private List<string> topicNames;
    [SerializeField] private List<Button> topicButtons;
    [SerializeField] private List<Text> topicButtonTexts;

    [Header("Multiple Chioces")]
    [SerializeField] private GameObject multiAnswerObject;
    [SerializeField] private List<Button> answerButtons;
    [SerializeField] private List<Text> answerButtonTexts;

    [Header("Input Answer")]
    [SerializeField] private GameObject inputAnswerObject;
    [SerializeField] private Button answerButton;
    [SerializeField] private InputField inputField;

    [Header("Game Over UI")]
    [SerializeField] private GameObject gameOverObject;
    [SerializeField] private Text playerName;
    [SerializeField] private int playerPoint = 0;
    [SerializeField] private bool isMyTurn = false;

    public void Start()
    {      
        if (isServer)
        {
            isMyTurn = !isMyTurn;
        }
    }

    private void Update()
    {
        if (canvas.activeInHierarchy)
        {
            return;
        }
        else if (handler && !IsQuestionable)
        {
            SetTopicPanel();
        }
    }
    #region Ui Thing
    public void EndGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }

    public void ReturnMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
    public void OpenMenu(int index)
    {
        if(index == 0)                              //menu ui opened
        {
            menuCanvas.SetActive(true);
            canvas.SetActive(false);
        }
        else if(index == 1)                         //menu ui closed
        {
            menuCanvas.SetActive(false);
            canvas.SetActive(true);
        }
    }
    public void Settings(int index)
    {
        if (index == 0)                              //settings opened
        {
            settingsPanel.SetActive(true);
        }
        else if(index == 1)
        {
            settingsPanel.SetActive(false);
        }
    }
    public void Profile(int index)
    {
        if (index == 0)                              //settings opened
        {
            profilePanel.SetActive(true);
            profile.text = connectionToClient.identity.GetComponent<MyNetworkPlayer>().GetDisplayName();
        }
        else if (index == 1)
        {
            profilePanel.SetActive(false);
        }
    }
    #endregion

    #region Topic

    private void SetTopicPanel()
    {
        topicsPanel.SetActive(true);
        questionText.text = "Choose your Topic";
        for (int i = 0; i < 3; i++)
        {
            topicButtonTexts[i].text = topicNames[i];
        }
        canvas.SetActive(true);
    }

    public void SetTopic(int ID)
    {
        topicID = ID;
        topicsPanel.SetActive(false);
        IsQuestionable = true;
        canvas.SetActive(false);
        TopicQuestionList();
    }

    public void TopicQuestionList()
    {
        foreach (QuestionStructure item in questionPool)
        {
            if (item.questionTopic == topicNames[topicID])
            {
                topicPool.Add(item);
            }
        }
        if (isServer)
        {
            SendQuestion();
        }
    }

    #endregion

    #region Network
    public void SetPlayer(QuestionHandler _handler)
    {
        handler = _handler;
    }

    public void TrueAnswer()
    {
        handler.Move();
    }

    [Command(requiresAuthority = false)]
    public void CmdSetTurn()
    {
        RpcSetTurn();
    }

    [ClientRpc]
    public void RpcSetTurn()
    {
        isMyTurn = !isMyTurn;

        if (playerPoint == 7)
        {
            Debug.Log("Won the Game");
            playerName.text = $"{connectionToClient.identity.GetComponent<MyNetworkPlayer>().GetDisplayName()} WON";
            gameOverObject.SetActive(true);
            canvas.SetActive(true);
            return;
        }

        if (isMyTurn)
        {
            SendQuestion();
        }
    }

    #endregion

    #region Question

    private void SelectQuestion()
    {
        int questionIndex = Random.Range(0, topicPool.Count);
        currQuestion = topicPool[questionIndex];
    }

    public void SetQuestion()
    {
        if (!currQuestion.isQuestionMultipleChoice)
        {
            string text = currQuestion.questionText.Replace("Newline", "\n");
            questionText.text = text;
        }
        else
        {
            questionText.text = currQuestion.questionText;
        }


        if (currQuestion.isQuestionMultipleChoice)
        {
            multiAnswerObject.SetActive(true);
            int i = 0;
            foreach (Text buttonText in answerButtonTexts)
            {
                buttonText.text = currQuestion.choices[i];
                i++;
            }
        }
        else
        {
            inputAnswerObject.SetActive(true);
        }
        canvas.SetActive(true);
    }
    
    public void AnswerQuestion(int choiceIndex)
    {
        canvas.SetActive(false);
        multiAnswerObject.SetActive(false);
        if (currQuestion.correctChoiceIndex == choiceIndex)
        {
            Debug.Log("Correct!");
            playerPoint++;
            TrueAnswer();
        }
        currQuestion = null;
    }

    public void AnswerQuestion()
    {
        canvas.SetActive(false);
        inputAnswerObject.SetActive(false);
        if (currQuestion.questionAnswerInput == inputField.text)
        {
            Debug.Log("Correct!");
            playerPoint++;
            TrueAnswer();
        }
        currQuestion = null;
    }

    public void SendQuestion()
    {
        SelectQuestion();
        SetQuestion();
    }


    #endregion
}
