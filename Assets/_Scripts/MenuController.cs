using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//EventHandler for StartButton Click
	public void StartButtonClick(){
		SceneManager.LoadScene ("Level1");
	}

	//EventHandler for InstructionButton Click
	public void InstructionsButtonClick(){
		SceneManager.LoadScene ("Instructions");
	}

	//EventHandler for QuitButton Click
	public void QuitButtonClick(){
		Application.Quit ();
	}
}
