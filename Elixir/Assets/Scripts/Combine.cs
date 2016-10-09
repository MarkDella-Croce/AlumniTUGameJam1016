using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Combine : MonoBehaviour {

    [SerializeField]
    private GameObject combineTable;

    private List<MixSlot> mixSlots = new List<MixSlot>();

	// Use this for initialization
	void Start () {
       foreach (Transform child in combineTable.transform) {
            if (child.gameObject.tag == "Mix") {
                mixSlots.Add(child.gameObject.GetComponent<MixSlot>());
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public Reagent combine() {
        Reagent mixedReagent = new Reagent();
        Color mixedColor = new Color();
        string mixedName = "";
        int currentSlot = 0;
        foreach(MixSlot mixSlot in mixSlots) {
            if (mixSlot.currentReagent != null) {
                //Debug.Log("Mixing: " + mixSlot.currentReagent.color + " and " + mixSlot.currentReagent.slotNames[currentSlot]);
                mixedColor += mixSlot.currentReagent.color;
                mixedName = mixedName + " " + mixSlot.currentReagent.slotNames[currentSlot];
                currentSlot++;
            }
        }
        mixedReagent.slotNames.Add(mixedName);
        mixedReagent.color = mixedColor;
        mixedReagent.isMixed = true;
        return mixedReagent;
    }

    
}
