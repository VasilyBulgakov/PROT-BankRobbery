using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonkeyGame{
	[RequireComponent(typeof(Rigidbody))]
	public class Throwable : MonoBehaviour {
		
		float gravity = 1f;

		public delegate void DestroyEvent();
    	public event DestroyEvent OnDestroyEvent;

	
		Rigidbody rb;

		bool collidedWithMonkey = false;

		// Use this for initialization
		void Start () {			
 			rb = GetComponent<Rigidbody>();
		}
		
		// Update is called once per frame
		void Update () {
			rb.AddForce( -Vector3.up * gravity);

			if(transform.position.y  < -10){							
				Destroy(gameObject);		
			}				
		}		

		private void OnDestroy() {
			OnDestroyEvent();
		}
		

		
		private void OnCollisionEnter(Collision other) {
			if(other.gameObject.tag == "Monkey")
			{
				other.gameObject.GetComponent<MonkeyActions>().interact(gameObject);				
			}
		}
	}
}