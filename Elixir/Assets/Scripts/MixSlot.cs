using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MixSlot : MonoBehaviour {
    [SerializeField]
    private Light ActiveSlot;    

    [SerializeField]
    private bool isActive;
       
    public Reagent currentReagent = new Reagent();

    private MeshRenderer renderer;

    [SerializeField]
    private Combine mixer;

    [SerializeField]
    private Teacher teacher;

    public bool canReceive;
    public bool canSend;

    public float moveTimeLeft;
    public float totalMoveTime;
    private Vector3 currentDest;

    

    public float movementScale;
    public float speed;


    // Use this for initialization
    void Start () {
        renderer = this.GetComponent<MeshRenderer>();
        if (renderer.material.color == Color.clear) {                    
            currentReagent = null;
        }        
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
            if (pc.currentActiveContainer == this) {
                pc.currentActiveContainer = null;
            }
            
        }
    }

    public void changeReagent(Reagent newReagent) {
        if (newReagent != null) {            
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
