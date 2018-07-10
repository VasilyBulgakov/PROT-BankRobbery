using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fpstest : MonoBehaviour {

	Vector3 lastMousePos;

	public float sens;
	bool first = true;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {		
		Vector3 delta = Input.mousePosition - lastMousePos;
		if(first){
			first = false;
			lastMousePos = Input.mousePosition;
			return;
		}
		Camera.main.transform.Rotate(Vector3.right, Input.GetAxis("Vertical") * Time.deltaTime * 20);		

		lastMousePos = Input.mousePosition;
	}
}
