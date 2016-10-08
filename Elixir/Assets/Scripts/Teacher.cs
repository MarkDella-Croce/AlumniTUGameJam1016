using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Teacher : MonoBehaviour {

    public int numberOfQuestions;
    public float totalTime;
    public float totalGradeTime;

    public string[] testQuestions;

    public string currentQuestion;

    [SerializeField]
    private int currentQuestionNumber;

    float timeLeft;

    [SerializeField]
    private Text questionUI;

    [SerializeField]
    private Text submissionUI;

    [SerializeField]
    private Text gradeUI;

    [SerializeField]
    private Text testResultUI;

    [SerializeField]
    private Text timerUI;

    [SerializeField]
    private float gradeTimerLeft;


    // Use this for initialization
    void Start () {
	    if (testQuestions.Length > 0) {
            currentQuestionNumber = 0;
            currentQuestion = testQuestions[currentQuestionNumber];
            questionUI.text = currentQuestion;
        } else {
            Debug.Log("Need at least one question!");
        }
        timeLeft = totalTime;
        testResultUI.enabled = false;
        gradeUI.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
        timeLeft -= Time.deltaTime;                
        if (timeLeft <= 0f) {
            timerUI.text = "00.00";
            testResultUI.enabled = true;
            testResultUI.text = "YOU FAILED!";
            testResultUI.color = Color.red;
            ;
        } else {
            timerUI.text = timeLeft.ToString("00.00");
        }
        if (gradeTimerLeft > 0) {
            gradeTimerLeft -= Time.deltaTime;
            if (gradeTimerLeft <= 0f) {
                gradeTimerLeft = 0;
                gradeUI.enabled = false;
            }
        }

	}

    public void Grade (Reagent submission) {
        submissionUI.text = submission.slotNames[0];
        if (submission.slotNames[0] == currentQuestion) {
            gradeUI.enabled = true;
            gradeUI.text = "CORRECT!";
            gradeUI.color = Color.green;
            if (currentQuestionNumber >= testQuestions.Length - 1) {
                testResultUI.enabled = true;
                testResultUI.text = "YOU PASSED!";
                testResultUI.color = Color.green;
            } else {
                currentQuestionNumber++;
                currentQuestion = testQuestions[currentQuestionNumber];
            }
        } else {
            gradeUI.enabled = true;
            gradeUI.text = "INCORRECT!";
            gradeUI.color = Color.red;
            //Trigger Bad Things here
        }
        gradeTimerLeft = totalGradeTime;
    }
}
