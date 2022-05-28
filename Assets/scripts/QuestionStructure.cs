using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionStructure : MonoBehaviour
{
    public string questionText;
    public List<string> choices;
    [Range(10, 99)] public int answerTime = 50;
    public int correctChoiceIndex = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
