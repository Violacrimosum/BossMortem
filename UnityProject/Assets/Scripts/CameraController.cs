using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject Player;
	public GameObject Player2;

	// Use this for initialization
	void Start () {
		Physics.IgnoreLayerCollision (8, 8, true);
	}


	// Update is called once per frame
	void Update () {
		Camera.main.transform.position = new Vector3 ((Player.transform.position.x+Player2.transform.position.x)/2,
		                                              ((Player.transform.position.y+Player2.transform.position.y)*0.5f)+3.5f,
		                                              -12f);
	}
}
