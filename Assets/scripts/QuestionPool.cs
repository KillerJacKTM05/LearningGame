using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Topics
{
    public string topicName;
    public List<GameObject> questions;
    public Topics(string name)
    {
        topicName = name;
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
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AddTopic(string topName)
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
    public void AddQuestionToPool(string topName, GameObject question)
    {
        int size = Topic.Count;
        if (Topic.Count != 0)
        {
            for (int i = 0; i < Topic.Count; i++)
            {
                if (Topic[i].topicName == topName)
                {
                    Topic[i].questions.Add(question);
                }
            }
            if(size < Topic.Count)
            {
                Debug.LogError("There is no suitable topic found with entered topic name. No question added.");
            }
        }
        else if(Topic.Count == 0)
        {
            Debug.LogError("There is no topic on the list. No question added.");
        }
    }
    public void WriteTopicCount()
    {
        Debug.Log("Topics Count:" + Topic.Count);
    }
}
