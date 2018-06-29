using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControl : MonoBehaviour {

	private void OnEnable() {
		Debug.Log("Appear Animation");
		var a =  GetComponent<Animation>();
		a["customBloc_Appear"].speed = 0.1f;
		a.Play();
	}
}
