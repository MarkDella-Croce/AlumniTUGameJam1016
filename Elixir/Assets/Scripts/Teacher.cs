using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Teacher : MonoBehaviour {

    public int numberOfQuestions;
    public float totalTime;
    public float totalGradeTime;

    public List<string> testQuestions = new List<string>();

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

    public List<GameObject> reagentSlots;

    [SerializeField]
    private GameObject firePrefab;
    


    // Use this for initialization
    void Start () {
	    if (testQuestions.Count > 0) {
            currentQuestionNumber = 0;
            currentQuestion = testQuestions[currentQuestionNumber];
            questionUI.text = "<i>" + currentQuestion + "</i>";
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
        if (submission.isMixed) {
            submissionUI.text = submission.slotNames[0];
            if (submission.slotNames[0] == currentQuestion) {
                gradeUI.enabled = true;
                gradeUI.text = "CORRECT!";
                gradeUI.color = Color.green;
                if (currentQuestionNumber >= testQuestions.Count - 1) {
                    testResultUI.enabled = true;
                    testResultUI.text = "YOU PASSED!";
                    testResultUI.color = Color.green;
                } else {
                    currentQuestionNumber++;
                    currentQuestion = testQuestions[currentQuestionNumber];
                    questionUI.text = "<i>" + currentQuestion + "</i>";
                }
            } else {
                gradeUI.enabled = true;
                gradeUI.text = "INCORRECT!";
                gradeUI.color = Color.red;
                BadThings();
            }
            gradeTimerLeft = totalGradeTime;
        } else {
            gradeUI.enabled = true;
            gradeUI.text = "MIX SOMETHING!";
            gradeUI.color = Color.red;
            BadThings();
            foreach (GameObject slot in reagentSlots) {
                MixSlot mixSlot = slot.GetComponent<MixSlot>();
                if (mixSlot.currentReagent == null) {
                    mixSlot.changeReagent(submission);
                    break;
                }
            }
        }
         
    }

    public void BadThings() {        
        int badThingNumber = Random.Range(1, 4);
        switch (badThingNumber) {
            case 1:
                //Generate Random Fires

                break;
            case 2:
                //Start ReAgents Moving Randomly
                break;
            case 3:
                //Explode on Player (stuns player)
                break;
            default:
                Debug.Log("Undefined Bad Thing!");
                break;
        }
    }
}
