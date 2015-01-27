using UnityEngine;
using System.Collections;

public class BossBodyManager : MonoBehaviour {

	public enum BossBodyState{
		INVISIBLE,
		VISIBLE
	}

	public GameObject bearer;
	public BossBodyState bbState;

	// Use this for initialization
	void Start () {
		bbState = BossBodyState.INVISIBLE;
		BoxCollider box = this.GetComponent<BoxCollider>();
		box.enabled = false;
		SpriteRenderer sRender = this.GetComponent<SpriteRenderer> ();
		sRender.enabled = false;
		//Rigidbody rBody = this.GetComponent<Rigidbody> ();
		//rBody.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {

		if (this.bbState == BossBodyState.INVISIBLE) {
						Vector3 tmp = bearer.gameObject.transform.position;
						tmp.z = 0;
						tmp.y += 3f;
						this.transform.position = bearer.gameObject.transform.position;
				}
		}

	void refreshState(){

	}
}
