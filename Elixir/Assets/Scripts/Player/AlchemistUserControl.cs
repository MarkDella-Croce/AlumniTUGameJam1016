﻿using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(ThirdPersonCharacter))]
public class AlchemistUserControl : MonoBehaviour {
    
    private ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
    private AlchemistCharacter m_Alchemist; // A reference to the AlchemistCharacter on the object
    private Transform m_Cam;                  // A reference to the main camera in the scenes transform
    private Vector3 m_CamForward;             // The current forward direction of the camera
    private Vector3 m_Move;
    private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.
    private Animator spriteBoy;

    private void Start() {
        // get the transform of the main camera
        if (Camera.main != null) {
            m_Cam = Camera.main.transform;
        } else {
            Debug.LogWarning(
                "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
            // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
        }

        // get the third person character ( this should never be null due to require component )
        m_Character = GetComponent<ThirdPersonCharacter>();
        m_Alchemist = GetComponent<AlchemistCharacter>();
        spriteBoy = transform.FindChild("SpriteBoy").gameObject.GetComponent<Animator>();
}


    private void Update() {
        if (!m_Jump) {
            m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
        }
        //Debug.Log("Fire 1: " + CrossPlatformInputManager.GetButtonDown("Fire1"));
        if (CrossPlatformInputManager.GetButtonDown("Fire1")) {
            Debug.Log("Fire 1");
            m_Alchemist.PickupReagent();
        }

        //Drop Slot 1
        if (CrossPlatformInputManager.GetButtonDown("Fire2")) {
            m_Alchemist.DropReagent(1);
        }

        //Drop Slot 2
        if (CrossPlatformInputManager.GetButtonDown("Fire3")) {
            m_Alchemist.DropReagent(2);
        }

        //Drop Slot 2
        if (CrossPlatformInputManager.GetButtonDown("Fire4")) {
            Debug.Log("Submit!");
            m_Alchemist.Submit();
        }
    }


    // Fixed update is called in sync with physics
    private void FixedUpdate() {

        if (this.transform.position.y > -.5) {
            if (!m_Alchemist.isOnFire) {
                // read inputs
                float h;
                float v;
                if (!m_Alchemist.isDizzy) {
                    h = CrossPlatformInputManager.GetAxis("Horizontal");
                    v = CrossPlatformInputManager.GetAxis("Vertical");
                } else {
                    h = CrossPlatformInputManager.GetAxis("Vertical");
                    v = CrossPlatformInputManager.GetAxis("Horizontal");
                }
                spriteBoy.SetFloat("Horz", Mathf.Abs(h));
                spriteBoy.SetFloat("Vert", Mathf.Abs(v));
                bool crouch = false;

                // calculate move direction to pass to character
                if (m_Cam != null) {
                    // calculate camera relative direction to move:
                    m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
                    m_Move = v * m_CamForward + h * m_Cam.right;
                } else {
                    // we use world-relative directions in the case of no main camera
                    m_Move = v * Vector3.forward + h * Vector3.right;
                }
#if !MOBILE_INPUT
                // walk speed multiplier
                if (Input.GetKey(KeyCode.LeftShift)) m_Move *= 0.5f;
#endif

                // pass all parameters to the character control script
                m_Character.Move(m_Move, crouch, m_Jump);
                m_Jump = false;
            }
        } else {
            this.transform.position = m_Alchemist.StartPosition.position;
        }
    }
}
