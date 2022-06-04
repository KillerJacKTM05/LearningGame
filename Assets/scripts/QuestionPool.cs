using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Topics
{
    public string topicName;
    public List<GameObject> questions;
    public Topics(string name)
    {
        topicName = name;
        questions = new List<GameObject>();
    }
}
public class QuestionPool : MonoBehaviour
{
    public static QuestionPool instance { get; private set; }
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
    public List<Topics> Topic = new List<Topics>();
    public List<GameObject> QuestionAddListForQuestionPool = new List<GameObject>();
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AddTopic(string topName)                                    //Add topic to the pool. If topic is exists, it will not create duplicate.
    {
        int count = 0;
        if(Topic.Count != 0)
        {
            for (int i = 0; i < Topic.Count; i++)
            {
                if (Topic[i].topicName == topName)
                {
                    count = 1;
                    break;
                }
            }
        }

        if(count == 0)
        {
            Topics temp = new Topics(topName);
            Topic.Add(temp);
        }
    }
    public void AddQuestionToPool()                                         //Automatically adds all the questions that prepared on the inspector tab accroding to their topic names.
    {
        for(int i = 0; i < QuestionAddListForQuestionPool.Count; i++)
        {
            bool topicFound = false;
            int topicIndex = 0;
            QuestionStructure reference = QuestionAddListForQuestionPool[i].GetComponent<QuestionStructure>();
            //Debug.Log("topic:" + reference.questionTopic);
            for(int j = 0; j < Topic.Count; j++)
            {
                if(reference.questionTopic == Topic[j].topicName)
                {
                    //Debug.Log("topic found! at:" + j);
                    topicFound = true;
                    topicIndex = j;
                }
                else { }
            }

            if (topicFound)
            {
                Topic[topicIndex].questions.Add(QuestionAddListForQuestionPool[i]);
            }
            else
            {
                Debug.LogError("No topic match for Question number" + i + ".");
            }
        }
    }
    public void WriteTopicCount()                               //writes total topic count to the console.
    {
        Debug.Log("Topics Count:" + Topic.Count);
    }

    public GameObject GetQuestion(string topicNam)              //returns random question with given topic. Please update the question UI after select a random question.
    {
        int random = 0;
        int index = 0;
        bool topicFound = false;
        if(Topic.Count == 0)
        {
            Debug.LogError("No topic defined. First, insert a topic database.");
        }
        else if(Topic.Count != 0)
        {
            for(int i = 0; i < Topic.Count; i++)
            {
                if(Topic[i].topicName == topicNam)
                {
                    index = i;
                    topicFound = true;
                }
            }
            if (!topicFound)
            {
                Debug.LogError("no topic found with given name.");
            }
        }

        if(Topic[index].questions.Count == 0)
        {
            Debug.LogError("no question found in the database.");
        }
        else if(Topic[index].questions.Count != 0)
        {
            random = Random.Range(0, Topic[index].questions.Count);
            
        }
        return Topic[index].questions[random];
    }

    public void UpdateQuestionPanel(GameObject selected)                //Question displayer
    {
        QuestionStructure reference = selected.GetComponent<QuestionStructure>();
        MenuManager.instance.questionPanel.setTimer(reference.answerTime);
        MenuManager.instance.questionPanel.setQuestionText(reference.questionText);
        MenuManager.instance.questionPanel.setChoices(reference.choices);
    }
}
