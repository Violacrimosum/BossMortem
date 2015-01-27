using UnityEngine;
using System.Collections;

public class Safehouse : Distractions {

	private float timer = 0;
	public float timerMax = 0;

	// Use this for initialization
	void Start () {
		shState = Distractions.DistractionsState.AVAILABLE;
	}

	void interact(){

	}

	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;

	}
}
