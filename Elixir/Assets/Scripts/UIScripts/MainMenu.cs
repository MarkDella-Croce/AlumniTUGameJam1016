using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void StartGame() {
        Application.LoadLevel("Level1");
    }

	public void WelcomeScreen() {
		Application.LoadLevel("Welcome_Screen");
	}

	public void Credits() {
		Application.LoadLevel("Credits_Screen");
	}
}
