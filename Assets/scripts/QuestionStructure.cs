using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionStructure : MonoBehaviour
{
    //Multiple choice structure.
    // Attention!! Topics are case sensitive.
    //Do not change
    public string questionText;
    public List<string> choices;
    [Range(10, 99)] public int answerTime = 50;
    public int correctChoiceIndex = 0;
    public string questionTopic = "";
    public bool isQuestionMultipleChoice = true;
    public string questionAnswerIfNotMultipleChoice;
}
