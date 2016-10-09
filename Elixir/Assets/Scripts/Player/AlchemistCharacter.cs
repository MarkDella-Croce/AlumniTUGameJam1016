using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameObjectComparer : IComparer<GameObject> {
    public int Compare(GameObject x, GameObject y) {
        return (new CaseInsensitiveComparer()).Compare(x.name, y.name);
    }    
}

public class AlchemistCharacter : MonoBehaviour {
    //Active Pickup Slot the player is in front of    
    public MixSlot currentActiveContainer;

    //Carry Slots
    [SerializeField]
    private List<CarrySlot> carrySlots = new List<CarrySlot>();
    
    public Transform StartPosition;

    public bool isOnFire = false;
    public bool isDizzy = false;

    // Use this for initialization
    void Start() {
        GameObject[] slots = GameObject.FindGameObjectsWithTag("UICarrySlot");
        Array.Sort(slots, new GameObjectComparer());
        foreach(GameObject slot in slots) {
            if (slot.activeInHierarchy == true) {                
                CarrySlot carrySlot = new CarrySlot();
                carrySlot.carrySlotUI = slot.GetComponent<Image>();
                carrySlot.changeReagent(null);
                carrySlots.Add(carrySlot);
            }
        }
    }

    // Update is called once per frame
    void Update() {

    }

    public void PickupReagent() {
        if (currentActiveContainer != null && currentActiveContainer.canSend && currentActiveContainer.currentReagent != null) {              
            foreach (CarrySlot carrySlot in carrySlots) {                
                if (carrySlot.reagent == null) {                                        
                    carrySlot.changeReagent(currentActiveContainer.currentReagent);
                    currentActiveContainer.changeReagent(null);
                    break;
                }
            }
        }
    }

    public void DropReagent(int slotNumber) {
        if (currentActiveContainer != null && currentActiveContainer.canReceive && currentActiveContainer.currentReagent == null && carrySlots.Count >= slotNumber && carrySlots[slotNumber - 1].reagent != null) {
            currentActiveContainer.changeReagent(carrySlots[slotNumber - 1].reagent);
            carrySlots[slotNumber-1].changeReagent(null);
        }                   
    }

    public void Submit() {
        if (currentActiveContainer != null) {
            currentActiveContainer.Submit();
        }
    }

    public void ResetPlayer() {
        foreach (CarrySlot carrySlot in carrySlots) {
            carrySlot.changeReagent(null);
            transform.position = StartPosition.position;
        }
    }
}