using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour {

	public GameObject selectionPrefab;
	bool canClick = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnMouseDrag() {		
		canClick = false;
	}
	private void OnMouseUp() {
		if( selectionPrefab == null ) return;
		if( !canClick ){
			canClick = true;
			return;
		}


		
	}

}
