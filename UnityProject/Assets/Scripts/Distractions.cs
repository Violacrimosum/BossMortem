using UnityEngine;
using System.Collections;

public class Distractions : MonoBehaviour {

	public enum DistractionsState{
		UNAVAILABLE,
		AVAILABLE,
		BEING_USED
	}

	enum DistractionType{
		ALCOVE,
		BLACKBOARD,
		DANSE
	}

	public GameObject[] elements;

	public Texture newTexture;

	protected DistractionsState shState;
	private DistractionType shType;
	// Use this for initialization
	void Start () {
		shState = DistractionsState.UNAVAILABLE;

		if(this.tag == "Alcove")
			shType = DistractionType.ALCOVE;
		else if(this.tag == "Blackboard")
			shType = DistractionType.BLACKBOARD;
		else if(this.tag == "Danse")
			shType = DistractionType.DANSE;
	}

	public DistractionsState getDState(){
		return this.shState;
		}

	public string interact(string type){
		string res = "nothing";
		if (type == "Scout") {
			if (this.shState == DistractionsState.UNAVAILABLE) {
				//play animations
				if (this.shType == DistractionType.ALCOVE) {
					//Explosion force + animation player scout
					foreach(GameObject child in elements){
						Vector3 pos = child.GetComponent<Transform>().position;
						pos.x += 2;
						pos.y += 4;
						pos.z += 7;
						child.rigidbody.AddExplosionForce(2500f, pos ,54, 3f);
					}

					res = "scouting";
				} else if (this.shType == DistractionType.BLACKBOARD) {
					//Do animations
					renderer.material.mainTexture = newTexture;
					res = "scouting";
				} else if (this.shType == DistractionType.DANSE) {
					//Do animations and start music
					res = "scouting";
				}
					//this.transform.localScale = new Vector3 (1.5f, 1f, 1f);
					this.shState = DistractionsState.AVAILABLE;
			}
		} else if (type == "Bearer") {
			if (this.shState == DistractionsState.AVAILABLE) {
				if (this.shType == DistractionType.ALCOVE) {
					//Explosion force + animation player scout
					res = "kiss";
				} else if (this.shType == DistractionType.BLACKBOARD) {
					//Do animations
					res = "pretend";
				} else if (this.shType == DistractionType.DANSE) {
					//Do animations and start music
					res = "danse";
				}
				//this.transform.localScale = new Vector3 (1.5f, 1.5f, 1f);
				this.shState = DistractionsState.BEING_USED;
			}
			else if(this.shState == DistractionsState.BEING_USED){
				if (this.shType == DistractionType.ALCOVE) {
					//Explosion force + animation player scout
				} else if (this.shType == DistractionType.BLACKBOARD) {
					//Do animations
				} else if (this.shType == DistractionType.DANSE) {
					//Do animations and start music
				}
				//this.transform.localScale = new Vector3 (1.5f, 1.5f, 1f);
				this.shState = DistractionsState.AVAILABLE;
			}
		}
		return res;
	}
	
	public void activate(){
		if(this.shState == DistractionsState.UNAVAILABLE)
			this.transform.localScale = new Vector3 (1.5f, 1f, 1f);
		else if(this.shState == DistractionsState.AVAILABLE)
			this.transform.localScale = new Vector3 (1.5f, 1.5f, 1f);
	}
	
	public void deactivate(){
		if(this.shState == DistractionsState.UNAVAILABLE)
			this.transform.localScale = new Vector3 (1f, 1f, 1f);
		else if(this.shState == DistractionsState.AVAILABLE)
			this.transform.localScale = new Vector3 (1.5f, 1f, 1f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
