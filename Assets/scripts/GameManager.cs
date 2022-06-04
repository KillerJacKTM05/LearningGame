using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
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


    public bool gameStart = false;
    public bool questionDisplayed = false;
    public bool timeIsUp = false;
    [Range(0, 50f)] public float startGameDelay = 5f;

    [SerializeField]private string player1Topic;
    [SerializeField]private string player2Topic;

    public GameObject displayedQuestion;
    private void OnGUI()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    void Start()
    {
        QuestionPool.instance.AddTopic("python");
        QuestionPool.instance.AddTopic("java");
        QuestionPool.instance.AddTopic("C#");
        MenuManager.instance.GetTopicChoices();
        MenuManager.instance.SetTopicChoices(0);
        QuestionPool.instance.AddQuestionToPool();
        //QuestionPool.instance.WriteTopicCount();
        gameStart = true;
        StartCoroutine(StartGameDelay());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public string GetPlayerTopicName(int index)       //1 or 2
    {
        if(index == 1)
        {
            return player1Topic;
        }
        else if(index == 2)
        {
            return player2Topic;
        }
        else
        {
            return " ";
        }
    }
    public void SetPlayerTopicName(int index, string name)      //1 or 2
    {
        if(index == 1)
        {
            player1Topic = name;
        }
        else if (index == 2)
        {
            player2Topic = name;
        }
    }
    private IEnumerator StartGameDelay()
    {
        yield return new WaitForSeconds(startGameDelay);
        MenuManager.instance.getTopicsPanel().SetActive(true);

    }
}
