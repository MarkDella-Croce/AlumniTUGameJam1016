using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class LevelManager : MonoBehaviour {

    public string[][] slotNames = new string[3][];

    public List<Color> randomColors = new List<Color>();

    public int questionsPerTest;
    public int maxSlots;

    public List<GameObject> starterReagents;

    public Teacher teacher;

    public GameObject[] test;

    private List<Reagent> availableReagents = new List<Reagent>();

    [SerializeField]
    private MixSlot[] resultSlots;

    public AlchemistCharacter player;

    void Awake() {
        slotNames[0] = new string[6] { "First1", "First2", "First3", "First4", "First5", "First6" };
        slotNames[1] = new string[6] { "Second1", "Second2", "Second3", "Second4", "Second5", "Second6" };
        slotNames[2] = new string[6] { "Third1", "Third2", "Third3", "Third4", "Third5", "Third6" };
        
        starterReagents = GameObject.FindGameObjectsWithTag("Reagent").Cast<GameObject>().ToList();
        teacher = GameObject.FindGameObjectWithTag("Teacher").GetComponent<Teacher>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<AlchemistCharacter>();
        teacher.reagentSlots = starterReagents;
        Random.InitState((int)System.DateTime.Now.Ticks);
    }

    // Use this for initialization
    void Start () {
        randomColors = new List<Color> { Color.red, Color.blue, Color.green };
        for (int reagentNum = 0; reagentNum <= starterReagents.Count - 1; reagentNum++) {            
            Reagent newReagent = new Reagent();
            for (int nameSlot = 0; nameSlot < maxSlots; nameSlot++) {
                int randomWord = Random.Range(0, slotNames[nameSlot].Length);
                newReagent.slotNames.Add(slotNames[nameSlot][randomWord]);                                
            }
            int randomColorNum = Random.Range(0, randomColors.Count);
            newReagent.color = randomColors[randomColorNum];
            randomColors.Remove(randomColors[randomColorNum]);
            availableReagents.Add(newReagent);
            starterReagents[reagentNum].GetComponent<MixSlot>().changeReagent(newReagent);
        }
                
        for (int i = 0; i < questionsPerTest; i++) {
            string newQuestion = "";    
            int questionSlots = Random.Range(2, maxSlots + 1);
            List<Reagent> currentReagents = new List<Reagent>(availableReagents);            
            for (int j = 0; j < questionSlots; j++) {
                int randomReagent = Random.Range(0, currentReagents.Count);
                newQuestion = newQuestion + " " + currentReagents[randomReagent].slotNames[j];
                currentReagents.RemoveAt(randomReagent);
            }
            if (teacher.testQuestions.Count != 0 && teacher.testQuestions[teacher.testQuestions.Count - 1] == newQuestion) {
                i--;
            } else {
                teacher.testQuestions.Add(newQuestion);
            }
        }

        /*foreach (string question in teacher.testQuestions) {
            Debug.Log("Question: " + question);
        }*/
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ResetLevel() {
        Time.timeScale = 1f;
        availableReagents = new List<Reagent>();
        teacher.testQuestions = new List<string>();
        player.ResetPlayer();
        foreach (MixSlot slot in resultSlots) {
            slot.changeReagent(null);
        } 
        Start();
    }
}
