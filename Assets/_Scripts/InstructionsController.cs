using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class InstructionsController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//EventHandler for BackButton Click
	public void BackButtonClick(){
		SceneManager.LoadScene ("Menu");
	}
}
