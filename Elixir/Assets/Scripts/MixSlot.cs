using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MixSlot : MonoBehaviour {
    [SerializeField]
    private Light ActiveSlot;    

    [SerializeField]
    private bool isActive;
       
    public Reagent currentReagent;

    private MeshRenderer renderer;

    [SerializeField]
    private Combine mixer;

    [SerializeField]
    private Teacher teacher;

    public bool canReceive;
    public bool canSend;

    // Use this for initialization
    void Start () {
        renderer = this.GetComponent<MeshRenderer>();
        if (renderer.material.color != Color.clear) {
            currentReagent = new Reagent();
            currentReagent.color = renderer.material.color;
            currentReagent.slotNames.Add(Random.Range(1, 99).ToString());
            currentReagent.slotNames.Add(Random.Range(1, 99).ToString());
            currentReagent.slotNames.Add(Random.Range(1, 99).ToString());
        } else {
            currentReagent = null;
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") {
            isActive = true;
            ActiveSlot.enabled = true;
            AlchemistCharacter pc = other.gameObject.GetComponent<AlchemistCharacter>();
            pc.currentActiveContainer =this;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            isActive = false;
            ActiveSlot.enabled = false;
            AlchemistCharacter pc = other.gameObject.GetComponent<AlchemistCharacter>();
            pc.currentActiveContainer = null;
        }
    }

    public void changeReagent(Reagent newReagent) {
        if (newReagent != null) {
            Debug.Log("Changing to " + newReagent.color);
            currentReagent = newReagent;
            renderer.material.color = currentReagent.color;
        } else {
            currentReagent = null;
            renderer.material.color = Color.clear;
        }
    }

    public void Submit() {
        if (mixer != null) {
            changeReagent(mixer.combine());
        } else {
            if (teacher != null) {
                if (currentReagent != null) {
                    teacher.Grade(currentReagent);
                    changeReagent(null);
                }
            }
        }

    }
}
