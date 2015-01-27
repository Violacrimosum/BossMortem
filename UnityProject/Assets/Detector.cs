using UnityEngine;
using System.Collections;

public class Detector : MonoBehaviour {

	public int LevelNum;

	void OnTriggerEnter(Collider other){
		if (other.tag == "Player1" || other.tag == "Player2") {
						if (other.gameObject.GetComponent<PlayerMovement> ().pType == PlayerMovement.PlayerType.BEARER)
						this.loose ();
				} else if (other.tag == "bossBody") {
					this.loose ();
				}
	}

	void loose(){
		Application.LoadLevel (LevelNum);
	}

	void OnTriggerExit(Collider other){
		
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
