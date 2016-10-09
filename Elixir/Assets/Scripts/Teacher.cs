using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityStandardAssets.Utility;

public class Teacher : MonoBehaviour {

    public int numberOfQuestions;
    public float totalTime;
    public float totalGradeTime;

    public float restartGameTotalTime;

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

    [SerializeField]
    private float restartGameTimeLeft;

    public List<GameObject> reagentSlots;


    //Fires
    [SerializeField]
    private int numberOfFires;

    [SerializeField]
    private GameObject firePrefab;
    [SerializeField]
    private GameObject dizzyPrefab;

    [SerializeField]
    private List<GameObject> activeFires;

    float fireTimeLeft;    
    float totalFireTime;

    float dizzyTimeLeft;
    [SerializeField]
    float totalDizzyTime;

   
    float onFireTimeLeft;
    [SerializeField]
    float totalOnFireTime;

    LevelManager levelManager;

    GameObject player;

    GameObject fireEffect;
    GameObject dizzyEffect;

    // Use this for initialization
    void Start () {
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        SetupLevel();
    }

    void SetupLevel() {
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
        if (timeLeft > 0) {
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0f) {
                timeLeft = 0;
                timerUI.text = "00.00";
                testResultUI.enabled = true;
                testResultUI.text = "YOU FAILED!";
                testResultUI.color = Color.red;
                restartGameTimeLeft = restartGameTotalTime;
                ;
            } else {
                timerUI.text = timeLeft.ToString("00.00");
            }
        }
        if (gradeTimerLeft > 0) {
            gradeTimerLeft -= Time.deltaTime;
            if (gradeTimerLeft <= 0f) {
                gradeTimerLeft = 0;
                gradeUI.enabled = false;
            }
        }
        if (restartGameTimeLeft > 0) {
            restartGameTimeLeft -= Time.deltaTime;
            if (restartGameTimeLeft <= 0f) {
                restartGameTimeLeft = 0;
                testResultUI.enabled = false;
                gradeUI.enabled = false;
                levelManager.ResetLevel();
                SetupLevel();
            }
        }
        if(dizzyTimeLeft > 0) {
            dizzyTimeLeft -= Time.deltaTime;
            if (dizzyTimeLeft <= 0f) {
                dizzyTimeLeft = 0;
                player.GetComponent<AlchemistCharacter>().isDizzy = false;
                Destroy(dizzyEffect);
                dizzyEffect = null;
            }
        }
        if (onFireTimeLeft > 0) {
            onFireTimeLeft -= Time.deltaTime;
            if (onFireTimeLeft <= 0f) {
                onFireTimeLeft = 0;
                player.GetComponent<AlchemistCharacter>().isOnFire = false;
                Destroy(fireEffect);
                fireEffect = null;
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
                    restartGameTimeLeft = restartGameTotalTime;
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
        Debug.Log("Bad Thing selected: " + badThingNumber);
        switch (badThingNumber) {
            case 1:
                for (int i = 0; i < numberOfFires; i++) {
                    //Generate Random Fires                
                    bool notValid = true;
                    int counter = 9999999;
                    Vector3 validLocation = new Vector3();
                    while (notValid || counter <= 0) {
                        float fireX = Random.Range(-4f, 4f);
                        float fireZ = Random.Range(-4f, 4f);
                        validLocation = new Vector3(fireX, 0f, fireZ);
                        Collider[] hitColliders = Physics.OverlapSphere(validLocation, .5f);
                        notValid = false;
                        foreach (Collider col in hitColliders) {
                            if (col.gameObject.name != "Floor") {
                                notValid = true;
                                break;
                            }
                        }
                        counter--;
                    }
                    GameObject newFire = (GameObject)Instantiate(firePrefab, validLocation, new Quaternion(0, 0, 0, 0));
                    activeFires.Add(newFire);
                }                                
                break;
            case 2:
                //Player is Dizzy
                if (dizzyEffect == null) {
                    GameObject newDizzy = (GameObject)Instantiate(dizzyPrefab, player.transform.position, new Quaternion(0, 0, 0, 0));
                    newDizzy.transform.parent = player.transform;
                    dizzyEffect = newDizzy;
                }
                dizzyTimeLeft = totalDizzyTime;
                player.GetComponent<AlchemistCharacter>().isDizzy = true;
                break;
            case 3:
                //Player is on fire
                if (fireEffect == null) {
                    GameObject newFire = (GameObject)Instantiate(firePrefab, player.transform.position, new Quaternion(0, 0, 0, 0));
                    newFire.transform.parent = player.transform;
                    newFire.GetComponent<ParticleSystemDestroyer>().minDuration = 30f;
                    newFire.GetComponent<ParticleSystemDestroyer>().maxDuration = 30f;
                    newFire.GetComponent<CapsuleCollider>().enabled = false;
                    fireEffect = newFire;
                }
                onFireTimeLeft = totalOnFireTime;
                player.GetComponent<AlchemistCharacter>().isOnFire = true;
                break;
            default:
                Debug.Log("Undefined Bad Thing!");
                break;
        }
    }
}
