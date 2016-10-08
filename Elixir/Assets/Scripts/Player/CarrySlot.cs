using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CarrySlot {

    public Image carrySlotUI;
    public Reagent reagent;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void changeReagent(Reagent newReagent) {
        if (newReagent != null) {
            reagent = newReagent;
            carrySlotUI.color = reagent.color;
        } else {
            reagent = null;
            carrySlotUI.color = Color.clear;
        }
        
    }
}
