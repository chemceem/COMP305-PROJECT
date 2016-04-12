using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*
 * Controller class for the Ga,e
 * Description : Class that controls the game viz. points earned, lives left etc
 * */
public class GameController : MonoBehaviour {

	//PRIVATE INSTANCE VARIABLES
	private int _scoreValue;
	private int _lifeValues;

	//PUBLIC INSTANCE VARIABLES
	public Text LivesText;
	public Text ScoreText;
	public Text GameoverText;
	public Text HighScoreText;
	public Text FinishGameText;
	public Button RestartButton;
	public HeroController heroController;

	//PUBLIC ACCESS METHODS
	public int ScoreValue{
		get { 
			return _scoreValue;
		}
		set { 
			this._scoreValue = value;
			this.ScoreText.text = "Score : " + this._scoreValue;
		}
	}

	public int LivesValue {
		get { 
			return _lifeValues;
		}
		set{
			this._lifeValues = value;
			if (this._lifeValues <= 0) {
				this._endGame ();
			} else {
				this.LivesText.text = "Lives  : " + this._lifeValues;
			}
		}
	}

	// Use this for initialization
	void Start () {
		this._initialize ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//PRIVATE METHODS
	//INITIALIZE METHOD
	private void _initialize(){
		this.ScoreValue = 0;
		this.LivesValue = 5;
		this.GameoverText.enabled = false;
		this.HighScoreText.enabled = false;
		this.RestartButton.gameObject.SetActive(false);
		this.FinishGameText.enabled = false;
	}

	//THIS METHOD IS CALLED WHEN THE PLAYER HAS LOST ALL HIS LIVES
	private void _endGame(){		
		this.HighScoreText.text = "Score : " + this._scoreValue;
		this.GameoverText.enabled = true;
		this.ScoreText.enabled = false;
		this.HighScoreText.enabled = true;
		this.RestartButton.gameObject.SetActive(true);
		this.LivesText.enabled = false;
		this.heroController.gameObject.SetActive(false);
		this.heroController.cameraObject.position = new Vector3 (1,1,-10);
	}

	//PUBLIC METHOD

	//THIS METHOD IS CALLED WHEN THE PLAYER REACHES THE FINISH POINT
	public void finishGame(){
		this.ScoreText.enabled = false;
		this.LivesText.enabled = false;
		this.HighScoreText.text = "Score : " + this._scoreValue;
		this.HighScoreText.enabled = true;
		this.FinishGameText.enabled = true;
		this.RestartButton.gameObject.SetActive(true);
		this.heroController.gameObject.SetActive(false);
		this.heroController.cameraObject.position = new Vector3 (1,1,-10);
	}

	//CALLED WHEN THE RESTART BUTTON IS CLICKED. WILL RESTART THE GAME
	public void RestartButtonClick(){				
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
