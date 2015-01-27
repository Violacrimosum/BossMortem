using UnityEngine;
using System.Collections;

public class MovingDetector : Detector {

	public GameObject point1;
	public GameObject point2;
	private Vector3 movementVector;

	private float timer;
	private float timerMax;


	private bool isRight = false;
	private CharacterController characterController;

	// Use this for initialization
	void Start () {
		characterController = GetComponent<CharacterController>();
		timerMax = (Random.value * 2) + 2;
		timer = 0;
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
		if (isRight) {
			print (""+transform.position.x+" : "+ this.point1.transform.position.x);
				if (this.transform.position.x <= this.point1.transform.position.x) {
						print ("avanceRight");
								movementVector.x = 7;
						} else {
							print ("test");

								timer += Time.deltaTime;
						}
				}
		else{
				if(this.transform.position.x >= this.point2.transform.position.x){
				print ("avanceLeft");
					movementVector.x = -7;
				}
			else {
				print ("test2");
				timer += Time.deltaTime;
			}
			}
		if(timer > timerMax){
			timer = 0;
			isRight= !isRight;
			this.Flip();
		}
		characterController.Move (movementVector * Time.deltaTime);
}
}