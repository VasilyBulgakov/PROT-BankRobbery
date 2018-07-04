using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FailSafe : MonoBehaviour {


	public float maxDist = 4.0f;
	public float maximumVerticalDist = 2.0f;
	GameObject canvas;
	// Use this for initialization
	void Start () {
		canvas = GameObject.Find("GamingCanvas");
	}
	
	// Update is called once per frame
	void Update () {
		if(canvas == null) return;

		if( canvas.transform.position.y - gameObject.transform.position.y > maximumVerticalDist)
			fix();

		if( (canvas.transform.position - gameObject.transform.position).sqrMagnitude > Mathf.Pow(maxDist, 2f) )
			fix();

	}

	public void fix()
	{
		transform.position = canvas.transform.position;

		Rigidbody rb = GetComponent<Rigidbody>();
		if(rb == null) return;
		rb.velocity = Vector3.zero;
	}
}
