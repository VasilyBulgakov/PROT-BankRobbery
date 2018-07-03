using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class enable : MonoBehaviour {

	MakePuzle puzzle;
	ParticleSystem ps;
	// Use this for initialization
	void Start () {
		ps = GetComponent<ParticleSystem>();

		MakePuzle.completedPuzzle += OnCompletion;

		var em = ps.emission;
        em.enabled = false;
	}	
	private void OnCompletion()
	{
		var em = ps.emission;
        em.enabled = true;
	}
	
}
