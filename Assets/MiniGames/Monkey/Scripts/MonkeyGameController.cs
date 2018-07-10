	using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MonkeyGame{
		
	public class MonkeyGameController : MonoBehaviour {

		public Transform monkeyPos;

		public GameObject monkey;

		// Use this for initialization
		void Start () {
				monkey.transform.position = monkeyPos.position;
		}
		
		// Update is called once per frame
		void Update () {
			
		}
	}
}
