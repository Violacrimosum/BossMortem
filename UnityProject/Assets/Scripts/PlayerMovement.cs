using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	enum PlayerState{
		IDLE,
		WALKING,
		DANCING,
		CLEANING,
		PREKISSING,
		KISSING,
		POSTKISSING,
		FALLEN,
		PRETENDING,
		SCOUTING,
		GRABING
	}

	public enum PlayerType{
		SCOUT,
		BEARER
	}

	private bool isFacingLeft = true;
	private float lastPos;
	private bool hasChanged = false;
	public int nextLevel;

	private float painLevel = 0;
	private int downPainLevel;

	//public float en
	public float renfoncement = 5;
	public int deathCoeff;
	public int healCoeff;
	private bool isSuicide = false;
	public bool doSuicide = false;
	private bool startGame = false;

	public GameObject bossBody;
	public GameObject bossBodySelector;

	private Vector3 movementVector;
	private CharacterController characterController;
	private Animator animator;
	private Distractions shObject;
	private Safehouse shObject2;
	private GameObject exitAccess;

	private bool locked = false;

	private PlayerState pState = PlayerState.IDLE;
	public PlayerType pType;

	public GameObject player1;
	public GameObject player2;

	private float movementSpeed = 8;
	private float jumpPower = 15;
	private float gravity = 40;


	// Use this for initialization
	void Start () {
		characterController = GetComponent<CharacterController>();
		animator = GetComponent<Animator> ();
		this.renderer.receiveShadows = true;
		this.renderer.castShadows = true;

		this.painLevel = 0;
		/*if (this.tag == "Player1")
			this.pType = PlayerType.BEARER;
		else
			this.pType = PlayerType.SCOUT;*/
		//print ((this.pType == PlayerType.BEARER) ? "Bearer" : "Scout");
	}

	void OnTriggerEnter (Collider other) {
		if (other.tag == "bossBody" ) {
			bossBodySelector = other.gameObject;
		}
		else if(other.tag == "bossBodyMenu"){
			startGame = true;
		}
		else if(other.tag == "quit"){
			isSuicide = true;
		}
		else if(other.tag == "exit" || other.tag == "lastExit"){
			exitAccess = other.gameObject;
		}
		else if (other.tag == "Alcove" || other.tag == "Danse" || other.tag == "Blackboard") {
						shObject = (Distractions)other.GetComponent (typeof(Distractions));	
		} else if (other.tag == "Closet") {
						shObject2 = (Safehouse)other.GetComponent (typeof(Safehouse));
		} 
	}

	void OnTriggerExit (Collider other) {
		if (other.tag == "bossBody") {
						bossBodySelector = null;
		}
		else if(other.tag == "bossBodyMenu"){
			startGame = true;
		}
		else if(other.tag == "quit"){
			isSuicide = false;
		}
		else if(other.tag == "exit" || other.tag == "lastExit"){
			exitAccess = null;
		}
		else if (other.tag == "Alcove" || other.tag == "Danse" || other.tag == "Blackboard") {
			if(this.pState != PlayerState.DANCING)	
			shObject = null;	
		} else if (other.tag == "Closet") {
			shObject2 = (Safehouse)other.GetComponent (typeof(Safehouse));
		}
	}

	void Flip()
	{		
		// Multiply the player's x local scale by -1
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
	
	// Update is called once per frame
	void Update () {
				//print (""+painLevel);
		if (doSuicide) {
			Application.Quit();
				}
		else{
		if (this.pType == PlayerType.BEARER && this.pState != PlayerState.PRETENDING && this.pState != PlayerState.KISSING && this.pState != PlayerState.DANCING &&
		    this.pState != PlayerState.PREKISSING &&  this.pState != PlayerState.POSTKISSING &&  this.pState != PlayerState.GRABING ) {
						if (Random.value >= 0.5)
								this.painLevel += Random.value / deathCoeff;
						else
								this.painLevel -= Random.value / deathCoeff;

						if (this.tag == "Player1") {
								this.painLevel -= Input.GetAxis ("RightJoystickY_Player1") / healCoeff;
				
						} else {
								this.painLevel -= Input.GetAxis ("RightJoystickY_Player2") / healCoeff;
						}

				}
				if (this.painLevel >= 1 || this.painLevel <= -1) {
						this.movementVector.x = 0;
						this.pState = PlayerState.FALLEN;
				}

				if (this.animator.GetCurrentAnimatorStateInfo (0).IsName ("grab_marco")) {
					if (this.animator.GetCurrentAnimatorStateInfo (0).normalizedTime >= 0.9f)
						this.pState = PlayerState.IDLE;
				}

				if (this.animator.GetCurrentAnimatorStateInfo (0).IsName ("useA_marco")) {
						if (this.animator.GetCurrentAnimatorStateInfo (0).normalizedTime >= 1)
								this.pState = PlayerState.IDLE;
				}
				if (this.animator.GetCurrentAnimatorStateInfo (0).IsName ("dropCorpse_marco")) {
						if (this.animator.GetCurrentAnimatorStateInfo (0).normalizedTime >= 1) {
								this.painLevel = 0;
								this.pState = PlayerState.IDLE;
								this.pType = PlayerType.SCOUT;
								Vector3	tmp = this.transform.position;
								tmp.z = 0f;
								tmp.y += 0.8f;
								this.bossBody.transform.position = tmp;
								Instantiate(this.bossBody);
						}
				}
				if (this.animator.GetCurrentAnimatorStateInfo (0).IsName ("danceA_marco")) {
						locked = false;
				}

				if (this.animator.GetCurrentAnimatorStateInfo (0).IsName ("puppetA_marco")) {
						locked = false;
				}

				if (this.animator.GetCurrentAnimatorStateInfo (0).IsName ("preKiss_marco")) {
						if (this.animator.GetCurrentAnimatorStateInfo (0).normalizedTime >= 1) {
								this.pState = PlayerState.KISSING;
								this.movementVector.z = 0;
						}
				}

				if (this.animator.GetCurrentAnimatorStateInfo (0).IsName ("kiss_marco")) {

				}

				if (this.animator.GetCurrentAnimatorStateInfo (0).IsName ("postKiss_marco")) {
						if (this.animator.GetCurrentAnimatorStateInfo (0).normalizedTime >= 1) {
								this.pState = PlayerState.IDLE;
								this.movementVector.z = 0;
						}
				}

				if (this.pState != PlayerState.SCOUTING && this.pState != PlayerState.KISSING && this.pState != PlayerState.POSTKISSING && 
						this.pState != PlayerState.PREKISSING && this.pState != PlayerState.PRETENDING && this.pState != PlayerState.FALLEN
		    &&  this.pState != PlayerState.GRABING) {
						if (this.tag == "Player1") {
								movementVector.x = Input.GetAxis ("LeftJoystickX_Player1") * movementSpeed;

						} else {
								movementVector.x = Input.GetAxis ("LeftJoystickX_Player2") * movementSpeed;
						}

						if(this.pType == PlayerType.BEARER){
								movementVector.x *= 0.75f; 
							}
						}
						if (this.pState != PlayerState.SCOUTING && this.pState != PlayerState.KISSING && this.pState != PlayerState.POSTKISSING && 
								this.pState != PlayerState.PREKISSING && this.pState != PlayerState.DANCING && this.pState != PlayerState.PRETENDING && this.pState != PlayerState.FALLEN
		    &&  this.pState != PlayerState.GRABING) {
								if (movementVector.x == 0)
										this.pState = PlayerState.IDLE;
								else
										this.pState = PlayerState.WALKING;
						}
						if (movementVector.x > 0) {
								if (this.isFacingLeft) {
										this.isFacingLeft = false;
										hasChanged = true;
								}
						}

						if (movementVector.x < 0) {
								if (!this.isFacingLeft) {
										this.isFacingLeft = true;
										hasChanged = true;
								}
						}

						if (hasChanged) {
								this.Flip ();
								hasChanged = false;	
						}

						if (characterController.isGrounded && this.pState != PlayerState.KISSING && this.pState != PlayerState.PREKISSING && this.pState != PlayerState.POSTKISSING &&
								this.pState != PlayerState.SCOUTING && this.pState != PlayerState.DANCING && this.pState != PlayerState.PRETENDING && this.pState != PlayerState.FALLEN
		    &&  this.pState != PlayerState.GRABING) {
								movementVector.y = 0;

								if (Input.GetButtonDown ("A_Player1") && this.tag == "Player1") {
										print ("test");
										if(bossBodySelector != null){
										if(this.pType == PlayerType.SCOUT){
												this.pState = PlayerState.GRABING;
												this.pType = PlayerType.BEARER;
												this.painLevel = 0;
												Destroy(bossBodySelector);
											}
										}
								else if(isSuicide){
						this.doSuicide = true;
						player2.GetComponent<PlayerMovement>().doSuicide = true;
					}
					else if(startGame){
						Application.LoadLevel(1);
					}
					else if(exitAccess != null){
						if(nextLevel == -1)
							Application.LoadLevel(0);
						else
						Application.LoadLevel(nextLevel);
					}
										else if (shObject != null) {

												movementVector.x = 0;
												string res = shObject.interact ((this.pType == PlayerType.BEARER) ? "Bearer" : "Scout");
												if (res == "kiss") {
														this.pState = PlayerState.PREKISSING;
														this.movementVector.z = renfoncement;
												} else if (res == "scouting")
														this.pState = PlayerState.SCOUTING;
												else if (res == "danse") {
														locked = true;
														this.pState = PlayerState.DANCING;
												} else if (res == "pretend") {
														locked = true;
														this.pState = PlayerState.PRETENDING;
												}
										}
										
								}
								if (Input.GetButtonDown ("A_Player2") && this.tag == "Player2") {	
									if(bossBodySelector != null){
										if(this.pType == PlayerType.SCOUT){
											this.pState = PlayerState.GRABING;
											this.pType = PlayerType.BEARER;
											this.painLevel = 0;
											Destroy(bossBodySelector);
										}
									}
					else if(isSuicide){
						this.doSuicide = true;
						player1.GetComponent<PlayerMovement>().doSuicide = true;
					}
					else if(startGame){
						Application.LoadLevel(1);
					}
					else if(exitAccess != null){
						if(nextLevel == -1)
							Application.LoadLevel(0);
						else
							Application.LoadLevel(nextLevel);
					}
					else if (shObject != null) {
												movementVector.x = 0;
												string res = shObject.interact ((this.pType == PlayerType.BEARER) ? "Bearer" : "Scout");
												if (res == "kiss") {
														this.pState = PlayerState.PREKISSING;
														this.movementVector.z = renfoncement;
												} else if (res == "scouting")
														this.pState = PlayerState.SCOUTING;
												else if (res == "danse") {
														locked = true;
														this.pState = PlayerState.DANCING;
												} else if (res == "pretend") {
														locked = true;
														this.pState = PlayerState.PRETENDING;
												}
										}
								}
								if (Input.GetButtonDown ("B_Player1") && this.tag == "Player1") {
					Application.LoadLevel(0);
								}
								if (Input.GetButtonDown ("B_Player2") && this.tag == "Player2") {
					Application.LoadLevel(0);
				
								}
						}

						if (this.pState == PlayerState.KISSING) {
								if (Input.GetButtonDown ("A_Player1") && this.tag == "Player1") {
										shObject.interact ((this.pType == PlayerType.BEARER) ? "Bearer" : "Scout");
										this.movementVector.z = -renfoncement;
										this.pState = PlayerState.POSTKISSING;
								}
								if (Input.GetButtonDown ("A_Player2") && this.tag == "Player2") {	
										shObject.interact ((this.pType == PlayerType.BEARER) ? "Bearer" : "Scout");
										this.movementVector.z = -renfoncement;
										this.pState = PlayerState.POSTKISSING;
								}
						}
		
						if (this.pState == PlayerState.DANCING && !locked) {
								if (Input.GetButtonDown ("A_Player1") && this.tag == "Player1") {
										shObject.interact ((this.pType == PlayerType.BEARER) ? "Bearer" : "Scout");
										//this.movementVector.z = -renfoncement;
										this.shObject = null;
										this.pState = PlayerState.IDLE;
								}
								if (Input.GetButtonDown ("A_Player2") && this.tag == "Player2") {	
										shObject.interact ((this.pType == PlayerType.BEARER) ? "Bearer" : "Scout");
										//this.movementVector.z = -renfoncement;
										this.shObject = null;
										this.pState = PlayerState.IDLE;
								}
						}

						if (this.pState == PlayerState.PRETENDING && !locked) {
								if (Input.GetButtonDown ("A_Player1") && this.tag == "Player1") {
										shObject.interact ((this.pType == PlayerType.BEARER) ? "Bearer" : "Scout");
										//this.movementVector.z = -renfoncement;
										this.pState = PlayerState.IDLE;
								}
								if (Input.GetButtonDown ("A_Player2") && this.tag == "Player2") {	
										shObject.interact ((this.pType == PlayerType.BEARER) ? "Bearer" : "Scout");
										//this.movementVector.z = -renfoncement;
										this.pState = PlayerState.IDLE;
								}
						}

						movementVector.y -= gravity * Time.deltaTime;

						if (playersTooFar ()) {
								if (this.tag == "Player2") {
										if (player2.transform.position.x > player1.transform.position.x && 
												movementVector.x > 0)
												movementVector.x = 0;
										else if (player2.transform.position.x < player1.transform.position.x && 
												movementVector.x < 0)
												movementVector.x = 0;
								}
								if (this.tag == "Player1") {
										if (player2.transform.position.x > player1.transform.position.x && 
												movementVector.x < 0)
												movementVector.x = 0;
										else if (player2.transform.position.x < player1.transform.position.x && 
												movementVector.x > 0)
												movementVector.x = 0;
								}
						}

						characterController.Move (movementVector * Time.deltaTime);



						if (this.pType == PlayerType.SCOUT) {
								if (this.pState == PlayerState.IDLE) {
										this.animator.Play ("idleA_marco");
								}
								if (this.pState == PlayerState.WALKING) {
										this.animator.Play ("walk_marco");
								}
								if (this.pState == PlayerState.SCOUTING) {
										this.animator.Play ("useA_marco");
								}
						}
						if (this.pType == PlayerType.BEARER) {
								if (this.pState == PlayerState.IDLE) {
										if (painLevel < -0.66f)
												this.animator.Play ("idleDownB_marco");
										else if (painLevel < -0.34f)
												this.animator.Play ("idleDownA_marco");
										else if (painLevel < 0.34)
												this.animator.Play ("idleCorpse_marco");
										else if (painLevel < 0.66)
												this.animator.Play ("idleUpA_marco");
										else 
												this.animator.Play ("idleUpB_marco");
								}
								if (this.pState == PlayerState.WALKING) {
										if (painLevel < -0.66f)
												this.animator.Play ("walkCorpseE_marco");
										else if (painLevel < -0.34f)
												this.animator.Play ("walkCorpseD_marco");
										else if (painLevel < 0.34)
												this.animator.Play ("walkCorpseA_marco");
										else if (painLevel < 0.66)
												this.animator.Play ("walkCorpseB_marco");
										else 
												this.animator.Play ("walkCorpseC_marco");
								}
								if (this.pState == PlayerState.FALLEN) {
										this.animator.Play ("dropCorpse_marco");
								}
								if (this.pState == PlayerState.PREKISSING) {
										this.animator.Play ("preKiss_marco");
								}
								if (this.pState == PlayerState.KISSING) {
										this.animator.Play ("kiss_marco");
								}
								if (this.pState == PlayerState.POSTKISSING) {
										this.animator.Play ("postKiss_marco");
								}
								if (this.pState == PlayerState.DANCING) {
										this.animator.Play ("danceA_marco");
								}
								if (this.pState == PlayerState.PRETENDING) {
										this.animator.Play ("puppetA_marco");
								}
								if (this.pState == PlayerState.GRABING) {
									this.animator.Play ("grab_marco");
								}
								if (this.pState != PlayerState.KISSING && this.pState != PlayerState.PREKISSING) {
										Vector3 temp = transform.position; 
										temp.z = -0.1f;
										this.transform.position = temp;
								}
						}
					
				}
		}

	bool playersTooFar(){
		//print (Camera.main.pixelWidth);
		Vector3 p1Cord = Camera.main.WorldToScreenPoint (player1.transform.position);
		Vector3 p2Cord = Camera.main.WorldToScreenPoint (player2.transform.position);

		float distance = Vector3.Distance (p1Cord, p2Cord);
		//Vector3 screenPos = Camera.main.WorldToScreenPoint (distance	);
		if(Mathf.Abs(distance*1.4f)>Camera.main.pixelWidth ) {
			return true;
				}
		return false;
	}
		

}

