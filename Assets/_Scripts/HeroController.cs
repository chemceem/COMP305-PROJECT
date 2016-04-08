using UnityEngine;
using System.Collections;

/*
 * Controller class for the Hero
 * Description : Class that controls the player movement and the collisions
 * */

//velocity range utility class
[System.Serializable]
public class VelocityRange {

	//PRIVATE INSTANCE VARIABLES
	public float minVelocity;
	public float maxVelocity;

	//PUBLIC INSTANCE VARIABLES

	//CONSTRUCTOR MEHTODS
	public VelocityRange(float min, float max){
		this.maxVelocity = max;
		this.minVelocity = min;	
	}
}

public class HeroController : MonoBehaviour {

	//PRIVATE INSTANCE VARIABLES
	private Transform _transform;	//this transform variable is to reference the hero gameobject
	private Animator _animator;
	private Rigidbody2D _rigidBody2D;
	private float _move;
	private float _jump;
	private bool _facingRight;
	private bool _isGrounded;

	//PUBLIC INSTANCE VARIABLES
	public VelocityRange velocityRange;
	public float moveForce;
	public float jumpForce;
	public Transform groundCheck;
	public Transform cameraObject;
	public GameObject[] floatObjects;
	public Vector3[] floatPositions;

	// Use this for initialization
	void Start () {
		//initialise public instance variables
		this.velocityRange = new VelocityRange(700f, 5000f);	
		this.moveForce = 800;
		this.jumpForce = 26000f;

		//initialise private instance variables
		this._transform = gameObject.GetComponent<Transform> ();
		this._animator = gameObject.GetComponent<Animator> ();
		this._rigidBody2D = gameObject.GetComponent<Rigidbody2D> ();
		this._move = 0f;
		this._jump = 0f;
		this._facingRight = true;
		this.floatPositions = new Vector3[3];

		this.getPosition ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		Vector3 currentPosition = new Vector3 (this._transform.position.x+150f, this._transform.position.y+0f, -1f);
		this.cameraObject.position = currentPosition;

		this._isGrounded = Physics2D.Linecast (this._transform.position, this.groundCheck.position,
			1 << LayerMask.NameToLayer("ground"));

		float forceX = 0f;
		float forceY = 0f;

		//absolute value of velocity of the gameObject
		float absVelocityX = Mathf.Abs (this._rigidBody2D.velocity.x);
		float absVelocityY = Mathf.Abs (this._rigidBody2D.velocity.y);

		//if the hero is on the ground
		if (this._isGrounded) {
			
			this._move = Input.GetAxis ("Horizontal");
			this._jump = Input.GetAxis ("Vertical");

			if (this._move != 0) {
				//if the user want to turn the hero to the right direction
				if(this._move > 0){
					if(absVelocityX < this.velocityRange.maxVelocity){
						forceX = this.moveForce;
					}
					this._facingRight = true;
					this._flip ();
				}
				//if the user want to turn the hero to left direction
				if(this._move < 0 ){
					if(absVelocityX < this.velocityRange.maxVelocity){
						forceX = -this.moveForce;
					}
					this._facingRight = false;
					this._flip ();
				}
				this._animator.SetInteger ("AnimState", 1);
			}else {
				this._animator.SetInteger ("AnimState", 0);
			}

			if (this._jump > 0) {
				if(absVelocityY < this.velocityRange.maxVelocity){
					//this._jumpSound.Play ();
					forceY = this.jumpForce;
				}
				this._animator.SetInteger ("AnimState", 2); 
			}
		} else {

			//gets a number between -1 to 1 for horizontal and vertical axes movement
			this._move = Input.GetAxis ("Horizontal");
			this._jump = Input.GetAxis ("Vertical");

			if (this._move != 0) {
				//if the user want to turn the hero to the right direction
				if (this._move > 0) {	
					if(absVelocityX < this.velocityRange.maxVelocity){
						forceX = this.moveForce*0.35f;
					}
					this._facingRight = true;
					this._flip ();
				}
				//if the user want to turn the hero to left direction
				if (this._move < 0) { 
					if(absVelocityX < this.velocityRange.maxVelocity){
						forceX = -this.moveForce*0.35f;
					}
					this._facingRight = false;
					this._flip ();
				}
			}
			//call the hero_jumb animation
			this._animator.SetInteger ("AnimState", 2);
		}
		this._rigidBody2D.AddForce (new Vector2 (forceX, forceY));
	}

	void OnCollisionEnter2D(Collision2D other) {
		if(other.gameObject.CompareTag("death")) {
			//this.gameController.LivesValue--;
			//this._deathSound.Play ();
			this._playerDeath ();
		}

		if (other.gameObject.CompareTag ("enemy")) {
			//this.gameController.LivesValue--;
			//this._deathSound.Play ();
			//this._playerDeath ();
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		
		if (other.gameObject.CompareTag ("coin")) {
			//this._coinSound.Play ();
			Destroy (other.gameObject);
			//this.gameController.ScoreValue += 100;
		}

		if (other.gameObject.CompareTag ("enemy")) {
			Destroy (other.gameObject);
			//this._enemyDeath.Play ();
		}

		//when the player reaches the final door
		if (other.gameObject.CompareTag ("door")) {
			//gameController.finishGame ();
		}
	}

	//PRIVATE METHODS

	//method to change the direction of the hero (left or right)
	private void _flip() {
		if (this._facingRight) {
			this._transform.localScale = new Vector2 (2,2);
		}
		else {
			this._transform.localScale = new Vector2 (-2,2);
		}
	}

	//method that takes care of the player death (fall or enemy)
	private void _playerDeath(){
		if (this._transform.position.x < 1400) {
			this._spawn (-366, 600, 0);
		} 
		if( this._transform.position.x > 1400f && this._transform.position.x < 3355) {
			//this._spawnBridge ();
			Debug.Log("spawn");
			this._spawn (1520, 600, 0);
		}
		if (this._transform.position.x > 3355) {
			this._spawn (3355, 600, 0);
			this._spawnFloats ();
		}
	}
	private void _spawn(float x, float y, float z){		
		this._transform.position = new Vector3 (x, y, z);
	}

	private void _spawnFloats(){
		floatObjects = GameObject.FindGameObjectsWithTag ("float");
		foreach(GameObject floatObject in floatObjects){
			floatObject.gameObject.SetActive (false);
			floatObject.transform.rotation = Quaternion.Euler (0,0,0); 
			floatObject.transform.position = new Vector3 (this.floatPositions[0].x, this.floatPositions[0].y, 0 );
			Instantiate (floatObject, floatObject.transform.position, floatObject.transform.rotation);
			floatObject.gameObject.SetActive (true);
		}
	}

	private void getPosition(){
		floatObjects = GameObject.FindGameObjectsWithTag ("float");
		int i = 0;
		foreach (GameObject floatObject in floatObjects) {			
			//this.floatPositions [i] = floatObject.GetComponent<Transform> ().position;
			//Debug.Log(floatObject.GetComponent<Transform> ().position);
			this.floatPositions [i].x = 3289f;//floatObject.GetComponent<Transform> ().position.x;
			this.floatPositions [i].y = 359.72499f;//floatObject.GetComponent<Transform> ().position.y;
			i++;
				//this._transform = gameObject.GetComponent<Transform> ();
		}
	}

}
