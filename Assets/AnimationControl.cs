using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControl : MonoBehaviour {

	public DoScan eventSource;

	public float timeout = 4.0f;

	float timeLeft;

	private void Start() {
		DoScan.scannedAllPOIs += OnAllScanned;
	}

	private void OnAllScanned(GameObject POI_Parent)
	{
		if(POI_Parent.name != transform.parent.name)
			return;
		
		enable();
	}

	private void enable() {
		GetComponent<Animator>().SetBool("Visible", true);
		timeLeft = timeout;
	}
	private void disable() {
		GetComponent<Animator>().SetBool("Visible", false);
	}
	private void Update() {		
		if(timeLeft < 0)
			disable();
		else
			timeLeft -= Time.deltaTime;
	}

            
}
