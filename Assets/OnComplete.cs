using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnComplete : MonoBehaviour {


	ParticleSystem[] objs;
	// Use this for initialization
	void Start () {
		objs = GetComponentsInChildren<ParticleSystem>();
		foreach(var ps in objs)
		{
			var e = ps.emission;
			e.enabled = false;
		}

		MakePuzle.completedPuzzle += OnCompletin;
	}
	
	private void OnCompletin(){
		foreach(var ps in objs)
		{
			var e = ps.emission;
			e.enabled = true;
		}
	}
}
