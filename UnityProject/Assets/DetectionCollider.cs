using UnityEngine;
using System.Collections;

public class DetectionCollider : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	private Safehouse shObject;

	void OnTriggerEnter (Collider other) {
		if (other.tag == "Alcove") {
			shObject = (Safehouse) other.GetComponent(typeof(Safehouse));	
			shObject.activate();
		}
	}
	
	void OnTriggerExit (Collider other) {
		if (other.tag == "Alcove") {
			shObject.deactivate ();
			shObject = (Safehouse) other.GetComponent(typeof(Safehouse));
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
}
