using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControl : MonoBehaviour {

	private void OnEnable() {
		Debug.Log("Appear Animation");
			
		 GetComponent<Animation>().Play();
	}
}
